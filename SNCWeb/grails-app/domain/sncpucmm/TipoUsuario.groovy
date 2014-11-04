package sncpucmm

class TipoUsuario {

    String authority
    String description

    static mapping = {
        cache true
    }

    static constraints = {
        authority blank: false, unique: true
    }
}
