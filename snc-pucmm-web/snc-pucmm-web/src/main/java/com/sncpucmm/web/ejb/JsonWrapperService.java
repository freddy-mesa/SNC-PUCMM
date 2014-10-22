/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.sncpucmm.web.ejb;

import com.sncpucmm.web.domain.Cuentafacebook;
import com.sncpucmm.web.domain.Detalleusuariotour;
import com.sncpucmm.web.domain.Followusuario;
import com.sncpucmm.web.domain.Localizacion;
import com.sncpucmm.web.domain.Localizacionusuario;
import com.sncpucmm.web.domain.Puntoreuniontour;
import com.sncpucmm.web.domain.Tipousuario;
import com.sncpucmm.web.domain.Tour;
import com.sncpucmm.web.domain.Ubicacion;
import com.sncpucmm.web.domain.Usuario;
import com.sncpucmm.web.domain.Usuariotour;
import com.sncpucmm.web.domain.Videollamada;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.ejb.Stateless;
import org.json.JSONObject;

/**
 *
 * @author Freddy Mesa
 */
@Stateless
public class JsonWrapperService {

    private Object hasJsonValue(JSONObject json, String key) {
        Object obj = null;

        if (json.has(key)) {
            obj = json.get(key);
        }

        return obj;
    }

    private Date convertToDate(String source) {
        Date date = null;
        try {
            date = new SimpleDateFormat("dd/MM/yyyy HH:mm:ss").parse(source);
        } catch (ParseException ex) {
            Logger.getLogger(JsonWrapperService.class.getName()).log(Level.SEVERE, null, ex);
        }

        return date;
    }

    private String dateToString(Date source) {
        return new SimpleDateFormat("dd/MM/yyyy HH:mm:ss").format(source);
    }

    public String cuentaFacebookToJson(Cuentafacebook cuentafacebook) {
        JSONObject json = new JSONObject();
        json.put("idcuentafacebook", cuentafacebook.getIdcuentafacebook());
        json.put("usuariofacebook", cuentafacebook.getUsuariofacebook());
        json.put("token", cuentafacebook.getToken());

        return json.toString();
    }

    public String detalleUsuarioTourToJson(Detalleusuariotour detalleUsuarioTour) {
        JSONObject json = new JSONObject();
        json.put("iddetalleusuariotour", detalleUsuarioTour.getIddetalleusuariotour());
        json.put("estado", detalleUsuarioTour.getEstado());
        json.put("fechainicio", dateToString(detalleUsuarioTour.getFechainicio()));
        json.put("fechallegada", dateToString(detalleUsuarioTour.getFechallegada()));
        json.put("idpuntoreunion", puntoReunionTourToJson(detalleUsuarioTour.getIdpuntoreunion()));
        json.put("idusuariotour", usuarioTourToJson(detalleUsuarioTour.getIdusuariotour()));

        return json.toString();
    }

    public String followUsuarioToJson(Followusuario followUsuario) {
        JSONObject json = new JSONObject();
        json.put("idfollow", followUsuario.getIdfollow());
        json.put("estadosolicitud", followUsuario.getEstadosolicitud());
        json.put("fecharegistrosolicitud", dateToString(followUsuario.getFecharegistrosolicitud()));
        json.put("fecharespuestasolicitud", dateToString(followUsuario.getFecharespuestasolicitud()));
        json.put("idusuariofollower", usuarioToJson(followUsuario.getIdusuariofollower()));
        json.put("idusuariofollowed", usuarioToJson(followUsuario.getIdusuariofollowed()));

        return json.toString();
    }

    public String localizacionToJson(Localizacion localizacion) {
        JSONObject json = new JSONObject();
        json.put("idlocalizacion", localizacion.getIdlocalizacion());
        json.put("nombre", localizacion.getNombre());
        json.put("idubicacion", ubicacionToJson(localizacion.getIdubicacion()));
        return json.toString();
    }

