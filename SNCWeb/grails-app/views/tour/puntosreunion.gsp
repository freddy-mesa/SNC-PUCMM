<%@ page import="sncpucmm.PuntoReunionTour" %>
<!DOCTYPE html>
<html>
<head>
    <meta name="layout" content="main">
    <g:set var="entityName" value="${message(code: 'tour.label', default: 'Tour')}" />
    <title><g:message code="default.create.label" args="[entityName]" /></title>
</head>
<body>
<a href="#create-tour" class="skip" tabindex="-1"><g:message code="default.link.skip.label" default="Skip to content&hellip;"/></a>
<div class="nav" role="navigation">
    <ul>
        <li><a class="home" href="${createLink(uri: '/')}"><g:message code="default.home.label"/></a></li>
        <li><g:link class="list" action="index"><g:message code="default.list.label" args="[entityName]" /></g:link></li>
    </ul>
</div>
<div id="create-tour" class="content scaffold-create" role="main">
    <h1>Punto de Reunión</h1>
    <g:if test="${flash.message}">
        <div class="message" role="status">${flash.message}</div>
    </g:if>
    <g:hasErrors bean="${tourInstance}">
        <ul class="errors" role="alert">
            <g:eachError bean="${tourInstance}" var="error">
                <li <g:if test="${error in org.springframework.validation.FieldError}">data-field-id="${error.field}"</g:if>><g:message error="${error}"/></li>
            </g:eachError>
        </ul>
    </g:hasErrors>
    <g:if test="${puntosReunion}">
        <g:each in="${puntosReunion}" var="puntos">
            <li class="fieldcontain">

                <span class="property-value" aria-labelledby="puntos-label">${((PuntoReunionTour)puntos).nodo.nombre}</span>

            </li>
        </g:each>
    </g:if>
    <g:form action="savePuntos">
        <fieldset class="form">

            <g:hiddenField name="tour" value="${tour}"></g:hiddenField>
            <div class="fieldcontain ${hasErrors(bean: usuarioInstance, field: 'cuentaFacebook', 'error')} ">

                <label>Nodo</label>
                <g:select id="puntoreuniontour" name="puntoreuniontour" from="${sncpucmm.Nodo.list()}" optionKey="id" optionValue="nombre" class="many-to-one" noSelection="['null': '']"/>

            </div>
            <div class="fieldcontain ${hasErrors(bean: usuarioInstance, field: 'cuentaFacebook', 'error')} ">

                <label>No. Secuencia</label>
                <g:textField name="secuencia" class="secuencia" />

            </div>
        </fieldset>
        <fieldset class="buttons">
            <g:submitButton name="create" class="save" value="Añadir" />
        </fieldset>
    </g:form>
</div>
</body>
</html>
