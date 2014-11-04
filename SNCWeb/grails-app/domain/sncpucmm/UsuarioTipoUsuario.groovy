package sncpucmm

import org.apache.commons.lang.builder.HashCodeBuilder

class UsuarioTipoUsuario implements Serializable {

    private static final long serialVersionUID = 1

    Usuario usuario
    TipoUsuario tipoUsuario

    boolean equals(other) {
        if (!(other instanceof UsuarioTipoUsuario)) {
            return false
        }

        other.usuario?.id == usuario?.id &&
                other.tipoUsuario?.id == tipoUsuario?.id
    }

    int hashCode() {
        def builder = new HashCodeBuilder()
        if (usuario) builder.append(usuario.id)
        if (tipoUsuario) builder.append(tipoUsuario.id)
        builder.toHashCode()
    }

    static UsuarioTipoUsuario get(long usuarioId, long tipoUsuarioId) {
        UsuarioTipoUsuario.where {
            usuario == Usuario.load(usuarioId) &&
                    tipoUsuario == TipoUsuario.load(tipoUsuarioId)
        }.get()
    }

    static boolean exists(long usuarioId, long tipoUsuarioId) {
        UsuarioTipoUsuario.where {
            usuario == Usuario.load(usuarioId) &&
                    tipoUsuario == TipoUsuario.load(tipoUsuarioId)
        }.count() > 0
    }

    static UsuarioTipoUsuario create(Usuario usuario, TipoUsuario tipoUsuario, boolean flush = false) {
        def instance = new UsuarioTipoUsuario(usuario: usuario, tipoUsuario: tipoUsuario)
        instance.save(flush: flush, insert: true)
        instance
    }

    static boolean remove(Usuario u, TipoUsuario r, boolean flush = false) {
        if (u == null || r == null) return false

        int rowCount = UsuarioTipoUsuario.where {
            usuario == Usuario.load(u.id) &&
                    tipoUsuario == TipoUsuario.load(r.id)
        }.deleteAll()

        if (flush) {
            UsuarioTipoUsuario.withSession { it.flush() }
        }

        rowCount > 0
    }

    static void removeAll(Usuario u, boolean flush = false) {
        if (u == null) return

        UsuarioTipoUsuario.where {
            usuario == Usuario.load(u.id)
        }.deleteAll()

        if (flush) {
            UsuarioTipoUsuario.withSession { it.flush() }
        }
    }

    static void removeAll(TipoUsuario r, boolean flush = false) {
        if (r == null) return

        UsuarioTipoUsuario.where {
            tipoUsuario == TipoUsuario.load(r.id)
        }.deleteAll()

        if (flush) {
            UsuarioTipoUsuario.withSession { it.flush() }
        }
    }

    static constraints = {
        tipoUsuario validator: { TipoUsuario r, UsuarioTipoUsuario ur ->
            if (ur.usuario == null) return
            boolean existing = false
            UsuarioTipoUsuario.withNewSession {
                existing = UsuarioTipoUsuario.exists(ur.usuario.id, r.id)
            }
            if (existing) {
                return 'userRole.exists'
            }
        }
    }

    static mapping = {
        id composite: ['tipoUsuario', 'usuario']
        version false
    }
}
