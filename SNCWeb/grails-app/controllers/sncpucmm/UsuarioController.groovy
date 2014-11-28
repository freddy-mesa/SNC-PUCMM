package sncpucmm

import grails.plugin.springsecurity.annotation.Secured

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

    def facebooktest(){

    }
}
