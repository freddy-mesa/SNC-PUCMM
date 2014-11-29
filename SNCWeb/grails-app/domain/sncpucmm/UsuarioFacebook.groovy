package sncpucmm

class UsuarioFacebook {

    Integer facebookId
    String fistname
    String lastname
    String gender
    String email

    static constraints = {
        facebookId unique: true
        email email: true
    }
}
