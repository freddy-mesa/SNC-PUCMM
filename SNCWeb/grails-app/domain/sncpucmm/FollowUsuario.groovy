package sncpucmm

class FollowUsuario {

    Usuario follower
    Usuario followed
    String estadoSolicitud
    Date fechaRegistroSolicitud
    Date fechaRespuestaSolicitud

    static constraints = {
        fechaRespuestaSolicitud nullable: true
    }
}
