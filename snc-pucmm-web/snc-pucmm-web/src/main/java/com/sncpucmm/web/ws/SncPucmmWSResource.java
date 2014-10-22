/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.sncpucmm.web.ws;

import com.sncpucmm.web.domain.Tour;
import com.sncpucmm.web.domain.Usuario;
import com.sncpucmm.web.ejb.JsonWrapperService;
import com.sncpucmm.web.ejb.SncPucmmService;
import javax.ejb.EJB;
import javax.ws.rs.core.Context;
import javax.ws.rs.core.UriInfo;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.Consumes;
import javax.ws.rs.FormParam;
import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.QueryParam;

import org.json.JSONObject;

/**
 * REST Web Service
 *
 * @author Freddy Mesa
 */
@Path("SncPucmmWS")
public class SncPucmmWSResource {

    @Context
    private UriInfo context;
    
    @EJB SncPucmmService service;
    @EJB JsonWrapperService jsonService;

    /**
     * Creates a new instance of SncPucmmWSResource
     */
    public SncPucmmWSResource() {
    }

    @GET
    @Produces("application/json")
    public String getJson() {
        JSONObject json = new JSONObject();
        json.put("working", true);
        return json.toString();
    }
    
    @Path("/usuario/create/")
    @POST
    @Produces("text/plain")
    public String createUsuario(@FormParam(value = "json") String json){
        Usuario user = service.createUsuario(jsonService.jsonToUsuario(json));
        String jsonToResponse = jsonService.usuarioToJson(user);
        return jsonToResponse;
    }
    
    @Path("/usuario/select/{id}")
    @GET
    @Produces("text/plain")
    public String selectUsuario(@PathParam(value = "id") Integer idUsuario){
        Usuario user = service.findUsuario(idUsuario);
        String jsonToResponse = jsonService.usuarioToJson(user);
        return jsonToResponse;
    }
    
    @Path("/tour/create/")
    @POST
    @Produces("text/plain")
    public String createTour(@FormParam(value = "json") String json){
        Tour tour = jsonService.jsonToTour(json);
        Tour tourCreated = service.createTour(tour);
        String jsonResponse = jsonService.tourToJson(tourCreated);
        return jsonResponse;
    }
    
    @Path("/tour/select/{id}")
    @GET
    @Produces("text/plain")
    public String selectTour(@PathParam(value = "id") Integer idTour){
        Tour tour = service.findTour(idTour);
        String jsonToResponse = jsonService.tourToJson(tour);
        return jsonToResponse;
    }
    
    @Path("/localizacion/list/")
    @GET
    @Produces("text/plain")
    public String listLocalizacion(){
        return "";
    }
}
