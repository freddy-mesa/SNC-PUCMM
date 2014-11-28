<%@ page import="sncpucmm.Nodo" %>

<div>
    <g:select multiple="true" id="puntosreuniontour" name="puntosreuniontour" from="${puntosReunion}" optionKey="id" optionValue="${{it.nombre}}"/>
</div>
<script>
    $(document).ready(function() {
        $("select").addClass("input-sm")
    });
</script>