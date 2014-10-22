/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.sncpucmm.web.domain;

import java.io.Serializable;
import java.util.Date;
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
import javax.persistence.Temporal;
import javax.persistence.TemporalType;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Freddy Mesa
 */
@Entity
@Table(name = "localizacionusuario")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Localizacionusuario.findAll", query = "SELECT l FROM Localizacionusuario l"),
    @NamedQuery(name = "Localizacionusuario.findByIdlocalizacionusuario", query = "SELECT l FROM Localizacionusuario l WHERE l.idlocalizacionusuario = :idlocalizacionusuario"),
    @NamedQuery(name = "Localizacionusuario.findByFechalocalizacion", query = "SELECT l FROM Localizacionusuario l WHERE l.fechalocalizacion = :fechalocalizacion")})
public class Localizacionusuario implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idlocalizacionusuario")
    private Integer idlocalizacionusuario;
    @Column(name = "fechalocalizacion")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechalocalizacion;
    @JoinColumn(name = "idlocalizacion", referencedColumnName = "idlocalizacion")
    @ManyToOne
    private Localizacion idlocalizacion;
    @JoinColumn(name = "idusuario", referencedColumnName = "idusuario")
    @ManyToOne
    private Usuario idusuario;

    public Localizacionusuario() {
    }

    public Localizacionusuario(Integer idlocalizacionusuario) {
        this.idlocalizacionusuario = idlocalizacionusuario;
    }

    public Integer getIdlocalizacionusuario() {
        return idlocalizacionusuario;
    }

    public void setIdlocalizacionusuario(Integer idlocalizacionusuario) {
        this.idlocalizacionusuario = idlocalizacionusuario;
    }

    public Date getFechalocalizacion() {
        return fechalocalizacion;
    }

    public void setFechalocalizacion(Date fechalocalizacion) {
        this.fechalocalizacion = fechalocalizacion;
    }

    public Localizacion getIdlocalizacion() {
        return idlocalizacion;
    }

    public void setIdlocalizacion(Localizacion idlocalizacion) {
        this.idlocalizacion = idlocalizacion;
    }

    public Usuario getIdusuario() {
        return idusuario;
    }

    public void setIdusuario(Usuario idusuario) {
        this.idusuario = idusuario;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (idlocalizacionusuario != null ? idlocalizacionusuario.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Localizacionusuario)) {
            return false;
        }
        Localizacionusuario other = (Localizacionusuario) object;
        if ((this.idlocalizacionusuario == null && other.idlocalizacionusuario != null) || (this.idlocalizacionusuario != null && !this.idlocalizacionusuario.equals(other.idlocalizacionusuario))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Localizacionusuario[ idlocalizacionusuario=" + idlocalizacionusuario + " ]";
    }
    
}
