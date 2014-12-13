package sncpucmm

class Usuario {

    transient springSecurityService

    UsuarioFacebook usuarioFacebook
    String username
    String password
    boolean enabled = true
    boolean accountExpired
    boolean accountLocked
    boolean passwordExpired

    static transients = ['springSecurityService']

    static constraints = {
        username blank: false, unique: true
        password blank: false
        usuarioFacebook nullable: true
    }

    static mapping = {
        password column: '`password`'
    }

    Set<TipoUsuario> getAuthorities() {
        UsuarioTipoUsuario.findAllByUsuario(this).collect { it.tipoUsuario }
    }

    def beforeInsert() {
        encodePassword()
    }

    def beforeUpdate() {
        if (isDirty('password')) {
            encodePassword()
        }
    }

    protected void encodePassword() {
        password = springSecurityService?.passwordEncoder ? springSecurityService.encodePassword(password) : password
    }
}
