
<%@ page import="sncpucmm.Tour" %>
<!DOCTYPE html>
<html>
<head>
    <meta name="layout" content="sbadmin">
    <g:set var="entityName" value="${message(code: 'tour.label', default: 'Tour')}" />
    <title><g:message code="default.show.label" args="[entityName]" /></title>
</head>
<body>
<div class="row">
    <div class="col-lg-12">
        <h1 class="page-header">${tourInstance.nombreTour}</h1>
    </div>
</div>
<div class="row">
    <div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Información General
            </div>
            <!-- /.panel-heading -->
            <div class="panel-body">
                <div class="table-responsive">
                    <table class="table">
                        <thead>
                        <tr>
                            <th>Nombre</th>
                            <th>Fecha de Inicio</th>
                            <th>Fecha Fin</th>
                            <th>Cantidad de Puntos</th>
                            <th>Creador</th>
                            <th>Fecha de Creación</th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr>
                            <td>${tourInstance.nombreTour}</td>
                            <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance?.fechaInicio}" /></td>
                            <g:if test="${tourInstance?.fechaFin}">
                                <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance?.fechaFin}" /></td>
                            </g:if>
                            <g:else>
                                <td>-</td>
                            </g:else>
                            <g:if test="${sncpucmm.PuntoReunionTour.findAllByTour(tourInstance)}">
                                <td>${sncpucmm.PuntoReunionTour.findAllByTour(tourInstance).size()}</td>
                            </g:if>
                            <g:else>
                                <td>0</td>
                            </g:else>
                            <td>${tourInstance.creador.username}</td>
                            <td><g:formatDate format="dd/MM/yyyy HH:mm:ss" date="${tourInstance?.fechaCreacion}" /></td>
                        </tr>
                        </tbody>
                    </table>
                </div>
                <!-- /.table-responsive -->
            </div>
            <!-- /.panel-body -->
        </div>
        <!-- /.panel -->
    </div>
    <!-- /.col-lg-6 -->
    <div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Puntos de Reunión
            </div>
            <!-- /.panel-heading -->
            <div class="panel-body">
                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead>
                        <tr>
                            <th>No. Secuencia</th>
                            <th>Nombre</th>
                            <th>Nombre Ubicación</th>
                        </tr>
                        </thead>
                        <tbody>
                        <g:each in="${sncpucmm.PuntoReunionTour.findAllByTour(tourInstance)}" var="puntoReunion">
                            <tr>
                                <td>${puntoReunion.secuenciaPuntoReunion}</td>
                                <td>${puntoReunion.nodo.nombre}</td>
                                <td>${puntoReunion.nodo.ubicacion.nombre}</td>
                            </tr>
                        </g:each>
                        </tbody>
                    </table>
                </div>
                <!-- /.table-responsive -->
            </div>
            <!-- /.panel-body -->
        </div>
        <!-- /.panel -->
    </div>

    <g:form url="[resource:tourInstance, action:'delete']" method="DELETE">
        <fieldset class="buttons">

            <g:link class="btn btn-primary" action="edit" resource="${tourInstance}">Editar</g:link>
            <g:actionSubmit class="btn btn-danger" action="delete" value="Eliminar" onclick="return confirm('${message(code: 'default.button.delete.confirm.message', default: 'Are you sure?')}');" />
        </fieldset>
    </g:form>
</div>
</body>
</html>