    public String localizacionUsuarioToJson(Localizacionusuario localizacionUsuario) {
        JSONObject json = new JSONObject();
        json.put("idlocalizacionusuario", localizacionUsuario.getIdlocalizacionusuario());
        json.put("fechalocalizacion", dateToString(localizacionUsuario.getFechalocalizacion()));
        json.put("idlocalizacion", localizacionToJson(localizacionUsuario.getIdlocalizacion()));
        json.put("idusuario", usuarioToJson(localizacionUsuario.getIdusuario()));

        return json.toString();
    }

    public String puntoReunionTourToJson(Puntoreuniontour puntoReunionTour) {
        JSONObject json = new JSONObject();
        json.put("idpuntoreunion", puntoReunionTour.getIdpuntoreunion());
        json.put("secuenciapuntoreunion", puntoReunionTour.getSecuenciapuntoreunion());
        json.put("idtour", tourToJson(puntoReunionTour.getIdtour()));
        json.put("idlocalizacion", localizacionToJson(puntoReunionTour.getIdlocalizacion()));
        return json.toString();
    }

    public String tipoUsuarioToJson(Tipousuario tipoUsuario) {
        JSONObject json = new JSONObject();
        json.put("idtipousuario", tipoUsuario.getIdtipousuario());
        json.put("nombre", tipoUsuario.getNombre());
        json.put("descripcion", tipoUsuario.getDescripcion());

        return json.toString();
    }

    public String tourToJson(Tour tour) {
        JSONObject json = new JSONObject();
        json.put("idtour", tour.getIdtour());
        json.put("nombretour", tour.getNombretour());
        json.put("fechacreacion", dateToString(tour.getFechacreacion()));
        json.put("fechainicio", dateToString(tour.getFechainicio()));
        json.put("fechafin", dateToString(tour.getFechafin()));
        if (tour.getIdusuario() != null) {
            json.put("idusuario", usuarioToJson(tour.getIdusuario()));
        }
        
        return json.toString();
    }

    public String ubicacionToJson(Ubicacion ubicacion) {
        JSONObject json = new JSONObject();
        json.put("idubicacion", ubicacion.getIdubicacion());
        json.put("nombre", ubicacion.getNombre());
        json.put("abreviacion", ubicacion.getAbreviacion());
        return json.toString();
    }

    public String usuarioToJson(Usuario usuario) {
        JSONObject json = new JSONObject();
        json.put("idusuario", usuario.getIdusuario());
        json.put("nombre", usuario.getNombre());
        json.put("apellido", usuario.getApellido());
        json.put("usuario", usuario.getUsuario());
        json.put("contrasena", usuario.getContrasena());
        if (usuario.getIdtipousuario() != null) {
            json.put("tipousuario", tipoUsuarioToJson(usuario.getIdtipousuario()));
        }
        if (usuario.getIdcuentafacebook() != null) {
            json.put("cuentafacebook", cuentaFacebookToJson(usuario.getIdcuentafacebook()));
        }
        return json.toString();
    }

    public String usuarioTourToJson(Usuariotour usuarioTour) {
        JSONObject json = new JSONObject();
        json.put("idusuariotour", usuarioTour.getIdusuariotour());
        json.put("estadousuariotour", usuarioTour.getEstadousuariotour());
        json.put("fechainicio", dateToString(usuarioTour.getFechainicio()));
        json.put("fechafin", dateToString(usuarioTour.getFechafin()));
        json.put("idtour", tourToJson(usuarioTour.getIdtour()));
        json.put("idusuario", usuarioToJson(usuarioTour.getIdusuario()));
        return json.toString();
    }

    public String videoLlamadaToJson(Videollamada videollamada) {
        JSONObject json = new JSONObject();

        json.put("idvideollamada", videollamada.getIdvideollamada());
        json.put("fechainicio", dateToString(videollamada.getFechainicio()));
        json.put("fechafin", dateToString(videollamada.getFechafin()));
        json.put("plataforma", videollamada.getPlataforma());
        json.put("longitud", videollamada.getLongitud());
        json.put("latitud", videollamada.getLatitud());
        json.put("idusuario", usuarioToJson(videollamada.getIdusuario()));

        return json.toString();
    }

