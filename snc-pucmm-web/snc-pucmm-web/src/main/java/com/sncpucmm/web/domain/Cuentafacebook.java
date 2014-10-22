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
@Table(name = "cuentafacebook")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Cuentafacebook.findAll", query = "SELECT c FROM Cuentafacebook c"),
    @NamedQuery(name = "Cuentafacebook.findByIdcuentafacebook", query = "SELECT c FROM Cuentafacebook c WHERE c.idcuentafacebook = :idcuentafacebook"),
    @NamedQuery(name = "Cuentafacebook.findByUsuariofacebook", query = "SELECT c FROM Cuentafacebook c WHERE c.usuariofacebook = :usuariofacebook"),
    @NamedQuery(name = "Cuentafacebook.findByToken", query = "SELECT c FROM Cuentafacebook c WHERE c.token = :token")})
public class Cuentafacebook implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idcuentafacebook")
    private Integer idcuentafacebook;
    @Size(max = 2147483647)
    @Column(name = "usuariofacebook")
    private String usuariofacebook;
    @Size(max = 2147483647)
    @Column(name = "token")
    private String token;
    @OneToMany(mappedBy = "idcuentafacebook")
    private List<Usuario> usuarioList;

    public Cuentafacebook() {
    }

    public Cuentafacebook(Integer idcuentafacebook) {
        this.idcuentafacebook = idcuentafacebook;
    }

    public Integer getIdcuentafacebook() {
        return idcuentafacebook;
    }

    public void setIdcuentafacebook(Integer idcuentafacebook) {
        this.idcuentafacebook = idcuentafacebook;
    }

    public String getUsuariofacebook() {
        return usuariofacebook;
    }

    public void setUsuariofacebook(String usuariofacebook) {
        this.usuariofacebook = usuariofacebook;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }

    @XmlTransient
    public List<Usuario> getUsuarioList() {
        return usuarioList;
    }

    public void setUsuarioList(List<Usuario> usuarioList) {
        this.usuarioList = usuarioList;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (idcuentafacebook != null ? idcuentafacebook.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Cuentafacebook)) {
            return false;
        }
        Cuentafacebook other = (Cuentafacebook) object;
        if ((this.idcuentafacebook == null && other.idcuentafacebook != null) || (this.idcuentafacebook != null && !this.idcuentafacebook.equals(other.idcuentafacebook))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Cuentafacebook[ idcuentafacebook=" + idcuentafacebook + " ]";
    }
    
}
