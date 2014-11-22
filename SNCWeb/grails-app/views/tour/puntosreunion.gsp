<%@ page import="sncpucmm.PuntoReunionTour" %>

<div>
    <g:select multiple="true" id="puntosreuniontour" name="puntosreuniontour" from="${puntosReunion}" optionValue="${{it.nodo.nombre}}"/>
</div>
<script>
    $(document).ready(function() {
        $("select").addClass("input-sm")
    });
</script>