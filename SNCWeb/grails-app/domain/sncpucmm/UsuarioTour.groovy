package sncpucmm

class UsuarioTour {

    Usuario usuario
    Tour tour
    String estado
    Date fechaInicio
    Date fechaFin

    static constraints = {
        fechaFin nullable: true
    }
}
