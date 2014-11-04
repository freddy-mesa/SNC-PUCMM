package sncpucmm

class Tour {

    Usuario creador
    String  nombreTour
    Date fechaCreacion
    Date fechaInicio
    Date fechaFin

    static constraints = {
        fechaFin nullable: true
    }
}
