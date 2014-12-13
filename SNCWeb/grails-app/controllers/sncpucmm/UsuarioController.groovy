package sncpucmm

import com.google.gson.JsonArray
import grails.converters.JSON
import grails.plugin.springsecurity.annotation.Secured
import org.codehaus.groovy.grails.web.json.JSONArray
import org.codehaus.groovy.grails.web.json.JSONObject

import static org.springframework.http.HttpStatus.*
import grails.transaction.Transactional

@Secured("ROLE_ADMIN")
@Transactional(readOnly = true)
class UsuarioController {

    static allowedMethods = [save: "POST", update: "PUT", delete: "DELETE"]

    def index(Integer max) {
        params.max = Math.min(max ?: 10, 100)
        respond Usuario.list(params), model:[usuarioInstanceCount: Usuario.count()]
    }

    def show(Usuario usuarioInstance) {
        respond usuarioInstance
    }

    def create() {
        respond new Usuario(params)
    }

    @Transactional
    def save(Usuario usuarioInstance) {
        if (usuarioInstance == null) {
            notFound()
            return
        }

        if (usuarioInstance.hasErrors()) {
            respond usuarioInstance.errors, view:'create'
            return
        }

        usuarioInstance.save flush:true

        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.created.message', args: [message(code: 'usuario.label', default: 'Usuario'), usuarioInstance.id])
                redirect usuarioInstance
            }
            '*' { respond usuarioInstance, [status: CREATED] }
        }
    }

    def edit(Usuario usuarioInstance) {
        respond usuarioInstance
    }

    @Transactional
    def update(Usuario usuarioInstance) {
        if (usuarioInstance == null) {
            notFound()
            return
        }

        if (usuarioInstance.hasErrors()) {
            respond usuarioInstance.errors, view:'edit'
            return
        }

        usuarioInstance.save flush:true

        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.updated.message', args: [message(code: 'Usuario.label', default: 'Usuario'), usuarioInstance.id])
                redirect usuarioInstance
            }
            '*'{ respond usuarioInstance, [status: OK] }
        }
    }

    @Transactional
    def delete(Usuario usuarioInstance) {

        if (usuarioInstance == null) {
            notFound()
            return
        }

        usuarioInstance.delete flush:true

        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.deleted.message', args: [message(code: 'Usuario.label', default: 'Usuario'), usuarioInstance.id])
                redirect action:"index", method:"GET"
            }
            '*'{ render status: NO_CONTENT }
        }
    }

    protected void notFound() {
        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.not.found.message', args: [message(code: 'usuario.label', default: 'Usuario'), params.id])
                redirect action: "index", method: "GET"
            }
            '*'{ render status: NOT_FOUND }
        }
    }

    @Secured(['permitAll'])
    def crear(){
        println "crear -> " + params.id
        def usuarioSearch = UsuarioFacebook.findByFacebookId(new Long(params.id))

        if(!usuarioSearch){
            new UsuarioFacebook(
                facebookId: new Long(params.id),
                fistname: params.fist_name,
                lastname: params.last_name,
                gender: params.gender,
                email: params.email
            ).save(flush: true, failOnError: true)
        }
    }

    //Recibe una petición de following
    @Secured(['permitAll'])
    def followRequest(){
        println "followRequest -> " + params.json
        JSONObject request = new JSONObject(params.json)
        JSONArray users = new JSONArray(request.get("usuarios").toString())

        def usuario = UsuarioFacebook.findByFacebookId(new Long(request.getString("id")))

        users.each {
            def followed = UsuarioFacebook.findByFacebookId(new Long(it))
            new FollowUsuario(follower: usuario, followed: followed, estadoSolicitud: "pending", fechaRegistroSolicitud: new Date()).save(flush: true, failOnError: true)
        }
    }

    //Recibe la respuesta de solicitud de una petición de following.
    @Secured(['permitAll'])
    def followResponse(){
        println "followResponse -> " + params.followed + " " + params.status + " " + params.follower
        def followUsuario = FollowUsuario.findByFollowerAndFollowed(UsuarioFacebook.findByFacebookId(new Long(params.follower)), UsuarioFacebook.findByFacebookId(new Long(params.followed)))
        //println followUsuario
        if(params.status == "denied"){
            followUsuario.delete(flush:true)
        }
        else if (params.status == "accepted"){
            followUsuario.estadoSolicitud = "accepted"
            followUsuario.save(flush: true)
        }
    }

    @Secured(['permitAll'])
    def notifyFollowingRequest(){
        println "Send Notifications to " + params.id
        def list = FollowUsuario.findAllByFollowedAndEstadoSolicitud(UsuarioFacebook.findByFacebookId(new Long(params.id)), "pending")
        //println list
        JSONArray requests = new JSONArray()
        //println "Lista: "
        list.each {
            //println "Followed: " + it.followed.facebookId + " Follower: " + it.follower.facebookId
            JSONObject request = new JSONObject()
            request.put("id", it.follower.facebookId.toString())
            request.put("name", it.follower.fistname + " " + it.follower.lastname)
            requests.add(request)
        }
        render requests as JSON
    }

    //Usuarios a los que un usuario sigue.
    @Secured(['permitAll'])
    def following(){
        println "Get following (Accept or Pending) friend: -> " + params.id
        def usuario = UsuarioFacebook.findByFacebookId(new Long(params.id))
        def followedAccepted = (Set<FollowUsuario>)FollowUsuario.findAllWhere(follower: usuario, estadoSolicitud:  "accepted")
        def followedPending = (Set<FollowUsuario>)FollowUsuario.findAllWhere(follower: usuario,estadoSolicitud:  "pending")
        def followerAccepted = (Set<FollowUsuario>)FollowUsuario.findAllWhere(followed: usuario, estadoSolicitud:  "accepted")
        def followerPending = (Set<FollowUsuario>)FollowUsuario.findAllWhere(followed: usuario,estadoSolicitud:  "pending")

        def following = followedAccepted + followedPending + followerAccepted + followerPending

        JSONArray list = new JSONArray()
        following.each {
            JSONObject user = new JSONObject()
            user.put("id", it.followed.facebookId.toString())
            list.add(user)
        }
        render list as JSON
    }

    @Secured(['permitAll'])
    def friends(){
        def usuario = UsuarioFacebook.findByFacebookId(new Long(params.id))
        JSONArray list = new JSONArray()

        def follower = FollowUsuario.findAllWhere(followed: usuario, estadoSolicitud: "accepted")
        follower.each {it ->
            def user = LocalizacionUsuario.findByUsuario(it.follower)
            JSONObject jsonObject = new JSONObject();

            jsonObject.put("id",user.usuario.facebookId.toString())
            jsonObject.put("nombre",user.usuario.fistname + " " + user.usuario.lastname)
            jsonObject.put("ubicacion",user.nodo.ubicacion.abreviacion)
            jsonObject.put("fecha",user.fechaLocalizacion)

            list.add(jsonObject)
        }

        def followed = FollowUsuario.findAllWhere(follower: usuario, estadoSolicitud: "accepted")
        followed.each {it ->
            def user = LocalizacionUsuario.findByUsuario(it.followed)
            JSONObject jsonObject = new JSONObject();

            jsonObject.put("id",user.usuario.facebookId.toString())
            jsonObject.put("nombre",user.usuario.fistname + " " + user.usuario.lastname)
            jsonObject.put("ubicacion",user.nodo.ubicacion.abreviacion)
            jsonObject.put("fecha",user.fechaLocalizacion)

            list.add(jsonObject)
        }

        render list as JSON
    }

    @Secured(['permitAll'])
    def notifySharedLocationRequest() {
        def usuario = UsuarioFacebook.findByFacebookId(new Long(params.id))

        def list = CompartirUsuario.findAllByReceiverAndCompartido(usuario, false)
        JSONArray json = new JSONArray()
        list.each {
            it.compartido = true
            it.save(failOnError: true, flush: true)

            JSONObject jsonObject = new JSONObject()
            jsonObject.put("sender", it.sender.facebookId.toString())
            jsonObject.put("mensaje", it.compartirUbicacion.mensaje)
            jsonObject.put("nodo", it.compartirUbicacion.nodo.id)
            json.add(jsonObject)
        }

        render json as JSON
    }

    @Secured(['permitAll'])
    def sharedLocationRequest() {
        def sender = UsuarioFacebook.findByFacebookId(new Long(params.id))

        JSONObject jsonObject = new JSONObject(params.json)
        JSONArray array = new JSONArray(jsonObject.getString("friends"))

        def mensaje = jsonObject.getString("message")
        def nodo = jsonObject.getInt("idNodo")

        array.each {
            def compartirUbicacion = new CompartirUbicacion(mensaje: mensaje, nodo: Nodo.findById(nodo)).save(flush: true, failOnError: true)

            def receiver = UsuarioFacebook.findByFacebookId(new Long(params.id))
            new CompartirUsuario(sender: sender, receiver: receiver, compartirUbicacion: compartirUbicacion).save(flush: true, failOnError: true)
        }
    }
}
