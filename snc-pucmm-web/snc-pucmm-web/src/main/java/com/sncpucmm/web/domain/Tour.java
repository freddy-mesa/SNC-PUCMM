/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.sncpucmm.web.domain;

import java.io.Serializable;
import java.util.Date;
import java.util.List;
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
import javax.persistence.OneToMany;
import javax.persistence.Table;
import javax.persistence.Temporal;
import javax.persistence.TemporalType;
import javax.validation.constraints.Size;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlTransient;

/**
 *
 * @author Freddy Mesa
 */
@Entity
@Table(name = "tour")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Tour.findAll", query = "SELECT t FROM Tour t"),
    @NamedQuery(name = "Tour.findByIdtour", query = "SELECT t FROM Tour t WHERE t.idtour = :idtour"),
    @NamedQuery(name = "Tour.findByNombretour", query = "SELECT t FROM Tour t WHERE t.nombretour = :nombretour"),
    @NamedQuery(name = "Tour.findByFechacreacion", query = "SELECT t FROM Tour t WHERE t.fechacreacion = :fechacreacion"),
    @NamedQuery(name = "Tour.findByFechainicio", query = "SELECT t FROM Tour t WHERE t.fechainicio = :fechainicio"),
    @NamedQuery(name = "Tour.findByFechafin", query = "SELECT t FROM Tour t WHERE t.fechafin = :fechafin")})
public class Tour implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idtour")
    private Integer idtour;
    @Size(max = 2147483647)
    @Column(name = "nombretour")
    private String nombretour;
    @Column(name = "fechacreacion")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechacreacion;
    @Column(name = "fechainicio")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechainicio;
    @Column(name = "fechafin")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechafin;
    @OneToMany(mappedBy = "idtour")
    private List<Puntoreuniontour> puntoreuniontourList;
    @OneToMany(mappedBy = "idtour")
    private List<Usuariotour> usuariotourList;
    @JoinColumn(name = "idusuario", referencedColumnName = "idusuario")
    @ManyToOne
    private Usuario idusuario;

    public Tour() {
    }

    public Tour(Integer idtour) {
        this.idtour = idtour;
    }

    public Integer getIdtour() {
        return idtour;
    }

    public void setIdtour(Integer idtour) {
        this.idtour = idtour;
    }

    public String getNombretour() {
        return nombretour;
    }

    public void setNombretour(String nombretour) {
        this.nombretour = nombretour;
    }

    public Date getFechacreacion() {
        return fechacreacion;
    }

    public void setFechacreacion(Date fechacreacion) {
        this.fechacreacion = fechacreacion;
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

    @XmlTransient
    public List<Puntoreuniontour> getPuntoreuniontourList() {
        return puntoreuniontourList;
    }

    public void setPuntoreuniontourList(List<Puntoreuniontour> puntoreuniontourList) {
        this.puntoreuniontourList = puntoreuniontourList;
    }

    @XmlTransient
    public List<Usuariotour> getUsuariotourList() {
        return usuariotourList;
    }

    public void setUsuariotourList(List<Usuariotour> usuariotourList) {
        this.usuariotourList = usuariotourList;
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
        hash += (idtour != null ? idtour.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Tour)) {
            return false;
        }
        Tour other = (Tour) object;
        if ((this.idtour == null && other.idtour != null) || (this.idtour != null && !this.idtour.equals(other.idtour))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Tour[ idtour=" + idtour + " ]";
    }
    
}
