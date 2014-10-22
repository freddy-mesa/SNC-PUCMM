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
import javax.validation.constraints.Size;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Freddy Mesa
 */
@Entity
@Table(name = "followusuario")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Followusuario.findAll", query = "SELECT f FROM Followusuario f"),
    @NamedQuery(name = "Followusuario.findByIdfollow", query = "SELECT f FROM Followusuario f WHERE f.idfollow = :idfollow"),
    @NamedQuery(name = "Followusuario.findByEstadosolicitud", query = "SELECT f FROM Followusuario f WHERE f.estadosolicitud = :estadosolicitud"),
    @NamedQuery(name = "Followusuario.findByFecharegistrosolicitud", query = "SELECT f FROM Followusuario f WHERE f.fecharegistrosolicitud = :fecharegistrosolicitud"),
    @NamedQuery(name = "Followusuario.findByFecharespuestasolicitud", query = "SELECT f FROM Followusuario f WHERE f.fecharespuestasolicitud = :fecharespuestasolicitud")})
public class Followusuario implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idfollow")
    private Integer idfollow;
    @Size(max = 2147483647)
    @Column(name = "estadosolicitud")
    private String estadosolicitud;
    @Column(name = "fecharegistrosolicitud")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fecharegistrosolicitud;
    @Column(name = "fecharespuestasolicitud")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fecharespuestasolicitud;
    @JoinColumn(name = "idusuariofollower", referencedColumnName = "idusuario")
    @ManyToOne
    private Usuario idusuariofollower;
    @JoinColumn(name = "idusuariofollowed", referencedColumnName = "idusuario")
    @ManyToOne
    private Usuario idusuariofollowed;

    public Followusuario() {
    }

    public Followusuario(Integer idfollow) {
        this.idfollow = idfollow;
    }

    public Integer getIdfollow() {
        return idfollow;
    }

    public void setIdfollow(Integer idfollow) {
        this.idfollow = idfollow;
    }

    public String getEstadosolicitud() {
        return estadosolicitud;
    }

    public void setEstadosolicitud(String estadosolicitud) {
        this.estadosolicitud = estadosolicitud;
    }

    public Date getFecharegistrosolicitud() {
        return fecharegistrosolicitud;
    }

    public void setFecharegistrosolicitud(Date fecharegistrosolicitud) {
        this.fecharegistrosolicitud = fecharegistrosolicitud;
    }

    public Date getFecharespuestasolicitud() {
        return fecharespuestasolicitud;
    }

    public void setFecharespuestasolicitud(Date fecharespuestasolicitud) {
        this.fecharespuestasolicitud = fecharespuestasolicitud;
    }

    public Usuario getIdusuariofollower() {
        return idusuariofollower;
    }

    public void setIdusuariofollower(Usuario idusuariofollower) {
        this.idusuariofollower = idusuariofollower;
    }

    public Usuario getIdusuariofollowed() {
        return idusuariofollowed;
    }

    public void setIdusuariofollowed(Usuario idusuariofollowed) {
        this.idusuariofollowed = idusuariofollowed;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (idfollow != null ? idfollow.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Followusuario)) {
            return false;
        }
        Followusuario other = (Followusuario) object;
        if ((this.idfollow == null && other.idfollow != null) || (this.idfollow != null && !this.idfollow.equals(other.idfollow))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Followusuario[ idfollow=" + idfollow + " ]";
    }
    
}
