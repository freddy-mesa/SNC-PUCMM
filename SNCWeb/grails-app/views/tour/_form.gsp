<%@ page import="sncpucmm.Tour" %>

<div class="fieldcontain ${hasErrors(bean: tourInstance, field: 'nombreTour', 'error')} required">
    <label for="nombreTour">
        <g:message code="tour.nombreTour.label" default="Nombre Tour" />
        <span class="required-indicator">*</span>
    </label>
    <g:textField name="nombreTour" required="" value="${tourInstance?.nombreTour}"/>

</div>

<div class="fieldcontain ${hasErrors(bean: tourInstance, field: 'fechaInicio', 'error')} required">
    <label for="fechaInicio">
        <g:message code="tour.fechaInicio.label" default="Fecha Inicio" />
        <span class="required-indicator">*</span>
    </label>
    <g:datePicker name="fechaInicio" precision="minute"  value="${tourInstance?.fechaInicio}"  />

</div>

<div class="fieldcontain ${hasErrors(bean: tourInstance, field: 'fechaFin', 'error')} ">
    <label for="fechaFin">
        <g:message code="tour.fechaFin.label" default="Fecha Fin" />

    </label>
    <g:datePicker name="fechaFin" precision="minute"  value="${tourInstance?.fechaFin}" default="none" noSelection="['': '']" />

</div>
