package sncpucmm

class UsuarioTour {

    UsuarioFacebook usuario
    Tour tour
    String estado
    Date fechaInicio
    Date fechaFin

    static constraints = {
        fechaFin nullable: true
    }
}
