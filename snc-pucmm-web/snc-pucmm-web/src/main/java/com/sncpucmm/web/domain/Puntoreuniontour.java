/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.sncpucmm.web.domain;

import java.io.Serializable;
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
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlTransient;

/**
 *
 * @author Freddy Mesa
 */
@Entity
@Table(name = "puntoreuniontour")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Puntoreuniontour.findAll", query = "SELECT p FROM Puntoreuniontour p"),
    @NamedQuery(name = "Puntoreuniontour.findByIdpuntoreunion", query = "SELECT p FROM Puntoreuniontour p WHERE p.idpuntoreunion = :idpuntoreunion"),
    @NamedQuery(name = "Puntoreuniontour.findBySecuenciapuntoreunion", query = "SELECT p FROM Puntoreuniontour p WHERE p.secuenciapuntoreunion = :secuenciapuntoreunion")})
public class Puntoreuniontour implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idpuntoreunion")
    private Integer idpuntoreunion;
    @Column(name = "secuenciapuntoreunion")
    private Integer secuenciapuntoreunion;
    @OneToMany(mappedBy = "idpuntoreunion")
    private List<Detalleusuariotour> detalleusuariotourList;
    @JoinColumn(name = "idnodo", referencedColumnName = "idnodo")
    @ManyToOne
    private Nodo idnodo;
    @JoinColumn(name = "idtour", referencedColumnName = "idtour")
    @ManyToOne
    private Tour idtour;

    public Puntoreuniontour() {
    }

    public Puntoreuniontour(Integer idpuntoreunion) {
        this.idpuntoreunion = idpuntoreunion;
    }

    public Integer getIdpuntoreunion() {
        return idpuntoreunion;
    }

    public void setIdpuntoreunion(Integer idpuntoreunion) {
        this.idpuntoreunion = idpuntoreunion;
    }

    public Integer getSecuenciapuntoreunion() {
        return secuenciapuntoreunion;
    }

    public void setSecuenciapuntoreunion(Integer secuenciapuntoreunion) {
        this.secuenciapuntoreunion = secuenciapuntoreunion;
    }

    @XmlTransient
    public List<Detalleusuariotour> getDetalleusuariotourList() {
        return detalleusuariotourList;
    }

    public void setDetalleusuariotourList(List<Detalleusuariotour> detalleusuariotourList) {
        this.detalleusuariotourList = detalleusuariotourList;
    }

    public Nodo getIdnodo() {
        return idnodo;
    }

    public void setIdnodo(Nodo idnodo) {
        this.idnodo = idnodo;
    }

    public Tour getIdtour() {
        return idtour;
    }

    public void setIdtour(Tour idtour) {
        this.idtour = idtour;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (idpuntoreunion != null ? idpuntoreunion.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Puntoreuniontour)) {
            return false;
        }
        Puntoreuniontour other = (Puntoreuniontour) object;
        if ((this.idpuntoreunion == null && other.idpuntoreunion != null) || (this.idpuntoreunion != null && !this.idpuntoreunion.equals(other.idpuntoreunion))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Puntoreuniontour[ idpuntoreunion=" + idpuntoreunion + " ]";
    }
    
}
