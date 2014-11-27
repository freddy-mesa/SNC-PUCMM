package sncpucmm

import grails.converters.JSON
import grails.plugin.springsecurity.annotation.Secured
import org.codehaus.groovy.grails.web.json.JSONArray
import org.codehaus.groovy.grails.web.json.JSONObject

import static org.springframework.http.HttpStatus.*
import grails.transaction.Transactional

@Transactional(readOnly = true)
@Secured("permitAll")
class TourController {

    def springSecurityService
    static allowedMethods = [save: "POST", update: "PUT", delete: "DELETE"]

    def index(Integer max) {
        params.max = Math.min(max ?: 10, 100)
        respond Tour.list(params), model: [tourInstanceCount: Tour.count()]
    }

    def show(Tour tourInstance) {
        respond tourInstance
    }

    def create() {
        session.setAttribute("puntosReunion", null)
        respond new Tour(params)
    }

    @Transactional
    def save(Tour tourInstance) {
        if (tourInstance == null) {
            notFound()
            return
        }

        if (tourInstance.hasErrors()) {
            respond tourInstance.errors, view: 'create'
            return
        }

        tourInstance.creador = (Usuario)springSecurityService.getCurrentUser();
        tourInstance.fechaCreacion = new Date();

        tourInstance.save flush: true

        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.created.message', args: [message(code: 'tour.label', default: 'Tour'), tourInstance.id])
                redirect tourInstance
            }
            '*' { respond tourInstance, [status: CREATED] }
        }
    }

    def createFromJson() {
        def tourInstance = new Tour(request.JSON)
        tourInstance.save flush: true
    }

    def subscriber(){
        def usuarioTour = new UsuarioTour(request.JSON)
        usuarioTour.save flush: true

        /Crear detalleUsuarioTour/
    }

    def getUsuarioTour(){
        def jsonObject = request.JSON
        def usuarioTour = UsuarioTour.findById(1)
    }

    def updateUsuarioTour(){
        def jsonObject = request.JSON

        jsonObject.each {

        }
        //def usuarioTour = UsuarioTour.findBy
    }

    def edit(Tour tourInstance) {
        respond tourInstance
    }

    @Transactional
    def update(Tour tourInstance) {
        if (tourInstance == null) {
            notFound()
            return
        }

        if (tourInstance.hasErrors()) {
            respond tourInstance.errors, view: 'edit'
            return
        }

        tourInstance.save flush: true

        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.updated.message', args: [message(code: 'Tour.label', default: 'Tour'), tourInstance.id])
                redirect tourInstance
            }
            '*' { respond tourInstance, [status: OK] }
        }
    }

    @Transactional
    def delete(Tour tourInstance) {

        if (tourInstance == null) {
            notFound()
            return
        }

        tourInstance.delete flush: true

        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.deleted.message', args: [message(code: 'Tour.label', default: 'Tour'), tourInstance.id])
                redirect action: "index", method: "GET"
            }
            '*' { render status: NO_CONTENT }
        }
    }

    protected void notFound() {
        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.not.found.message', args: [message(code: 'tour.label', default: 'Tour'), params.id])
                redirect action: "index", method: "GET"
            }
            '*' { render status: NOT_FOUND }
        }
    }

    def list(){
        JSONObject jsonObject = new JSONObject()
        JSONArray jsonTours = new JSONArray()
        JSONArray jsonUsuarioTour = new JSONArray()

        def tours = Tour.list()
        def usuarioTours = UsuarioTour.list()

        if(tours.size() > 0){
            tours.each {
                JSONObject tour = new JSONObject();
                JSONObject punto
                JSONArray puntos = new JSONArray();

                tour.put("id", it.id)
                tour.put("idUsuarioCreador", it.creador.id)
                tour.put("nombreTour", it.nombreTour)
                tour.put("fechaCreacion", it.fechaCreacion.format("dd/MM/yyyy HH:mm:ss"))
                tour.put("fechaInicio", it.fechaInicio.format("dd/MM/yyyy HH:mm:ss"))
                if(tour.fechaFin)
                    tour.put("fechaFin", it.fechaFin.format("dd/MM/yyyy HH:mm:ss"))

                def puntosReunion = PuntoReunionTour.findAllByTour(it)
                puntosReunion.each { pr ->
                    punto = new JSONObject();
                    punto.put("id", pr.id)
                    punto.put("idNodo", pr.nodo.id)
                    punto.put("idTour", pr.tour.id)
                    punto.put("secuencia", pr.secuenciaPuntoReunion)
                    puntos.put(punto)
                }
                tour.put("PuntosReunion", puntos)
                jsonTours.put(tour)
            }

            usuarioTours.each { ut ->
                JSONObject userTour = new JSONObject();
                JSONObject detalle
                JSONArray detallesUT = new JSONArray();
                userTour.put("id", ut.id)
                userTour.put("idUsuario", ut.usuario.id)
                userTour.put("idTour", ut.tour.id)
                userTour.put("estado", ut.estado)
                if(ut.fechaInicio)
                    userTour.put("fechaInicio", ut.fechaInicio.format("dd/MM/yyyy HH:mm:ss"))
                if(ut.fechaFin)
                    userTour.put("fechaFin", ut.fechaFin.format("dd/MM/yyyy HH:mm:ss"))

                def detallesUsuarioTour = DetalleUsuarioTour.findAllByUsuarioTour(ut)
                detallesUsuarioTour.each { dut ->
                    detalle = new JSONObject()
                    detalle.put("id", dut.id)
                    detalle.put("idPuntoReunionTour", dut.puntoReunionTour.id)
                    if(dut.fechaInicio)
                        detalle.put("fechaInicio", dut.fechaInicio.format("dd/MM/yyyy HH:mm:ss"))
                    if(dut.fechaFin)
                        detalle.put("fechaFin", dut.fechaFin.format("dd/MM/yyyy HH:mm:ss"))
                    if(dut.fechaLlegada)
                        detalle.put("fechaLlegada", dut.fechaLlegada.format("dd/MM/yyyy HH:mm:ss"))
                    detallesUT.put(detalle)
                }
                userTour.put("DetalleUsuarioTourList", detallesUT)
                jsonUsuarioTour.put(userTour)
            }
            jsonObject.put("Tours", jsonTours)
            jsonObject.put("UsuariosTours", jsonUsuarioTour)
        }
        render jsonObject as JSON
    }

    def updateSubscriber(){
        JSONArray array = request.JSON

        array.each {
            JSONObject usuarioTour = ((JSONObject)it).getJSONObject("UsuarioTour")
            JSONArray detalleUsuarioTourList = ((JSONObject)it).getJSONArray("DetalleUsuarioTourList")

            if(usuarioTour.get("request") == "create"){
                UsuarioTour newUsuarioTour = new UsuarioTour(
                        usuario: Usuario.findById(usuarioTour.get("idUsuario")),
                        tour: Tour.findById(usuarioTour.get("idTour")),
                        estado: usuarioTour.get("estado"),
                        fechaInicio: Date.parse("dd/MM/yyyy HH:mm:ss", usuarioTour.get("fechaInicio")),
                        fechaFin: Date.parse("dd/MM/yyyy HH:mm:ss", usuarioTour.get("fechaFin")))
                newUsuarioTour = newUsuarioTour.save(flush: true, failOnError: true)

                detalleUsuarioTourList.each { detalle ->
                    detalle = (JSONObject)detalle
                    new DetalleUsuarioTour(
                            usuarioTour: newUsuarioTour,
                            puntoReunionTour: PuntoReunionTour.findById(detalle.get("idPuntoReunionTour")),
                            fechaInicio:  Date.parse("dd/MM/yyyy HH:mm:ss", detalle.get("fechaInicio")),
                            fechaFin:  Date.parse("dd/MM/yyyy HH:mm:ss", detalle.get("fechaFin")),
                            fechaLlegada: detalle.containsKey("fechaLlegada") ? Date.parse("dd/MM/yyyy HH:mm:ss", detalle.("fechaLlegada")) : null
                    ).save(flush: true, failOnError: true)
                }
            }
            else{
                UsuarioTour updateUsuarioTour = UsuarioTour.findByUsuarioAndTour(Usuario.findById(usuarioTour.get("idUsuario")), Tour.findById(usuarioTour.get("idTour")))
                updateUsuarioTour.estado = usuarioTour.get("estado")
                updateUsuarioTour.fechaInicio = Date.parse("dd/MM/yyyy HH:mm:ss", usuarioTour.get("fechaInicio"))
                updateUsuarioTour.fechaFin = Date.parse("dd/MM/yyyy HH:mm:ss",  usuarioTour.get("fechaFin"))
                updateUsuarioTour.save(flush: true)

                detalleUsuarioTourList.each { detalle ->
                    detalle = (JSONObject)detalle
                    DetalleUsuarioTour updateDUT = DetalleUsuarioTour.findByUsuarioTourAndPuntoReunionTour(updateUsuarioTour, PuntoReunionTour.findById(detalle.get("idPuntoReunionTour")))
                    updateDUT.puntoReunionTour = PuntoReunionTour.findById(detalle.get("idPuntoReunionTour"))
                    updateDUT.fechaInicio = Date.parse("dd/MM/yyyy HH:mm:ss", detalle.get("fechaInicio"))
                    updateDUT.fechaFin = Date.parse("dd/MM/yyyy HH:mm:ss", detalle.get("fechaFin"))
                    updateDUT.fechaLlegada = Date.parse("dd/MM/yyyy HH:mm:ss", detalle.get("fechaLlegada"))
                    updateDUT.save(flush: true)
                }
            }
        }
        redirect(action: "index")
    }

