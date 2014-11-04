package sncpucmm

class DetalleUsuarioTour {

    UsuarioTour usuarioTour
    PuntoReunionTour puntoReunionTour
    Date fechaInicio
    Date fechaFin
    Date fechaLlegada

    static constraints = {
        fechaFin nullable: true
        fechaLlegada nullable: true
    }
}
