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
@Table(name = "usuario")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Usuario.findAll", query = "SELECT u FROM Usuario u"),
    @NamedQuery(name = "Usuario.findByIdusuario", query = "SELECT u FROM Usuario u WHERE u.idusuario = :idusuario"),
    @NamedQuery(name = "Usuario.findByNombre", query = "SELECT u FROM Usuario u WHERE u.nombre = :nombre"),
    @NamedQuery(name = "Usuario.findByApellido", query = "SELECT u FROM Usuario u WHERE u.apellido = :apellido"),
    @NamedQuery(name = "Usuario.findByUsuario", query = "SELECT u FROM Usuario u WHERE u.usuario = :usuario"),
    @NamedQuery(name = "Usuario.findByContrasena", query = "SELECT u FROM Usuario u WHERE u.contrasena = :contrasena")})
public class Usuario implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "idusuario")
    private Integer idusuario;
    @Size(max = 2147483647)
    @Column(name = "nombre")
    private String nombre;
    @Size(max = 2147483647)
    @Column(name = "apellido")
    private String apellido;
    @Size(max = 2147483647)
    @Column(name = "usuario")
    private String usuario;
    @Size(max = 2147483647)
    @Column(name = "contrasena")
    private String contrasena;
    @OneToMany(mappedBy = "idusuario")
    private List<Localizacionusuario> localizacionusuarioList;
    @OneToMany(mappedBy = "idusuariofollower")
    private List<Followusuario> followusuarioList;
    @OneToMany(mappedBy = "idusuariofollowed")
    private List<Followusuario> followusuarioList1;
    @OneToMany(mappedBy = "idusuario")
    private List<Usuariotour> usuariotourList;
    @JoinColumn(name = "idcuentafacebook", referencedColumnName = "idcuentafacebook")
    @ManyToOne
    private Cuentafacebook idcuentafacebook;
    @JoinColumn(name = "idtipousuario", referencedColumnName = "idtipousuario")
    @ManyToOne
    private Tipousuario idtipousuario;
    @OneToMany(mappedBy = "idusuario")
    private List<Tour> tourList;
    @OneToMany(mappedBy = "idusuario")
    private List<Videollamada> videollamadaList;

    public Usuario() {
    }

    public Usuario(Integer idusuario) {
        this.idusuario = idusuario;
    }

    public Integer getIdusuario() {
        return idusuario;
    }

    public void setIdusuario(Integer idusuario) {
        this.idusuario = idusuario;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getApellido() {
        return apellido;
    }

    public void setApellido(String apellido) {
        this.apellido = apellido;
    }

    public String getUsuario() {
        return usuario;
    }

    public void setUsuario(String usuario) {
        this.usuario = usuario;
    }

    public String getContrasena() {
        return contrasena;
    }

    public void setContrasena(String contrasena) {
        this.contrasena = contrasena;
    }

    @XmlTransient
    public List<Localizacionusuario> getLocalizacionusuarioList() {
        return localizacionusuarioList;
    }

    public void setLocalizacionusuarioList(List<Localizacionusuario> localizacionusuarioList) {
        this.localizacionusuarioList = localizacionusuarioList;
    }

    @XmlTransient
    public List<Followusuario> getFollowusuarioList() {
        return followusuarioList;
    }

    public void setFollowusuarioList(List<Followusuario> followusuarioList) {
        this.followusuarioList = followusuarioList;
    }

    @XmlTransient
    public List<Followusuario> getFollowusuarioList1() {
        return followusuarioList1;
    }

    public void setFollowusuarioList1(List<Followusuario> followusuarioList1) {
        this.followusuarioList1 = followusuarioList1;
    }

    @XmlTransient
    public List<Usuariotour> getUsuariotourList() {
        return usuariotourList;
    }

    public void setUsuariotourList(List<Usuariotour> usuariotourList) {
        this.usuariotourList = usuariotourList;
    }

    public Cuentafacebook getIdcuentafacebook() {
        return idcuentafacebook;
    }

    public void setIdcuentafacebook(Cuentafacebook idcuentafacebook) {
        this.idcuentafacebook = idcuentafacebook;
    }

    public Tipousuario getIdtipousuario() {
        return idtipousuario;
    }

    public void setIdtipousuario(Tipousuario idtipousuario) {
        this.idtipousuario = idtipousuario;
    }

    @XmlTransient
    public List<Tour> getTourList() {
        return tourList;
    }

    public void setTourList(List<Tour> tourList) {
        this.tourList = tourList;
    }

    @XmlTransient
    public List<Videollamada> getVideollamadaList() {
        return videollamadaList;
    }

    public void setVideollamadaList(List<Videollamada> videollamadaList) {
        this.videollamadaList = videollamadaList;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (idusuario != null ? idusuario.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Usuario)) {
            return false;
        }
        Usuario other = (Usuario) object;
        if ((this.idusuario == null && other.idusuario != null) || (this.idusuario != null && !this.idusuario.equals(other.idusuario))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "com.sncpucmm.web.domain.Usuario[ idusuario=" + idusuario + " ]";
    }
    
}
