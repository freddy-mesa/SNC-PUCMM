package sncpucmm

class VideoLlamada {

    Usuario usuario
    Date fechaInicio
    Date fechaFin
    String plataforma
    double longitud
    double latitud

    static constraints = {
        plataforma nullable: true
        fechaFin nullable: true
    }
}
