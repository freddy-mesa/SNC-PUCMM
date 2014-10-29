/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.sncpucmm.web.ejb;

import com.sncpucmm.web.domain.Coordenadanodo;
import com.sncpucmm.web.domain.Neighbor;
import com.sncpucmm.web.domain.Nodo;
import com.sncpucmm.web.domain.Ubicacion;
import java.util.List;
import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import org.json.JSONArray;
import org.json.JSONObject;

/**
 *
 * @author Freddy Mesa
 */
@Startup
@Singleton
public class ApplicationManager {

    @EJB
    DataBaseService service;

    @PostConstruct
    public void init() {

        Ubicacion ubicacion = new Ubicacion();
        ubicacion.setNombre("Academia Real Madrid");
        ubicacion.setAbreviacion("ACADREAL");
        service.persist(ubicacion);

        Nodo nodo1 = new Nodo();
        nodo1.setActivo(1);
        nodo1.setEdificio(30);
        nodo1.setNombre("Nodo 80");
        nodo1.setIdubicacion(ubicacion);
        service.persist(nodo1);

        Nodo nodo2 = new Nodo();
        nodo1.setActivo(1);
        nodo1.setNombre("Nodo 81");
        service.persist(nodo2);

        Coordenadanodo coordenadaNodo1 = new Coordenadanodo();
        coordenadaNodo1.setIdnodo(nodo1);
        coordenadaNodo1.setLatitud(19.192334f);
        coordenadaNodo1.setLongitud(-70.243231f);
        service.persist(coordenadaNodo1);

        Coordenadanodo coordenadaNodo2 = new Coordenadanodo();
        coordenadaNodo1.setIdnodo(nodo2);
        coordenadaNodo1.setLatitud(19.192321f);
        coordenadaNodo1.setLongitud(-70.243231f);
        service.persist(coordenadaNodo2);

        Neighbor neighbor = new Neighbor();
        neighbor.setIdnodo(nodo1);
        neighbor.setIdnodoneighbor(nodo2);
        service.persist(neighbor);
    }

    public String databaseClientUpdateToJson() {
        JSONObject json = new JSONObject();
        
        List<Ubicacion> ubicacion = service.findAll(Ubicacion.class, "Ubicacion.findAll");        
        List<Nodo> nodos = service.findAll(Nodo.class, "Nodo.findAll");
        List<Coordenadanodo> coordenadanodos = service.findAll(Coordenadanodo.class, "Coordenadanodo.findAll");
        List<Neighbor> neighbors = service.findAll(Neighbor.class, "Neighbor.findAll");
        
        json.put("Ubicacion", new JSONArray(ubicacion));
        json.put("Nodo", new JSONArray(nodos));
        json.put("Coordenadanodo", new JSONArray(coordenadanodos));
        json.put("Neighbor", new JSONArray(neighbors));
        
        return json.toString();
    }
}
