
<%@ page import="sncpucmm.Usuario" %>
<!DOCTYPE html>
<html>
<head>
    <meta name="layout" content="sbadmin">
    <g:set var="entityName" value="${message(code: 'usuario.label', default: 'Usuario')}" />
    <title><g:message code="default.list.label" args="[entityName]" /></title>
</head>
<body>
<div class="row">
    <div class="col-lg-12">
        <h1 class="page-header">Usuarios</h1>
    </div>
    <!-- /.col-lg-12 -->
</div>

<div class="row">
    <div class="table-responsive">
        <table class="table table-striped table-bordered table-hover">
            <thead>
            <tr>

                <th>Username</th>

                <th>Cuenta Facebook</th>

                <th>Habilidado</th>

            </tr>
            </thead>
            <tbody>
                <g:each in="${usuarioInstanceList}" status="i" var="usuarioInstance">
                    <tr class="${(i % 2) == 0 ? 'even gradeA' : 'odd gradeA'}">

                        <td><g:link action="show" id="${usuarioInstance.id}">${fieldValue(bean: usuarioInstance, field: "username")}</g:link></td>

                        <td>${fieldValue(bean: usuarioInstance, field: "cuentaFacebook")}</td>

                        <td><g:formatBoolean boolean="${usuarioInstance.enabled}" /></td>

                    </tr>
                </g:each>
            </tbody>
        </table>
    </div>
</div>
</body>
</html>
