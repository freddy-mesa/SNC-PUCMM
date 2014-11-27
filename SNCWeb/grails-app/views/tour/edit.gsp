<%@ page import="sncpucmm.Tour" %>
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
    <div>
        <div class="panel panel-default ">
            <div class="panel-heading">
                Datos del Tour
            </div>
            <div class="panel-body">

                <div class="row">
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
			<g:form url="[resource:tourInstance, action:'update']" method="PUT" class="form-group col-lg-6">
				<g:hiddenField name="version" value="${tourInstance?.version}" />
				<fieldset class="form">
					<g:render template="form"/>
				</fieldset>
				<fieldset class="buttons">
					<g:actionSubmit class="btn btn-primary" action="update" value="${message(code: 'default.button.update.label', default: 'Update')}" />
				</fieldset>
			</g:form>
		</div>
                </div>
            </div>
        </div>
        <script>
            $(document).ready(function() {
                $("select").addClass("input-sm")
            });
        </script>
	</body>
</html>
