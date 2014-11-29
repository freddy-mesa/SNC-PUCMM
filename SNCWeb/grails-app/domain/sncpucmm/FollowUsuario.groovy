package sncpucmm

class FollowUsuario {

    UsuarioFacebook follower
    UsuarioFacebook followed
    String estadoSolicitud
    Date fechaRegistroSolicitud
    Date fechaRespuestaSolicitud

    static constraints = {
        fechaRespuestaSolicitud nullable: true
    }
}
