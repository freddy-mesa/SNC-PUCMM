import groovy.sql.Sql
import sncpucmm.TipoUsuario
import sncpucmm.Usuario
import sncpucmm.UsuarioTipoUsuario

class BootStrap {

    def grailsApplication
    def init = { servletContext ->
        //Creando tipos de usuarios
        def tipoAdmin = new TipoUsuario(authority: "ROLE_ADMIN", description: "Administrador del sistema.").save(failOnError: true);
        def tipoUsuario = new TipoUsuario(authority: "ROLE_USUARIO", description: "Usuario del sistema.").save(failOnError: true);

        //Creando usuarios
        def admin = new Usuario(username: "admin", password: "admin", name: "admin", lastname: "admin", accountExpired: false, accountLocked: false, passwordExpired: false).save(failOnError: true);
        def usuario = new Usuario(username: "usuario", password: "usuario", name: "usuario", lastname: "usuario", accountExpired: false, accountLocked: false, passwordExpired: false).save(failOnError: true);

        //Asignando usuarios a su tipo.
        UsuarioTipoUsuario.create(admin, tipoAdmin, true);
        UsuarioTipoUsuario.create(usuario, tipoUsuario, true);

    }
    def destroy = {
    }
}