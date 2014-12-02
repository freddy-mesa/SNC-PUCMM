package sncpucmm

class UsuarioFacebook {

    Long facebookId
    String fistname
    String lastname
    String gender
    String email

    static constraints = {
        facebookId unique: true
        //email email: true
        email nullable: true
    }
}
