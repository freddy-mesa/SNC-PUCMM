
<%@ page import="sncpucmm.Tour" %>
<!DOCTYPE html>
<html>
<head>
    <meta name="layout" content="main">
    <g:set var="entityName" value="${message(code: 'tour.label', default: 'Tour')}" />
    <title><g:message code="default.show.label" args="[entityName]" /></title>
</head>
<body>
<a href="#show-tour" class="skip" tabindex="-1"><g:message code="default.link.skip.label" default="Skip to content&hellip;"/></a>
<div class="nav" role="navigation">
    <ul>
        <li><a class="home" href="${createLink(uri: '/')}"><g:message code="default.home.label"/></a></li>
        <li><g:link class="list" action="index"><g:message code="default.list.label" args="[entityName]" /></g:link></li>
        <li><g:link class="create" action="create"><g:message code="default.new.label" args="[entityName]" /></g:link></li>
    </ul>
</div>
<div id="show-tour" class="content scaffold-show" role="main">
    <h1><g:message code="default.show.label" args="[entityName]" /></h1>
    <g:if test="${flash.message}">
        <div class="message" role="status">${flash.message}</div>
    </g:if>
    <ol class="property-list tour">

        <g:if test="${tourInstance?.nombreTour}">
            <li class="fieldcontain">
                <span id="nombreTour-label" class="property-label"><g:message code="tour.nombreTour.label" default="Nombre Tour" /></span>

                <span class="property-value" aria-labelledby="nombreTour-label"><g:fieldValue bean="${tourInstance}" field="nombreTour"/></span>

            </li>
        </g:if>

        <g:if test="${tourInstance?.fechaInicio}">
            <li class="fieldcontain">
                <span id="fechaInicio-label" class="property-label"><g:message code="tour.fechaInicio.label" default="Fecha Inicio" /></span>

                <span class="property-value" aria-labelledby="fechaInicio-label"><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance?.fechaInicio}" /></span>

            </li>
        </g:if>

        <g:if test="${tourInstance?.fechaFin}">
            <li class="fieldcontain">
                <span id="fechaFin-label" class="property-label"><g:message code="tour.fechaFin.label" default="Fecha Fin" /></span>

                <span class="property-value" aria-labelledby="fechaFin-label"><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance?.fechaFin}" /></span>

            </li>
        </g:if>

        <g:if test="${tourInstance?.creador}">
            <li class="fieldcontain">
                <span id="creador-label" class="property-label"><g:message code="tour.creador.label" default="Creador" /></span>

                <span class="property-value" aria-labelledby="creador-label"><g:link controller="usuario" action="show" id="${tourInstance?.creador?.id}">${tourInstance?.creador.username}</g:link></span>

            </li>
        </g:if>

        <g:if test="${tourInstance?.fechaCreacion}">
            <li class="fieldcontain">
                <span id="fechaCreacion-label" class="property-label"><g:message code="tour.fechaCreacion.label" default="Fecha Creacion" /></span>

                <span class="property-value" aria-labelledby="fechaCreacion-label"><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance?.fechaCreacion}" /></span>

            </li>
        </g:if>

    </ol>
    <g:form url="[resource:tourInstance, action:'delete']" method="DELETE">
        <fieldset class="buttons">
            <g:link class="edit" action="puntosreunion" resource="${tourInstance}">Añadir Puntos de Reunión</g:link>
            <g:link class="edit" action="edit" resource="${tourInstance}"><g:message code="default.button.edit.label" default="Edit" /></g:link>
            <g:actionSubmit class="delete" action="delete" value="${message(code: 'default.button.delete.label', default: 'Delete')}" onclick="return confirm('${message(code: 'default.button.delete.confirm.message', default: 'Are you sure?')}');" />
        </fieldset>
    </g:form>
</div>
</body>
</html>
