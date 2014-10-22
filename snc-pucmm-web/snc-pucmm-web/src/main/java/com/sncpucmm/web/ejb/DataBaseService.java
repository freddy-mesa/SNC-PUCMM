/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.sncpucmm.web.ejb;
 
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.annotation.Resource;
import javax.ejb.Lock;
import javax.ejb.LockType;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.ejb.TransactionManagement;
import javax.ejb.TransactionManagementType;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.transaction.HeuristicMixedException;
import javax.transaction.HeuristicRollbackException;
import javax.transaction.NotSupportedException;
import javax.transaction.RollbackException;
import javax.transaction.SystemException;
import javax.transaction.UserTransaction;
 
/**
 *
 * @author fmesa
 */
@Startup
@Singleton
@TransactionManagement(TransactionManagementType.BEAN)
public class DataBaseService {
 
    @PersistenceContext(unitName = "snc-pucmm-web_PU")
    private EntityManager entityManager;
 
    @Resource
    public UserTransaction utx;
 
    /**
     * Save an entity into database
     *
     * @param object
     */
    @Lock(LockType.WRITE)
    public void persist(Object object) {
        try {
            utx.begin();
            entityManager.persist(object);
            utx.commit();
        } catch (NotSupportedException | SystemException | RollbackException | HeuristicMixedException | HeuristicRollbackException | SecurityException | IllegalStateException e) {
            Logger.getLogger(getClass().getName()).log(Level.SEVERE, "exception caught", e);
            throw new RuntimeException(e);
        }
    }
 
    /**
     * Get a entity from a target Class
     *
     * @param <T> Target Class
     * @param className Target Class
     * @param value Entity's Id
     * @return entity
     */
    public <T> T getEntityById(Class<T> className, Integer value) {
        T entity = null;
        try {
            entity = entityManager.find(className, value);
        } catch (Exception e) {
            Logger.getLogger(getClass().getName()).log(Level.SEVERE, "exception caught", e);
        }
        return entity;
    }
 
    /**
     * Find a list of Entities given a target class
     *
     * @param <T> Target Class
     * @param className Target Class
     * @param queryName QueryNamed declare in Entity Class
     * @param column Column name
     * @param value Value for where clause
     * @return List of Target Class (List of Entities)
     */
    public <T> List<T> findEntityList(Class<T> className, String queryName, String column, Object value) {
        List<T> resultList = null;
        try {
            resultList = entityManager.createNamedQuery(queryName, className).setParameter(column, value).getResultList();
        } catch (Exception e) {
            Logger.getLogger(getClass().getName()).log(Level.SEVERE, "exception caught", e);
        }
 
        if (resultList == null || resultList.isEmpty()) {
            return null;
        }
 
        return resultList;
    }
 
    /**
     * Find a single Entity
     *
     * @param <T> Target Class
     * @param className Target Class
     * @param queryName QueryNamed declare in Entity Class
     * @param column Column name
     * @param value Value for where clause
     * @return Single Entity of Target Class
     */
    public <T> T findEntity(Class<T> className, String queryName, String column, Object value) {
        List<T> findEntityList = findEntityList(className, queryName, column, value);
 
        if (findEntityList == null) {
            return null;
        }
 
        return findEntityList.get(0);
    }
 
    /**
     * Find all entity from a target class
     *
     * @param <T> Target Class Type
     * @param className Target Class
     * @param queryNamed QueryNamed declared in Entity Target Class
     * @return Target Class's List of Entity
     */
    public <T> List<T> findAll(Class<T> className, String queryNamed) {
        List<T> entityList = null;
 
        try {
            entityList = entityManager.createNamedQuery(queryNamed, className).getResultList();
        } catch (Exception e) {
            Logger.getLogger(getClass().getName()).log(Level.SEVERE, "exception caught", e);
        }
 
        return entityList;
    }
}