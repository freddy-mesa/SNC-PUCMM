/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.sncpucmm.web.ejb;

import com.sncpucmm.web.domain.Tour;
import com.sncpucmm.web.domain.Usuario;
import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.ejb.EJB;
import javax.ejb.Stateless;
import javax.transaction.HeuristicMixedException;
import javax.transaction.HeuristicRollbackException;
import javax.transaction.NotSupportedException;
import javax.transaction.RollbackException;
import javax.transaction.SystemException;

/**
 *
 * @author Freddy Mesa
 */
@Stateless
public class SncPucmmService {
    
    @EJB DataBaseService database;
    
    public Usuario findUsuario(int idUsuario){
        return database.getEntityById(Usuario.class, idUsuario);
    }
    
    public Usuario createUsuario(Usuario user){
        database.persist(user);
        return findUsuario(user.getIdusuario());
    }
    
    public Usuario updateUsuario(Usuario user){
        try {
            database.utx.begin();
            Usuario userDB = findUsuario(user.getIdusuario());
            
            userDB.setNombre(user.getNombre());
            userDB.setApellido(user.getApellido());
            userDB.setContrasena(user.getContrasena());
            userDB.setIdcuentafacebook(user.getIdcuentafacebook());
            userDB.setIdtipousuario(user.getIdtipousuario());
            
            database.utx.commit(); 
            
        } catch (NotSupportedException | SystemException | RollbackException | HeuristicMixedException | HeuristicRollbackException | SecurityException | IllegalStateException ex) {
            Logger.getLogger(SncPucmmService.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        Usuario updatedUsuario = findUsuario(user.getIdusuario());
        return updatedUsuario;
    }
    
    public Tour findTour(int idTour){
        return database.getEntityById(Tour.class, idTour);
    }
    
    public Tour createTour(Tour tour) {
        database.persist(tour);
        return findTour(tour.getIdtour());
    }

    public Tour updateTour(Tour tour){
        try {
            database.utx.begin();
            Tour tourDB = findTour(tour.getIdtour());
            
            database.utx.commit(); 
            
        } catch (NotSupportedException | SystemException | RollbackException | HeuristicMixedException | HeuristicRollbackException | SecurityException | IllegalStateException ex) {
            Logger.getLogger(SncPucmmService.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        Tour updatedTour = findTour(tour.getIdtour());
        return updatedTour;
    }
}
