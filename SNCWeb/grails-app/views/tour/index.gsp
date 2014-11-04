
<%@ page import="sncpucmm.Tour" %>
<!DOCTYPE html>
<html>
<head>
    <meta name="layout" content="main">
    <g:set var="entityName" value="${message(code: 'tour.label', default: 'Tour')}" />
    <title><g:message code="default.list.label" args="[entityName]" /></title>
</head>
<body>
<a href="#list-tour" class="skip" tabindex="-1"><g:message code="default.link.skip.label" default="Skip to content&hellip;"/></a>
<div class="nav" role="navigation">
    <ul>
        <li><a class="home" href="${createLink(uri: '/')}"><g:message code="default.home.label"/></a></li>
        <li><g:link class="create" action="create"><g:message code="default.new.label" args="[entityName]" /></g:link></li>
    </ul>
</div>
<div id="list-tour" class="content scaffold-list" role="main">
    <h1><g:message code="default.list.label" args="[entityName]" /></h1>
    <g:if test="${flash.message}">
        <div class="message" role="status">${flash.message}</div>
    </g:if>
    <table>
        <thead>
        <tr>

            <g:sortableColumn property="nombreTour" title="${message(code: 'tour.nombreTour.label', default: 'Nombre Tour')}" />

            <th><g:message code="tour.creador.label" default="Creador" /></th>

            <g:sortableColumn property="fechaInicio" title="${message(code: 'tour.fechaInicio.label', default: 'Fecha Inicio')}" />

            <g:sortableColumn property="fechaFin" title="${message(code: 'tour.fechaFin.label', default: 'Fecha Fin')}" />

            <g:sortableColumn property="fechaCreacion" title="${message(code: 'tour.fechaCreacion.label', default: 'Fecha Creacion')}" />

        </tr>
        </thead>
        <tbody>
        <g:each in="${tourInstanceList}" status="i" var="tourInstance">
            <tr class="${(i % 2) == 0 ? 'even' : 'odd'}">

                <td><g:link action="show" id="${tourInstance.id}">${fieldValue(bean: tourInstance, field: "nombreTour")}</g:link></td>

                <td>${fieldValue(bean: tourInstance.creador, field: "username")}</td>

                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance.fechaInicio}" /></td>

                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance.fechaFin}"/></td>

                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance.fechaCreacion}" /></td>
            </tr>
        </g:each>
        </tbody>
    </table>
    <div class="pagination">
        <g:paginate total="${tourInstanceCount ?: 0}" />
    </div>
</div>
</body>
</html>
