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
import javax.validation.constraints.Size;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Freddy Mesa
 */
@Entity
@Table(name = "neighbor")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Neighbor.findAll", query = "SELECT n FROM Neighbor n"),
    @NamedQuery(name = "Neighbor.findByIdneighbor", query = "SELECT n FROM Neighbor n WHERE n.idneighbor = :idneighbor"),
    @NamedQuery(name = "Neighbor.findByNodoname", query = "SELECT n FROM Neighbor n WHERE n.nodoname = :nodoname"),
    @NamedQuery(name = "Neighbor.findByNodoneighborname", query = "SELECT n FROM Neighbor n WHERE n.nodoneighborname = :nodoneighborname")})
public class Neighbor implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idneighbor")
    private Integer idneighbor;
    @Size(max = 2147483647)
    @Column(name = "nodoname")
    private String nodoname;
    @Size(max = 2147483647)
    @Column(name = "nodoneighborname")
    private String nodoneighborname;
    @JoinColumn(name = "idnodo", referencedColumnName = "idnodo")
    @ManyToOne
    private Nodo idnodo;
    @JoinColumn(name = "idnodoneighbor", referencedColumnName = "idnodo")
    @ManyToOne
    private Nodo idnodoneighbor;

    public Neighbor() {
    }

    public Neighbor(Integer idneighbor) {
        this.idneighbor = idneighbor;
    }

    public Integer getIdneighbor() {
        return idneighbor;
    }

    public void setIdneighbor(Integer idneighbor) {
        this.idneighbor = idneighbor;
    }

    public String getNodoname() {
        return nodoname;
    }

    public void setNodoname(String nodoname) {
        this.nodoname = nodoname;
    }

    public String getNodoneighborname() {
        return nodoneighborname;
    }

    public void setNodoneighborname(String nodoneighborname) {
        this.nodoneighborname = nodoneighborname;
    }

    public Nodo getIdnodo() {
        return idnodo;
    }

    public void setIdnodo(Nodo idnodo) {
        this.idnodo = idnodo;
    }

    public Nodo getIdnodoneighbor() {
        return idnodoneighbor;
    }

    public void setIdnodoneighbor(Nodo idnodoneighbor) {
        this.idnodoneighbor = idnodoneighbor;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (idneighbor != null ? idneighbor.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Neighbor)) {
            return false;
        }
        Neighbor other = (Neighbor) object;
        if ((this.idneighbor == null && other.idneighbor != null) || (this.idneighbor != null && !this.idneighbor.equals(other.idneighbor))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Neighbor[ idneighbor=" + idneighbor + " ]";
    }
    
}
