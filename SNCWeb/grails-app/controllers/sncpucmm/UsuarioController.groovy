package sncpucmm

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

    def crear(){
        JSONObject usuario = request.JSON

        def usuarioSearch = Usuario.findById(usuario.get("id"))

        if(!usuarioSearch){
            new UsuarioFacebook(
                    facebookId: usuario.get("id"),
                    fistname: usuario.get("fist_name"),
                    lastname: usuario.get("last_name"),
                    gender: usuario.get("gender"),
                    email: usuario.get("email")).save(flush: true)
        }
    }

    //Recibe una petición de following
    def followRequest(){
        JSONObject request = request.JSON
        JSONArray users = (JSONArray)request.get("usuarios")
        def usuario = UsuarioFacebook.findByFacebookId(request.get("id"))
        users.each {
            def followed = UsuarioFacebook.findByFacebookId(((JSONObject)it).get("id"))
            new FollowUsuario(follower: usuario, followed: followed, estadoSolicitud: "pending", fechaRegistroSolicitud: new Date()).save(flush: true)
        }
    }

    //Recibe la respuesta de solicitud de una petición de following.
    def followResponse(){
        def followUsuario = FollowUsuario.findByFollowerAndFollowed(params.followers, params.followed)
        if(params.response == "denied"){
            followUsuario.delete(flush:true)
        }
        else if (params.response == "accepted"){
            followUsuario.estadoSolicitud = "accepted"
            followUsuario.save(flush: true)
        }
    }

    def noticyRequest(){
        def list = FollowUsuario.findAllByFollowedAndEstadoSolicitud(params.id, "pending")
        JSONArray requests = new JSONArray()
        list.each {
            JSONObject request = new JSONObject()
            request.put("id", it.follower.facebookId)
            request.put("name", it.follower.fistname)
            request.put("lastname", it.follower.lastname)
            requests.add(request)
        }
        return requests
    }

    //Usuarios a los que un usuario sigue.
    def following(){
        def following = (Set<FollowUsuario>)FollowUsuario.findAllByFollowerAndEstadoSolicitud(UsuarioFacebook.findByFacebookId(params.id), "accepted")

        JSONArray list = new JSONArray()
        following.each {
            JSONObject user = new JSONObject()
            user.put("id", it.followed.facebookId)
            list.add(user)
        }
        return list
    }
}
