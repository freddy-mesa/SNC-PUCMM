<%@ page import="sncpucmm.UsuarioTipoUsuario; sncpucmm.Usuario" %>

<div class="fieldcontain ${hasErrors(bean: usuarioInstance, field: 'name', 'error')} required">
    <label for="name">
        <g:message code="usuario.name.label" default="Name" />
        <span class="required-indicator">*</span>
    </label>
    <g:textField name="name" required="" value="${usuarioInstance?.name}"/>

</div>

<div class="fieldcontain ${hasErrors(bean: usuarioInstance, field: 'lastname', 'error')} required">
    <label for="lastname">
        <g:message code="usuario.lastname.label" default="Last Name" />
        <span class="required-indicator">*</span>
    </label>
    <g:textField name="lastname" required="" value="${usuarioInstance?.lastname}"/>

</div>


<div class="fieldcontain ${hasErrors(bean: usuarioInstance, field: 'username', 'error')} required">
    <label for="username">
        <g:message code="usuario.username.label" default="Username" />
        <span class="required-indicator">*</span>
    </label>
    <g:textField name="username" required="" value="${usuarioInstance?.username}"/>

</div>

<div class="fieldcontain ${hasErrors(bean: usuarioInstance, field: 'password', 'error')} required">
    <label for="password">
        <g:message code="usuario.password.label" default="Password" />
        <span class="required-indicator">*</span>
    </label>
    <g:textField name="password" required="" value="${usuarioInstance?.password}"/>

</div>

<div class="fieldcontain ${hasErrors(bean: usuarioInstance, field: 'cuentaFacebook', 'error')} ">
    <label for="cuentaFacebook">
        <g:message code="usuario.cuentaFacebook.label" default="Tipo de Usuario" />

    </label>
    <g:select id="tipousuario" name="tipousuario.id" from="${sncpucmm.TipoUsuario.list()}" optionKey="id" value="2" optionValue="authority"  class="many-to-one" />

</div>

<div class="fieldcontain ${hasErrors(bean: usuarioInstance, field: 'cuentaFacebook', 'error')} ">
    <label for="cuentaFacebook">
        <g:message code="usuario.cuentaFacebook.label" default="Cuenta Facebook" />

    </label>
    <g:select id="cuentaFacebook" name="cuentaFacebook.id" from="${sncpucmm.CuentaFacebook.list()}" optionKey="id" value="${usuarioInstance?.cuentaFacebook?.id}" class="many-to-one" noSelection="['null': '']"/>

</div>
