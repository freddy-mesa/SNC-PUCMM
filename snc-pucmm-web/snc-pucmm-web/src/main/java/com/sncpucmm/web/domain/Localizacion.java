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
import javax.validation.constraints.Size;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlTransient;

/**
 *
 * @author Freddy Mesa
 */
@Entity
@Table(name = "localizacion")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Localizacion.findAll", query = "SELECT l FROM Localizacion l"),
    @NamedQuery(name = "Localizacion.findByIdlocalizacion", query = "SELECT l FROM Localizacion l WHERE l.idlocalizacion = :idlocalizacion"),
    @NamedQuery(name = "Localizacion.findByNombre", query = "SELECT l FROM Localizacion l WHERE l.nombre = :nombre")})
public class Localizacion implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idlocalizacion")
    private Integer idlocalizacion;
    @Size(max = 2147483647)
    @Column(name = "nombre")
    private String nombre;
    @OneToMany(mappedBy = "idlocalizacion")
    private List<Localizacionusuario> localizacionusuarioList;
    @OneToMany(mappedBy = "idlocalizacion")
    private List<Puntoreuniontour> puntoreuniontourList;
    @JoinColumn(name = "idubicacion", referencedColumnName = "idubicacion")
    @ManyToOne
    private Ubicacion idubicacion;

    public Localizacion() {
    }

    public Localizacion(Integer idlocalizacion) {
        this.idlocalizacion = idlocalizacion;
    }

    public Integer getIdlocalizacion() {
        return idlocalizacion;
    }

    public void setIdlocalizacion(Integer idlocalizacion) {
        this.idlocalizacion = idlocalizacion;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    @XmlTransient
    public List<Localizacionusuario> getLocalizacionusuarioList() {
        return localizacionusuarioList;
    }

    public void setLocalizacionusuarioList(List<Localizacionusuario> localizacionusuarioList) {
        this.localizacionusuarioList = localizacionusuarioList;
    }

    @XmlTransient
    public List<Puntoreuniontour> getPuntoreuniontourList() {
        return puntoreuniontourList;
    }

    public void setPuntoreuniontourList(List<Puntoreuniontour> puntoreuniontourList) {
        this.puntoreuniontourList = puntoreuniontourList;
    }

    public Ubicacion getIdubicacion() {
        return idubicacion;
    }

    public void setIdubicacion(Ubicacion idubicacion) {
        this.idubicacion = idubicacion;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (idlocalizacion != null ? idlocalizacion.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Localizacion)) {
            return false;
        }
        Localizacion other = (Localizacion) object;
        if ((this.idlocalizacion == null && other.idlocalizacion != null) || (this.idlocalizacion != null && !this.idlocalizacion.equals(other.idlocalizacion))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Localizacion[ idlocalizacion=" + idlocalizacion + " ]";
    }
    
}
