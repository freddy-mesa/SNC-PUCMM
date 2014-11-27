<%--
  Created by IntelliJ IDEA.
  User: Yandri
  Date: 20/11/2014
  Time: 16:09
--%>
<%@ page import="sncpucmm.PuntoReunionTour" %>

<g:select multiple="true" id="puntoreuniontour" name="puntoreuniontour" from="${sncpucmm.Nodo.findAllByNombreNotBetween("Node", "Node 9.9")}" optionKey="id" optionValue="nombre"/>
