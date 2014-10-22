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
@Table(name = "detalleusuariotour")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Detalleusuariotour.findAll", query = "SELECT d FROM Detalleusuariotour d"),
    @NamedQuery(name = "Detalleusuariotour.findByIddetalleusuariotour", query = "SELECT d FROM Detalleusuariotour d WHERE d.iddetalleusuariotour = :iddetalleusuariotour"),
    @NamedQuery(name = "Detalleusuariotour.findByEstado", query = "SELECT d FROM Detalleusuariotour d WHERE d.estado = :estado"),
    @NamedQuery(name = "Detalleusuariotour.findByFechainicio", query = "SELECT d FROM Detalleusuariotour d WHERE d.fechainicio = :fechainicio"),
    @NamedQuery(name = "Detalleusuariotour.findByFechallegada", query = "SELECT d FROM Detalleusuariotour d WHERE d.fechallegada = :fechallegada")})
public class Detalleusuariotour implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "iddetalleusuariotour")
    private Integer iddetalleusuariotour;
    @Size(max = 2147483647)
    @Column(name = "estado")
    private String estado;
    @Column(name = "fechainicio")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechainicio;
    @Column(name = "fechallegada")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechallegada;
    @JoinColumn(name = "idpuntoreunion", referencedColumnName = "idpuntoreunion")
    @ManyToOne
    private Puntoreuniontour idpuntoreunion;
    @JoinColumn(name = "idusuariotour", referencedColumnName = "idusuariotour")
    @ManyToOne
    private Usuariotour idusuariotour;

    public Detalleusuariotour() {
    }

    public Detalleusuariotour(Integer iddetalleusuariotour) {
        this.iddetalleusuariotour = iddetalleusuariotour;
    }

    public Integer getIddetalleusuariotour() {
        return iddetalleusuariotour;
    }

    public void setIddetalleusuariotour(Integer iddetalleusuariotour) {
        this.iddetalleusuariotour = iddetalleusuariotour;
    }

    public String getEstado() {
        return estado;
    }

    public void setEstado(String estado) {
        this.estado = estado;
    }

    public Date getFechainicio() {
        return fechainicio;
    }

    public void setFechainicio(Date fechainicio) {
        this.fechainicio = fechainicio;
    }

    public Date getFechallegada() {
        return fechallegada;
    }

    public void setFechallegada(Date fechallegada) {
        this.fechallegada = fechallegada;
    }

    public Puntoreuniontour getIdpuntoreunion() {
        return idpuntoreunion;
    }

    public void setIdpuntoreunion(Puntoreuniontour idpuntoreunion) {
        this.idpuntoreunion = idpuntoreunion;
    }

    public Usuariotour getIdusuariotour() {
        return idusuariotour;
    }

    public void setIdusuariotour(Usuariotour idusuariotour) {
        this.idusuariotour = idusuariotour;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (iddetalleusuariotour != null ? iddetalleusuariotour.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Detalleusuariotour)) {
            return false;
        }
        Detalleusuariotour other = (Detalleusuariotour) object;
        if ((this.iddetalleusuariotour == null && other.iddetalleusuariotour != null) || (this.iddetalleusuariotour != null && !this.iddetalleusuariotour.equals(other.iddetalleusuariotour))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Detalleusuariotour[ iddetalleusuariotour=" + iddetalleusuariotour + " ]";
    }
    
}
