<%@ page import="sncpucmm.PuntoReunionTour; sncpucmm.Tour" %>
<!DOCTYPE html>
<html>
	<head>
		<meta name="layout" content="sbadmin">
		<g:set var="entityName" value="${message(code: 'tour.label', default: 'Tour')}" />
		<title><g:message code="default.edit.label" args="[entityName]" /></title>
	</head>
	<body>
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Editar: ${tourInstance.nombreTour}</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Datos del Tour
                </div>
                <!-- .panel-heading -->
                <div class="panel-body">

                <div class="panel-group" id="accordion">
                    <g:form url="[resource:tourInstance, action:'actualizartour']">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    Información General
                                </h4>
                            </div>
                            <div class="panel-body">
                                <div class="col-lg-6">
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

                                    <fieldset>
                                        <g:render template="form"/>
                                    </fieldset>
                                </div>

                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    Puntos de Reunion
                                </h4>
                            </div>
                            <div class="panel-body">
                                <div class="col-lg-5">
                                    <fieldset>
                                        <g:render template="formpuntosreunion"/>
                                    </fieldset>
                                </div>
                                <div style="padding-top:50px" class="col-lg-2">
                                    <g:submitToRemote url="[action:'puntosreunion']" update="puntosReunion"  name="anadir" class="btn btn-info"  value="&nbsp &nbsp Añadir &gt&gt" />
                                    <div>&nbsp;</div>
                                    <g:submitToRemote url="[action:'puntosreunionremover']" update="puntosReunion"  name="remover" class="btn btn-info"  value="&lt&lt Remover" />
                                </div>
                                <div id="puntosReunion" class="col-md-3">
                                    <div>
                                        <g:select multiple="true" id="puntosreuniontour" name="puntosreuniontour" from="${puntosReunion}" optionKey="id" optionValue="${{it.nombre}}"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                        <fieldset>
                            <g:submitButton name="create" class="btn btn-primary btn-lg" value="Actualizar" />
                        </fieldset>
                    </g:form>
                </div>
                <!-- .panel-body -->
            </div>
            <!-- /.panel -->
        </div>
        <!-- /.col-lg-12 -->
    </div>
    <!-- /.row -->
        <script>
            $(document).ready(function() {
                $("select").addClass("input-sm")
            });
        </script>
	</body>
</html>
