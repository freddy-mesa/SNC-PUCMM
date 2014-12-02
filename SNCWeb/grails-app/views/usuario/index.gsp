
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
        <div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    Lista de Tours
                </div>
                <div class="panel-body">
                    <div class="table-responsive">
                        <table class="table table-striped table-hover" id="usuarios">
                            <thead>
                            <tr>

                                <th>Username</th>

                                <th>Habilidado</th>

                            </tr>
                            </thead>
                            <tbody>
                            <g:each in="${usuarioInstanceList}" status="i" var="usuarioInstance">
                                <tr class="${(i % 2) == 0 ? 'even gradeA' : 'odd gradeA'}">

                                    <td><g:link action="show" id="${usuarioInstance.id}">${fieldValue(bean: usuarioInstance, field: "username")}</g:link></td>

                                    <td><g:formatBoolean boolean="${usuarioInstance.enabled}" /></td>

                                </tr>
                            </g:each>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function() {
            $('#usuarios').dataTable();
        });
    </script>
</body>
</html>