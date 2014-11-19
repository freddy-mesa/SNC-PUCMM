<!DOCTYPE html>
<html>
	<head>
		<meta name="layout" content="sbadmin">
		<g:set var="entityName" value="${message(code: 'tour.label', default: 'Tour')}" />
		<title><g:message code="default.create.label" args="[entityName]" /></title>
	</head>
	<body>
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Crear Tour</h1>
        </div>
    </div>
		<div class="row" ">
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
			<g:form url="[resource:tourInstance, action:'save']" role="form" class="form-group">
				<fieldset class="form-group">
					<g:render template="form"/>
				</fieldset>
				<fieldset>
					<g:submitButton name="create" class="btn btn-primary" value="${message(code: 'default.button.create.label', default: 'Create')}" />
				</fieldset>
			</g:form>
		</div>
	</body>
</html>
