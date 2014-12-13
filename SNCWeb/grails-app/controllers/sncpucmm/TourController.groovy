package sncpucmm

import grails.converters.JSON
import grails.plugin.springsecurity.annotation.Secured
import org.codehaus.groovy.grails.web.json.JSONArray
import org.codehaus.groovy.grails.web.json.JSONObject

import static org.springframework.http.HttpStatus.*
import grails.transaction.Transactional

@Transactional(readOnly = true)
@Secured("ROLE_ADMIN")
class TourController {

    def springSecurityService
    def facebookContext
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

    def actualizartour(Tour tourInstance){
        if (tourInstance == null) {
            notFound()
            return
        }

        if (tourInstance.hasErrors()) {
            respond tourInstance.errors, view: 'edit'
            return
        }

        tourInstance.save flush: true

        def puntosReunion = (Set<Nodo>)session.getAttribute("puntosReunion")
        if(!puntosReunion){
            puntosReunion = [] as Set<Nodo>
        }

        def tours = PuntoReunionTour.findAllByTour(tourInstance)
        tours.each {
            it.delete(flush: true)
        }

        int i = 1
        puntosReunion.each {
            print(it.nombre)
           // tours.each {
                //if()
            //    it.delete(flush: true)
                new PuntoReunionTour(tour: tourInstance, nodo: it, secuenciaPuntoReunion: i).save(flush: true)
           // }

            i++
        }

        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.updated.message', args: [message(code: 'Tour.label', default: 'Tour'), tourInstance.id])
                redirect tourInstance
            }
            '*' { respond tourInstance, [status: OK] }
        }
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

        tourInstance = tourInstance.save flush: true

        def puntosReunion = (Set<Nodo>)session.getAttribute("puntosReunion")
        if(!puntosReunion){
            puntosReunion = [] as Set<Nodo>
        }

        int i = 1
        puntosReunion.each {
            new PuntoReunionTour(tour: tourInstance, nodo: it, secuenciaPuntoReunion: i).save(flush: true)
            i++
        }

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
        def puntosReunion = PuntoReunionTour.findAllByTour(tourInstance)
        def nodos = [] as Set<Nodo>

        puntosReunion.each {
            nodos.add(it.nodo)
        }
        session.setAttribute("puntosReunion", nodos)
        respond tourInstance, model: [puntosReunion: nodos]
    }

    @Transactional
    def update() {
        if (tourInstance == null) {
            notFound()
            return
        }

        if (tourInstance.hasErrors()) {
            respond tourInstance.errors, view: 'edit'
            return
        }

        tourInstance.save flush: true

        def puntosReunion = (Set<Nodo>)session.getAttribute("puntosReunion")
        if(!puntosReunion){
            puntosReunion = [] as Set<Nodo>
        }

        def tours = PuntoReunionTour.findAllByTour(tourInstance)
        tours.each {
            it.delete(flush: true)
        }

        int i = 1
        puntosReunion.each {
            new PuntoReunionTour(tour: tourInstance, nodo: it, secuenciaPuntoReunion: i).save(flush: true)
            i++
        }

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

//    def puntosreunion(Tour tourInstance) {
//        respond new PuntoReunionTour(params), model: [tour: tourInstance.id , puntosReunion: PuntoReunionTour.findAllByTour(tourInstance)]
//    }


    def puntosreunion() {
        def puntosReunion = (Set<Nodo>)session.getAttribute("puntosReunion")
        if(!puntosReunion){
            puntosReunion = [] as Set<Nodo>
        }

        def puntoslist = params.list("puntoreuniontour")
        if(puntoslist){
            puntoslist.each {
                puntosReunion.add(Nodo.findById(it))
            }
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
        def puntosReunion = (Set<Nodo>)session.getAttribute("puntosReunion")
        if(!puntosReunion){
            puntosReunion = [] as Set<Nodo>
        }

        def puntoslist = (Set<Nodo>)params.list("puntosreuniontour")
        if(puntoslist){
            puntoslist.each {
                for(int i = 0; i < puntosReunion.size(); i++){
                    if (puntosReunion[i].id == Integer.parseInt(it)){
                        puntosReunion = puntosReunion.minus(puntosReunion[i])
                        break
                    }
                }
            }
        }
        session.setAttribute("puntosReunion", puntosReunion)
        render(view: 'puntosreunion', model:[puntosReunion: puntosReunion])
    }

    @Secured(['permitAll'])
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

    @Secured(['permitAll'])
    def updateSubscriber(){
        JSONArray usuarioLocalizacionList = new JSONArray(params.usuarioLocalizacionList)
        if(usuarioLocalizacionList.size() > 0) {
            usuarioLocalizacionList.each { it ->
                JSONObject jsonObject = new JSONObject(it.toString())
                def facebookId = new Long(jsonObject.getString("idUsuario"))
                def nodoId = jsonObject.getInt("idNodo")
                def fechaLocalizacion = new Date(jsonObject.getString("fechaLocalizacion"))

                new LocalizacionUsuario(usuario: UsuarioFacebook.findByFacebookId(facebookId), nodo: Nodo.findById(nodoId), fechaLocalizacion: fechaLocalizacion).save(failOnError: true, flush: true)
            }
        }

        JSONArray usuarioTourList = new JSONArray(params.usuarioTourList)
        if(usuarioTourList.size() > 0) {
            usuarioTourList.each { it ->
                JSONObject jsonObject = new JSONObject(it.toString())

                JSONObject usuarioTour = jsonObject.getJSONObject("UsuarioTour")
                JSONArray detalleUsuarioTourList = jsonObject.getJSONArray("DetalleUsuarioTourList")

                if (usuarioTour.getString("request") == "create") {
                    UsuarioTour newUsuarioTour = new UsuarioTour(
                            usuario: UsuarioFacebook.findById(new Long(usuarioTour.getString("idUsuario"))),
                            tour: Tour.findById(usuarioTour.getInt("idTour")),
                            estado: usuarioTour.getString("estado"),
                            fechaInicio: Date.parse("dd/MM/yyyy HH:mm:ss", usuarioTour.getString("fechaInicio")),
                            fechaFin: Date.parse("dd/MM/yyyy HH:mm:ss", usuarioTour.getString("fechaFin"))
                    )
                    newUsuarioTour = newUsuarioTour.save(flush: true, failOnError: true)

                    detalleUsuarioTourList.each { detalle ->
                        JSONObject detalleUsuarioTour = new JSONObject(detalle.toString())
                        new DetalleUsuarioTour(
                                usuarioTour: newUsuarioTour,
                                puntoReunionTour: PuntoReunionTour.findById(detalleUsuarioTour.getInt("idPuntoReunionTour")),
                                fechaInicio: Date.parse("dd/MM/yyyy HH:mm:ss", detalleUsuarioTour.getString("fechaInicio")),
                                fechaFin: Date.parse("dd/MM/yyyy HH:mm:ss", detalleUsuarioTour.getString("fechaFin")),
                                fechaLlegada: detalleUsuarioTour.containsKey("fechaLlegada") ? Date.parse("dd/MM/yyyy HH:mm:ss", detalleUsuarioTour.getString("fechaLlegada")) : null
                        ).save(flush: true, failOnError: true)
                    }
                } else {
                    UsuarioTour updateUsuarioTour = UsuarioTour.findByUsuarioAndTour(UsuarioFacebook.findById(new Long(usuarioTour.getString("idUsuario"))), Tour.findById(usuarioTour.getInt("idTour")))
                    updateUsuarioTour.estado = usuarioTour.getString("estado")
                    updateUsuarioTour.fechaInicio = Date.parse("dd/MM/yyyy HH:mm:ss", usuarioTour.getString("fechaInicio"))
                    updateUsuarioTour.fechaFin = Date.parse("dd/MM/yyyy HH:mm:ss", usuarioTour.getString("fechaFin"))
                    updateUsuarioTour.save(flush: true)

                    detalleUsuarioTourList.each { detalle ->
                        JSONObject detalleUsuarioTour = new JSONObject(detalle.toString())
                        DetalleUsuarioTour updateDUT = DetalleUsuarioTour.findByUsuarioTourAndPuntoReunionTour(updateUsuarioTour, PuntoReunionTour.findById(detalleUsuarioTour.getInt("idPuntoReunionTour")))
                        updateDUT.puntoReunionTour = PuntoReunionTour.findById(detalleUsuarioTour.getInt("idPuntoReunionTour"))
                        updateDUT.fechaInicio = Date.parse("dd/MM/yyyy HH:mm:ss", detalleUsuarioTour.getString("fechaInicio"))
                        updateDUT.fechaFin = Date.parse("dd/MM/yyyy HH:mm:ss", detalleUsuarioTour.getString("fechaFin"))
                        updateDUT.fechaLlegada = Date.parse("dd/MM/yyyy HH:mm:ss", detalleUsuarioTour.getString("fechaLlegada"))
                        updateDUT.save(flush: true)
                    }
                }
            }
        }
        JSONObject jsonObject = new JSONObject()
        JSONArray jsonTours = new JSONArray()
        JSONArray jsonUsuarioTour = new JSONArray()

        def tours = Tour.findAllByFechaFinGreaterThanEquals(new Date())
        if(tours.size() > 0){
            tours.each { it ->
                JSONObject tour = new JSONObject();
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
                    JSONObject punto = new JSONObject();
                    punto.put("id", pr.id)
                    punto.put("idNodo", pr.nodo.id)
                    punto.put("idTour", pr.tour.id)
                    punto.put("secuencia", pr.secuenciaPuntoReunion)
                    puntos.put(punto)
                }

                tour.put("PuntosReunion", puntos)
                jsonTours.put(tour)
            }

            def usuarioTours = UsuarioTour.findAllByUsuario(UsuarioFacebook.findByFacebookId(new Long(params.id)))

            usuarioTours.each { ut ->
                JSONObject userTour = new JSONObject();
                JSONArray detallesUT = new JSONArray();

                userTour.put("id", ut.id)
                userTour.put("idUsuario", ut.usuario.facebookId.toString())
                userTour.put("idTour", ut.tour.id)
                userTour.put("estado", ut.estado)
                if(ut.fechaInicio)
                    userTour.put("fechaInicio", ut.fechaInicio.format("dd/MM/yyyy HH:mm:ss"))
                if(ut.fechaFin)
                    userTour.put("fechaFin", ut.fechaFin.format("dd/MM/yyyy HH:mm:ss"))

                def detallesUsuarioTour = DetalleUsuarioTour.findAllByUsuarioTour(ut)
                detallesUsuarioTour.each { dut ->
                    JSONObject detalle = new JSONObject()

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
}
