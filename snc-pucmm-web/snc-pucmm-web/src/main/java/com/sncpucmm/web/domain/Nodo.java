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
@Table(name = "nodo")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Nodo.findAll", query = "SELECT n FROM Nodo n"),
    @NamedQuery(name = "Nodo.findByIdnodo", query = "SELECT n FROM Nodo n WHERE n.idnodo = :idnodo"),
    @NamedQuery(name = "Nodo.findByEdificio", query = "SELECT n FROM Nodo n WHERE n.edificio = :edificio"),
    @NamedQuery(name = "Nodo.findByNombre", query = "SELECT n FROM Nodo n WHERE n.nombre = :nombre"),
    @NamedQuery(name = "Nodo.findByActivo", query = "SELECT n FROM Nodo n WHERE n.activo = :activo")})
public class Nodo implements Serializable {
    @OneToMany(mappedBy = "idnodo")
    private List<Neighbor> neighborList;
    @OneToMany(mappedBy = "idnodoneighbor")
    private List<Neighbor> neighborList1;
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idnodo")
    private Integer idnodo;
    @Column(name = "edificio")
    private Integer edificio;
    @Size(max = 2147483647)
    @Column(name = "nombre")
    private String nombre;
    @Column(name = "activo")
    private Integer activo;
    @OneToMany(mappedBy = "idnodo")
    private List<Localizacionusuario> localizacionusuarioList;
    @OneToMany(mappedBy = "idnodo")
    private List<Puntoreuniontour> puntoreuniontourList;
    @OneToMany(mappedBy = "idnodo")
    private List<Coordenadanodo> coordenadanodoList;
    @JoinColumn(name = "idubicacion", referencedColumnName = "idubicacion")
    @ManyToOne
    private Ubicacion idubicacion;

    public Nodo() {
    }

    public Nodo(Integer idnodo) {
        this.idnodo = idnodo;
    }

    public Integer getIdnodo() {
        return idnodo;
    }

    public void setIdnodo(Integer idnodo) {
        this.idnodo = idnodo;
    }

    public Integer getEdificio() {
        return edificio;
    }

    public void setEdificio(Integer edificio) {
        this.edificio = edificio;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public Integer getActivo() {
        return activo;
    }

    public void setActivo(Integer activo) {
        this.activo = activo;
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

    @XmlTransient
    public List<Coordenadanodo> getCoordenadanodoList() {
        return coordenadanodoList;
    }

    public void setCoordenadanodoList(List<Coordenadanodo> coordenadanodoList) {
        this.coordenadanodoList = coordenadanodoList;
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
        hash += (idnodo != null ? idnodo.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Nodo)) {
            return false;
        }
        Nodo other = (Nodo) object;
        if ((this.idnodo == null && other.idnodo != null) || (this.idnodo != null && !this.idnodo.equals(other.idnodo))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Nodo[ idnodo=" + idnodo + " ]";
    }

    @XmlTransient
    public List<Neighbor> getNeighborList() {
        return neighborList;
    }

    public void setNeighborList(List<Neighbor> neighborList) {
        this.neighborList = neighborList;
    }

    @XmlTransient
    public List<Neighbor> getNeighborList1() {
        return neighborList1;
    }

    public void setNeighborList1(List<Neighbor> neighborList1) {
        this.neighborList1 = neighborList1;
    }
    
}