    public Cuentafacebook jsonToCuentaFacebook(String source) {
        Cuentafacebook cuentafacebook = new Cuentafacebook();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idcuentafacebook") != null) {
            cuentafacebook.setIdcuentafacebook((Integer) hasJsonValue(json, "idcuentafacebook"));
        }
        if (hasJsonValue(json, "usuariofacebook") != null) {
            cuentafacebook.setUsuariofacebook((String) hasJsonValue(json, "usuariofacebook"));
        }
        if (hasJsonValue(json, "token") != null) {
            cuentafacebook.setToken((String) hasJsonValue(json, "token"));
        }

        return cuentafacebook;
    }

    public Detalleusuariotour jsonToDetalleUsuarioTour(String source) {
        Detalleusuariotour detalleUsuarioTour = new Detalleusuariotour();

        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "iddetalleusuariotour") != null) {
            detalleUsuarioTour.setIddetalleusuariotour((Integer) hasJsonValue(json, "iddetalleusuariotour"));
        }
        if (hasJsonValue(json, "estado") != null) {
            detalleUsuarioTour.setEstado((String) hasJsonValue(json, "estado"));
        }
        if (hasJsonValue(json, "fechainicio") != null) {
            detalleUsuarioTour.setFechainicio(convertToDate((String) hasJsonValue(json, "fechainicio")));
        }
        if (hasJsonValue(json, "fechallegada") != null) {
            detalleUsuarioTour.setFechallegada(convertToDate((String) hasJsonValue(json, "fechallegada")));
        }
        if (hasJsonValue(json, "idpuntoreunion") != null) {
            detalleUsuarioTour.setIdpuntoreunion(jsonToPuntoReunionTour((String) hasJsonValue(json, "idpuntoreunion")));
        }
        if (hasJsonValue(json, "idusuariotour") != null) {
            detalleUsuarioTour.setIdusuariotour(jsonToUsuarioTour((String) hasJsonValue(json, "idusuariotour")));
        }

        return detalleUsuarioTour;
    }

    public Followusuario jsonToFollowUsuario(String source) {
        Followusuario followusuario = new Followusuario();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idfollow") != null) {
            followusuario.setIdfollow((Integer) hasJsonValue(json, "idfollow"));
        }
        if (hasJsonValue(json, "estadosolicitud") != null) {
            followusuario.setEstadosolicitud((String) hasJsonValue(json, "estadosolicitud"));
        }
        if (hasJsonValue(json, "fecharegistrosolicitud") != null) {
            followusuario.setFecharegistrosolicitud(convertToDate((String) hasJsonValue(json, "fecharegistrosolicitud")));
        }
        if (hasJsonValue(json, "fecharespuestasolicitud") != null) {
            followusuario.setFecharegistrosolicitud(convertToDate((String) hasJsonValue(json, "fecharespuestasolicitud")));
        }
        if (hasJsonValue(json, "idusuariofollower") != null) {
            followusuario.setIdusuariofollower(jsonToUsuario((String) hasJsonValue(json, "idusuariofollower")));
        }
        if (hasJsonValue(json, "idusuariofollowed") != null) {
            followusuario.setIdusuariofollowed(jsonToUsuario((String) hasJsonValue(json, "idusuariofollowed")));
        }

        return followusuario;
    }

    public Localizacion jsonToLocalizacion(String source) {
        Localizacion localizacion = new Localizacion();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idlocalizacion") != null) {
            localizacion.setIdlocalizacion((Integer) hasJsonValue(json, "idlocalizacion"));
        }
        if (hasJsonValue(json, "nombre") != null) {
            localizacion.setNombre((String) hasJsonValue(json, "nombre"));
        }
        if (hasJsonValue(json, "idubicacion") != null) {
            localizacion.setIdubicacion(jsonToUbicacion((String) hasJsonValue(json, "idubicacion")));
        }

        return localizacion;
    }

    public Localizacionusuario jsonToLocalizacionUsuario(String source) {
        Localizacionusuario localizacionusuario = new Localizacionusuario();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idlocalizacionusuario") != null) {
            localizacionusuario.setIdlocalizacionusuario((Integer) hasJsonValue(json, "idlocalizacionusuario"));
        }
        if (hasJsonValue(json, "fechalocalizacion") != null) {
            localizacionusuario.setFechalocalizacion(convertToDate((String) hasJsonValue(json, "fechalocalizacion")));
        }
        if (hasJsonValue(json, "idlocalizacion") != null) {
            localizacionusuario.setIdlocalizacion(jsonToLocalizacion((String) hasJsonValue(json, "idlocalizacion")));
        }
        if (hasJsonValue(json, "idusuario") != null) {
            localizacionusuario.setIdusuario(jsonToUsuario((String) hasJsonValue(json, "idusuario")));
        }

        return localizacionusuario;
    }

    public Puntoreuniontour jsonToPuntoReunionTour(String source) {
        Puntoreuniontour puntoreuniontour = new Puntoreuniontour();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idpuntoreunion") != null) {
            puntoreuniontour.setIdpuntoreunion((Integer) hasJsonValue(json, "idpuntoreunion"));
        }
        if (hasJsonValue(json, "secuenciapuntoreunion") != null) {
            puntoreuniontour.setSecuenciapuntoreunion((Integer) hasJsonValue(json, "secuenciapuntoreunion"));
        }
        if (hasJsonValue(json, "idlocalizacion") != null) {
            puntoreuniontour.setIdlocalizacion(jsonToLocalizacion((String) hasJsonValue(json, "idlocalizacion")));
        }
        if (hasJsonValue(json, "idtour") != null) {
            puntoreuniontour.setIdtour(jsonToTour((String) hasJsonValue(json, "idtour")));
        }

        return puntoreuniontour;
    }

    public Tipousuario jsonToTipoUsuario(String source) {
        Tipousuario tipousuario = new Tipousuario();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idtipousuario") != null) {
            tipousuario.setIdtipousuario((Integer) hasJsonValue(json, "idtipousuario"));
        }
        if (hasJsonValue(json, "nombre") != null) {
            tipousuario.setNombre((String) hasJsonValue(json, "nombre"));
        }
        if (hasJsonValue(json, "descripcion") != null) {
            tipousuario.setDescripcion((String) hasJsonValue(json, "descripcion"));
        }

        return tipousuario;
    }

    public Tour jsonToTour(String source) {
        Tour tour = new Tour();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idtour") != null) {
            tour.setIdtour((Integer) hasJsonValue(json, "idtour"));
        }
        if (hasJsonValue(json, "nombretour") != null) {
            tour.setNombretour((String) hasJsonValue(json, "nombretour"));
        }
        if (hasJsonValue(json, "fechacreacion") != null) {
            tour.setFechacreacion(convertToDate((String) hasJsonValue(json, "fechacreacion")));
        }
        if (hasJsonValue(json, "fechainicio") != null) {
            tour.setFechainicio(convertToDate((String) hasJsonValue(json, "fechainicio")));
        }
        if (hasJsonValue(json, "fechafin") != null) {
            tour.setFechafin(convertToDate((String) hasJsonValue(json, "fechafin")));
        }
        if (hasJsonValue(json, "idusuario") != null) {
            tour.setIdusuario(jsonToUsuario((String) hasJsonValue(json, "idusuario")));
        }

        return tour;
    }

    public Ubicacion jsonToUbicacion(String source) {
        Ubicacion ubicacion = new Ubicacion();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idubicacion") != null) {
            ubicacion.setIdubicacion((Integer) hasJsonValue(json, "idubicacion"));
        }
        if (hasJsonValue(json, "nombre") != null) {
            ubicacion.setNombre((String) hasJsonValue(json, "nombre"));
        }
        if (hasJsonValue(json, "abreviacion") != null) {
            ubicacion.setAbreviacion((String) hasJsonValue(json, "abreviacion"));
        }

        return ubicacion;
    }

    public Usuario jsonToUsuario(String source) {
        Usuario usuario = new Usuario();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idusuario") != null) {
            usuario.setIdusuario((Integer) hasJsonValue(json, "idusuario"));
        }
        if (hasJsonValue(json, "nombre") != null) {
            usuario.setNombre((String) hasJsonValue(json, "nombre"));
        }
        if (hasJsonValue(json, "apellido") != null) {
            usuario.setApellido((String) hasJsonValue(json, "apellido"));
        }
        if (hasJsonValue(json, "usuario") != null) {
            usuario.setUsuario((String) hasJsonValue(json, "usuario"));
        }
        if (hasJsonValue(json, "contrasena") != null) {
            usuario.setContrasena((String) hasJsonValue(json, "contrasena"));
        }
        if (hasJsonValue(json, "tipousuario") != null) {
            usuario.setIdtipousuario(jsonToTipoUsuario((String) hasJsonValue(json, "tipousuario")));
        }
        if (hasJsonValue(json, "cuentafacebook") != null) {
            usuario.setIdcuentafacebook(jsonToCuentaFacebook((String) hasJsonValue(json, "cuentafacebook")));
        }

        return usuario;
    }

    public Usuariotour jsonToUsuarioTour(String source) {
        Usuariotour usuariotour = new Usuariotour();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idusuariotour") != null) {
            usuariotour.setIdusuariotour((Integer) hasJsonValue(json, "idusuariotour"));
        }
        if (hasJsonValue(json, "estadousuariotour") != null) {
            usuariotour.setEstadousuariotour((String) hasJsonValue(json, "estadousuariotour"));
        }
        if (hasJsonValue(json, "fechainicio") != null) {
            usuariotour.setFechainicio(convertToDate((String) hasJsonValue(json, "fechainicio")));
        }
        if (hasJsonValue(json, "fechafin") != null) {
            usuariotour.setFechafin(convertToDate((String) hasJsonValue(json, "fechafin")));
        }
        if (hasJsonValue(json, "idtour") != null) {
            usuariotour.setIdtour(jsonToTour((String) hasJsonValue(json, "idtour")));
        }

        return usuariotour;
    }

    public Videollamada jsonToVideoLlamada(String source) {
        Videollamada videollamada = new Videollamada();
        JSONObject json = new JSONObject(source);

        if (hasJsonValue(json, "idvideollamada") != null) {
            videollamada.setIdvideollamada((Integer) hasJsonValue(json, "idvideollamada"));
        }
        if (hasJsonValue(json, "fechainicio") != null) {
            videollamada.setFechainicio(convertToDate((String) hasJsonValue(json, "fechainicio")));
        }
        if (hasJsonValue(json, "fechafin") != null) {
            videollamada.setFechafin(convertToDate((String) hasJsonValue(json, "fechafin")));
        }
        if (hasJsonValue(json, "plataforma") != null) {
            videollamada.setPlataforma((String) hasJsonValue(json, "plataforma"));
        }
        if (hasJsonValue(json, "longitud") != null) {
            videollamada.setLongitud((Float) hasJsonValue(json, "longitud"));
        }
        if (hasJsonValue(json, "latitud") != null) {
            videollamada.setLatitud((Float) hasJsonValue(json, "latitud"));
        }
        if (hasJsonValue(json, "idusuario") != null) {
            videollamada.setIdusuario(jsonToUsuario((String) hasJsonValue(json, "idusuario")));
        }

        return videollamada;
    }
}
