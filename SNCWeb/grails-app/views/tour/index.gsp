
<%@ page import="sncpucmm.Tour" %>
<!DOCTYPE html>
<html>
<head>
    <meta name="layout" content="sbadmin">
    <g:set var="entityName" value="${message(code: 'tour.label', default: 'Tour')}" />
    <title><g:message code="default.list.label" args="[entityName]" /></title>
</head>
<body>

<div class="row">
    <div class="col-lg-12">
        <h1 class="page-header">Tours</h1>
    </div>
    <!-- /.col-lg-12 -->
</div>
<div class="row">
    <div class="table-responsive">
        <table class="table table-striped table-bordered table-hover">
        <thead>
        <tr>

            <th>Nombre</th>

            <th>Creador</th>

            <th>Fecha Inicio</th>

            <th>Fecha Fin</th>

            <th>Fecha Creacion</th>

        </tr>
        </thead>
        <tbody>
        <g:each in="${tourInstanceList}" status="i" var="tourInstance">
            <tr class="${(i % 2) == 0 ? 'even gradeA' : 'odd gradeA'}">

                <td><g:link action="show" id="${tourInstance.id}">${fieldValue(bean: tourInstance, field: "nombreTour")}</g:link></td>

                <td>${fieldValue(bean: tourInstance.creador, field: "username")}</td>

                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance.fechaInicio}" /></td>

                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance.fechaFin}"/></td>

                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance.fechaCreacion}" /></td>
            </tr>
        </g:each>
        </tbody>
    </table>
        </div>
</div>
</body>
</html>