//    def puntosreunion(Tour tourInstance) {
//        respond new PuntoReunionTour(params), model: [tour: tourInstance.id , puntosReunion: PuntoReunionTour.findAllByTour(tourInstance)]
//    }


    def puntosreunion() {
        print("Entró")
        def puntosReunion = (Set<PuntoReunionTour>)session.getAttribute("puntosReunion")
        if(!puntosReunion){
            puntosReunion = [] as Set<PuntoReunionTour>
        }

        def puntoslist = params.list("puntoreuniontour")
        if(puntoslist){
            puntoslist.each {
                puntosReunion.add(new PuntoReunionTour(nodo: Nodo.findById(it), secuenciaPuntoReunion: 1))
            }
        }

        print(puntosReunion.size())
        puntosReunion.each {
            print(it.nodo.nombre)
        }
        session.setAttribute("puntosReunion", puntosReunion)
        [puntosReunion: puntosReunion]
    }

    @Transactional
    def savePuntos() {
        PuntoReunionTour puntoReunionTour = new PuntoReunionTour(nodo: Nodo.findById(params.puntoreuniontour), tour: Tour.findById(params.tour), secuenciaPuntoReunion: params.secuencia )

        if (puntoReunionTour == null) {
            notFound()
            return
        }

        if (puntoReunionTour.hasErrors()) {
            respond puntoReunionInstance.errors, view: 'create'
            return
        }
        puntoReunionTour.save(flush: true)
        redirect(action: "puntosreunion", id : params.tour)
    }
    def puntosreunionremover() {
        print("Entró")
        def puntosReunion = (Set<PuntoReunionTour>)session.getAttribute("puntosReunion")
        if(!puntosReunion){
            puntosReunion = [] as Set<PuntoReunionTour>
        }

        def puntoslist = params.list("puntosreuniontour")
        if(puntoslist){
            puntoslist.each {
                print("Weyyyyy!: " + it)
                puntosReunion.removeAll { j ->
                    if(j.nodo.nombre == it){
                        return true
                    }
                }
            }
        }
        session.setAttribute("puntosReunion", puntosReunion)
        respond(view: 'puntosreunion', model:[puntosReunion: puntosReunion])
    }
}
