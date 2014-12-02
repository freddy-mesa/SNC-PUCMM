<%@ page import="sncpucmm.UsuarioTipoUsuario; sncpucmm.Usuario" %>

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
