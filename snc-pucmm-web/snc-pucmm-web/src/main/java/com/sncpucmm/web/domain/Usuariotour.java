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
@Table(name = "usuariotour")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Usuariotour.findAll", query = "SELECT u FROM Usuariotour u"),
    @NamedQuery(name = "Usuariotour.findByIdusuariotour", query = "SELECT u FROM Usuariotour u WHERE u.idusuariotour = :idusuariotour"),
    @NamedQuery(name = "Usuariotour.findByEstadousuariotour", query = "SELECT u FROM Usuariotour u WHERE u.estadousuariotour = :estadousuariotour"),
    @NamedQuery(name = "Usuariotour.findByFechainicio", query = "SELECT u FROM Usuariotour u WHERE u.fechainicio = :fechainicio"),
    @NamedQuery(name = "Usuariotour.findByFechafin", query = "SELECT u FROM Usuariotour u WHERE u.fechafin = :fechafin")})
public class Usuariotour implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idusuariotour")
    private Integer idusuariotour;
    @Size(max = 2147483647)
    @Column(name = "estadousuariotour")
    private String estadousuariotour;
    @Column(name = "fechainicio")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechainicio;
    @Column(name = "fechafin")
    @Temporal(TemporalType.TIMESTAMP)
    private Date fechafin;
    @OneToMany(mappedBy = "idusuariotour")
    private List<Detalleusuariotour> detalleusuariotourList;
    @JoinColumn(name = "idtour", referencedColumnName = "idtour")
    @ManyToOne
    private Tour idtour;
    @JoinColumn(name = "idusuario", referencedColumnName = "idusuario")
    @ManyToOne
    private Usuario idusuario;

    public Usuariotour() {
    }

    public Usuariotour(Integer idusuariotour) {
        this.idusuariotour = idusuariotour;
    }

    public Integer getIdusuariotour() {
        return idusuariotour;
    }

    public void setIdusuariotour(Integer idusuariotour) {
        this.idusuariotour = idusuariotour;
    }

    public String getEstadousuariotour() {
        return estadousuariotour;
    }

    public void setEstadousuariotour(String estadousuariotour) {
        this.estadousuariotour = estadousuariotour;
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
    public List<Detalleusuariotour> getDetalleusuariotourList() {
        return detalleusuariotourList;
    }

    public void setDetalleusuariotourList(List<Detalleusuariotour> detalleusuariotourList) {
        this.detalleusuariotourList = detalleusuariotourList;
    }

    public Tour getIdtour() {
        return idtour;
    }

    public void setIdtour(Tour idtour) {
        this.idtour = idtour;
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
        hash += (idusuariotour != null ? idusuariotour.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Usuariotour)) {
            return false;
        }
        Usuariotour other = (Usuariotour) object;
        if ((this.idusuariotour == null && other.idusuariotour != null) || (this.idusuariotour != null && !this.idusuariotour.equals(other.idusuariotour))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Usuariotour[ idusuariotour=" + idusuariotour + " ]";
    }
    
}
