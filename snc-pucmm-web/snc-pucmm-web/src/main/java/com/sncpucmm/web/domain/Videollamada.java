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
@Table(name = "videollamada")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Videollamada.findAll", query = "SELECT v FROM Videollamada v"),
    @NamedQuery(name = "Videollamada.findByIdvideollamada", query = "SELECT v FROM Videollamada v WHERE v.idvideollamada = :idvideollamada"),
    @NamedQuery(name = "Videollamada.findByFechainicio", query = "SELECT v FROM Videollamada v WHERE v.fechainicio = :fechainicio"),
    @NamedQuery(name = "Videollamada.findByFechafin", query = "SELECT v FROM Videollamada v WHERE v.fechafin = :fechafin"),
    @NamedQuery(name = "Videollamada.findByPlataforma", query = "SELECT v FROM Videollamada v WHERE v.plataforma = :plataforma"),
    @NamedQuery(name = "Videollamada.findByLongitud", query = "SELECT v FROM Videollamada v WHERE v.longitud = :longitud"),
    @NamedQuery(name = "Videollamada.findByLatitud", query = "SELECT v FROM Videollamada v WHERE v.latitud = :latitud")})
public class Videollamada implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idvideollamada")
    private Integer idvideollamada;
    @Column(name = "fechainicio")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechainicio;
    @Column(name = "fechafin")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechafin;
    @Size(max = 2147483647)
    @Column(name = "plataforma")
    private String plataforma;
    // @Max(value=?)  @Min(value=?)//if you know range of your decimal fields consider using these annotations to enforce field validation
    @Column(name = "longitud")
    private Float longitud;
    @Column(name = "latitud")
    private Float latitud;
    @JoinColumn(name = "idusuario", referencedColumnName = "idusuario")
    @ManyToOne
    private Usuario idusuario;

    public Videollamada() {
    }

    public Videollamada(Integer idvideollamada) {
        this.idvideollamada = idvideollamada;
    }

    public Integer getIdvideollamada() {
        return idvideollamada;
    }

    public void setIdvideollamada(Integer idvideollamada) {
        this.idvideollamada = idvideollamada;
    }

    public Date getFechainicio() {
        return fechainicio;
    }

    public void setFechainicio(Date fechainicio) {
        this.fechainicio = fechainicio;
    }

    public Date getFechafin() {
        return fechafin;
    }

    public void setFechafin(Date fechafin) {
        this.fechafin = fechafin;
    }

    public String getPlataforma() {
        return plataforma;
    }

    public void setPlataforma(String plataforma) {
        this.plataforma = plataforma;
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

    public Usuario getIdusuario() {
        return idusuario;
    }

    public void setIdusuario(Usuario idusuario) {
        this.idusuario = idusuario;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (idvideollamada != null ? idvideollamada.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Videollamada)) {
            return false;
        }
        Videollamada other = (Videollamada) object;
        if ((this.idvideollamada == null && other.idvideollamada != null) || (this.idvideollamada != null && !this.idvideollamada.equals(other.idvideollamada))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Videollamada[ idvideollamada=" + idvideollamada + " ]";
    }
    
}
