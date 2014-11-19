<!DOCTYPE html>
<html>
	<head>
		<meta name="layout" content="sbadmin"/>
		<title>SNC Web</title>
	</head>
	<body>
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Available Controllers</h1>
                <div>
                    <ul>
                        <g:each var="c" in="${grailsApplication.controllerClasses.sort { it.fullName } }">
                            <li class="controller"><g:link controller="${c.logicalPropertyName}">${c.fullName}</g:link></li>
                        </g:each>
                    </ul>
                </div>
            </div>
        </div>
	</body>
</html>
