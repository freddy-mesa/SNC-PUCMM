/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.sncpucmm.web.domain;

import java.io.Serializable;
import javax.persistence.Basic;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.NamedQueries;
import javax.persistence.NamedQuery;
import javax.persistence.Table;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Freddy Mesa
 */
@Entity
@Table(name = "coordenadanodo")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Coordenadanodo.findAll", query = "SELECT c FROM Coordenadanodo c"),
    @NamedQuery(name = "Coordenadanodo.findByIdcoordenadanodo", query = "SELECT c FROM Coordenadanodo c WHERE c.idcoordenadanodo = :idcoordenadanodo"),
    @NamedQuery(name = "Coordenadanodo.findByLongitud", query = "SELECT c FROM Coordenadanodo c WHERE c.longitud = :longitud"),
    @NamedQuery(name = "Coordenadanodo.findByLatitud", query = "SELECT c FROM Coordenadanodo c WHERE c.latitud = :latitud")})
public class Coordenadanodo implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idcoordenadanodo")
    private Integer idcoordenadanodo;
    // @Max(value=?)  @Min(value=?)//if you know range of your decimal fields consider using these annotations to enforce field validation
    @Column(name = "longitud")
    private Float longitud;
    @Column(name = "latitud")
    private Float latitud;
    @JoinColumn(name = "idnodo", referencedColumnName = "idnodo")
    @ManyToOne
    private Nodo idnodo;

    public Coordenadanodo() {
    }

    public Coordenadanodo(Integer idcoordenadanodo) {
        this.idcoordenadanodo = idcoordenadanodo;
    }

    public Integer getIdcoordenadanodo() {
        return idcoordenadanodo;
    }

    public void setIdcoordenadanodo(Integer idcoordenadanodo) {
        this.idcoordenadanodo = idcoordenadanodo;
    }

    public Float getLongitud() {
        return longitud;
    }

    public void setLongitud(Float longitud) {
        this.longitud = longitud;
    }

    public Float getLatitud() {
        return latitud;
    }

    public void setLatitud(Float latitud) {
        this.latitud = latitud;
    }

    public Nodo getIdnodo() {
        return idnodo;
    }

    public void setIdnodo(Nodo idnodo) {
        this.idnodo = idnodo;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (idcoordenadanodo != null ? idcoordenadanodo.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Coordenadanodo)) {
            return false;
        }
        Coordenadanodo other = (Coordenadanodo) object;
        if ((this.idcoordenadanodo == null && other.idcoordenadanodo != null) || (this.idcoordenadanodo != null && !this.idcoordenadanodo.equals(other.idcoordenadanodo))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Coordenadanodo[ idcoordenadanodo=" + idcoordenadanodo + " ]";
    }
    
}
