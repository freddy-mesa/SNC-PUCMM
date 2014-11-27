
<%@ page import="sncpucmm.Tour" %>
<!DOCTYPE html>
<html>
<head>
    <meta name="layout" content="sbadmin">
    <g:set var="entityName" value="${message(code: 'tour.label', default: 'Tour')}" />
    <title><g:message code="default.list.label" args="[entityName]" /></title>

    <asset:stylesheet src="plugins/dataTables.bootstrap.css"></asset:stylesheet>

</head>
<body>

<div class="row">
    <div class="col-lg-12">
        <h1 class="page-header">Tours</h1>
    </div>
    <!-- /.col-lg-12 -->
</div>
<div class="row">

    <div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Lista de Tours
            </div>
            <div class="panel-body">
                <div class="table-responsive">
                    <table class="table table-striped table-hover" id="tours">
                        <thead>
                        <tr>

                            <th>Nombre</th>

                            <th>Fecha Inicio</th>

                            <th>Fecha Fin</th>

                            <th>Creador</th>

                            <th>Fecha Creacion</th>

                        </tr>
                        </thead>
                        <tbody>
                        <g:each in="${tourInstanceList}" status="i" var="tourInstance">
                            <tr class="${(i % 2) == 0 ? 'even gradeA' : 'odd gradeA'}">

                                <td><g:link action="show" id="${tourInstance.id}">${fieldValue(bean: tourInstance, field: "nombreTour")}</g:link></td>

                                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance.fechaInicio}" /></td>

                                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance.fechaFin}"/></td>

                                <td>${fieldValue(bean: tourInstance.creador, field: "username")}</td>

                                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance.fechaCreacion}" /></td>
                            </tr>
                        </g:each>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <script>
            $(document).ready(function() {
                $('#tours').dataTable();
            });
        </script>
    </div>
</body>
</html>
