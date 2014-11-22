using SncPucmm.Controller.GUI;
using SncPucmm.Controller.Navigation;
using SncPucmm.Controller.Tours;
using SncPucmm.Database;
using SncPucmm.Model;
using SncPucmm.Model.Domain;
using SncPucmm.Model.Navigation;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.Controller.Navigation
{
    class NavigationController
    {
        #region Atributos

        Graph graph;

        #endregion

        #region Constructor

        public NavigationController()
        {
            Initializer();
        }

        #endregion

        #region Metodos

        private void Initializer()
        {
            CreateGraph();
        }

        public void StartNavigation(string destinationName)
        {           
            //List<PathData> bestPath = GetBestPathData(destinationName);
            List<PathDataDijkstra> bestPath = graph.Dijkstra("Aulas 4", destinationName);

            //Mostrar el menu de direciones
            MenuNavigation menuNavigation = new MenuNavigation("MenuNavigation", bestPath);
            MenuManager.GetInstance().AddMenu(menuNavigation);

            State.ChangeState(eState.MenuNavigation);
        }

        public void StartNavigation(int destinationIdNode)
        {
            //List<PathData> bestPath = GetBestPathData(destinationName);
            List<PathDataDijkstra> bestPath = graph.Dijkstra(11, destinationIdNode);

            //Mostrar el menu de direciones
            MenuNavigation menuNavigation = new MenuNavigation("MenuNavigation", bestPath);
            MenuManager.GetInstance().AddMenu(menuNavigation);

            State.ChangeState(eState.MenuNavigation);
        }

        public void StartTourNavigation(string tourName, List<DetalleUsuarioTour> detalleUsuarioTourList)
        {
            var tourController = new TourController(detalleUsuarioTourList);

            List<PathDataDijkstra> tourPath = new List<PathDataDijkstra>();
            bool isSelectCurrentIndexSectionTour = false;
            int indexCurrentTourPathData = 0;

            using (var sqlService = new SQLiteService())
            {
                for (int i = 0; i < detalleUsuarioTourList.Count; ++i)
                {
                    if (i + 1 != detalleUsuarioTourList.Count)
                    {
                        //Buscar en la base de dato el desde y el hasta
                        string desde = string.Empty, hasta = string.Empty;

                        var sql =
                            "SELECT NODA.nombre as desde, NODB.nombre as hasta " +
                            "FROM PuntoReunionTour PUNA, Nodo NODA, PuntoReunionTour PUNB, Nodo NODB " +
                            "WHERE PUNA.id = " + detalleUsuarioTourList[i].idPuntoReunionTour + " AND PUNA.idNodo = NODA.idNodo " +
                            "AND PUNB.id = " + detalleUsuarioTourList[i + 1].idPuntoReunionTour + " AND PUNB.idNodo = NODB.idNodo " +
                            "ORDER BY PUNA.id";

                        using (var result = sqlService.SelectQuery(sql))
                        {
                            if (result.Read())
                            {
                                desde = System.Convert.ToString(result["desde"]);
                                hasta = System.Convert.ToString(result["hasta"]);
                            }
                        }

                        List<PathDataDijkstra> bestPath = graph.Dijkstra(desde, hasta);
                        tourPath.AddRange(bestPath);

                        tourController.AddSectionTour(new SectionTourData()
                        {
                            Desde = desde,
                            IdPuntoReuionNodoDesde = detalleUsuarioTourList[i].idPuntoReunionTour.Value,
                            Hasta = hasta,
                            IdPuntoReuionNodoHasta = detalleUsuarioTourList[i + 1].idPuntoReunionTour.Value,
                        });

                        if (!isSelectCurrentIndexSectionTour && !detalleUsuarioTourList[i].fechaInicio.HasValue)
                        {
                            isSelectCurrentIndexSectionTour = true;
                            tourController.SetStartSectionTour(i);

                            indexCurrentTourPathData = tourPath.FindIndex(path => path.StartNode.Name == desde);
                        }
                    }
                }
            }

            ModelPoolManager.GetInstance().Add("tourCtrl", tourController);

            //Mostrar el menu de direciones
            MenuNavigation menuNavigation = new MenuNavigation("MenuNavigation", tourPath, indexCurrentTourPathData, tourName);
            MenuManager.GetInstance().AddMenu(menuNavigation);

            State.ChangeState(eState.MenuNavigation);
        }

        private List<PathDataDijkstra> GetBestPathData(string destinationName)
        {
            List<NodeDijkstra> startNodeList = findClosestNodes();

            List<PathDataDijkstra> bestPath = null;
            float minDistance = float.MaxValue;

            foreach (NodeDijkstra startNode in startNodeList)
            {
                var dataPath = graph.Dijkstra(startNode.Name, destinationName);

                if (dataPath[dataPath.Count - 1].DistancePathed < minDistance)
                {
                    bestPath = dataPath;
                    minDistance = dataPath[dataPath.Count - 1].DistancePathed;
                }
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                var path = new List<PathDataDijkstra>();

                path.Add(GetUsuarioPosition(bestPath[0].StartNode));
                path.AddRange(bestPath);

                return path;
            }
            else
            {
                return bestPath;
            }
        }

        private List<NodeDijkstra> findClosestNodes()
        {
            //Obteniendo la posicion actual del usuario
            float currentUserPosX = UIUtils.getXDistance(UIGPS.Longitude);
            float currentUserPosZ = UIUtils.getZDistance(UIGPS.Latitude);

            float meters = 0f;

            List<NodeDijkstra> nodeList = null;

            do {

                //Añade 25 metros
                meters += 25f;

                //Los nodos mas cercanos en un area segun los metros
                nodeList = graph.Nodes.FindAll(node =>
                {
                    float nodePosX = UIUtils.getXDistance(node.Longitude);
                    float nodePosY = UIUtils.getZDistance(node.Latitude);

                    //Distancia entre un nodo y el usuario
                    float resultant = UIUtils.getDirectDistance(currentUserPosX, currentUserPosZ, nodePosX, nodePosY);

                    if (resultant <= meters)
                    {
                        return true;
                    }

                    return false;
                });

            } while (nodeList != null && nodeList.Count == 0);

            return nodeList;
        }

        private PathDataDijkstra GetUsuarioPosition(NodeDijkstra startNavigationNode)
        {
            PathDataDijkstra userPos = new PathDataDijkstra();
            userPos.StartNode = new NodeDijkstra() { Latitude = UIGPS.Latitude, Longitude = UIGPS.Longitude, Name = "User Node" };
            userPos.EndNode = startNavigationNode;

            float adjacent = UIUtils.getXDistance(UIGPS.Longitude) - UIUtils.getXDistance(startNavigationNode.Longitude); //x1 - x2
            float opposite = UIUtils.getZDistance(UIGPS.Latitude) - UIUtils.getZDistance(startNavigationNode.Latitude);   //y1 - y2

            float distance = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));

            userPos.DistanceToNeighbor = distance;
            userPos.DistancePathed = distance;

            return userPos;
        }

        #endregion

        #region Graph Create/Update

        public void CreateGraph()
        {
            graph = new Graph();

            using (var sqlService = new SQLiteService())
            {
                string sql = "SELECT NOD.activo, NOD.nombre, NOD.planta, NOD.idNodo, NOD.idUbicacion, COO.latitud, COO.longitud " +
                            "FROM Nodo NOD LEFT JOIN CoordenadaNodo COO ON NOD.idNodo = COO.idNodo " +
                            "ORDER BY NOD.idNodo";

                using(var reader = sqlService.SelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        bool active = (Convert.ToInt32(reader["activo"]) == 0 ? false : true);

                        if (active)
                        {
                            string nombre = Convert.ToString(reader["nombre"]);
                            int idNode = Convert.ToInt32(reader["idNodo"]);

                            float valueFloat, latitud = 0, longitud = 0;

                            string obj = Convert.ToString(reader["latitud"]);
                            if (float.TryParse(obj, out valueFloat))
                            {
                                latitud = valueFloat;
                            }

                            obj = Convert.ToString(reader["longitud"]);
                            if (float.TryParse(obj, out valueFloat))
                            {
                                longitud = valueFloat;
                            }

                            bool isInsideBuilding = false;
                            string buildingName = null;
                            int planta = 0;

                            string strObject = Convert.ToString(reader["planta"]);
                            int value;
                            if (int.TryParse(strObject, out value))
                            {
                                planta = value;
                                isInsideBuilding = true;

                                obj = Convert.ToString(reader["idUbicacion"]);
                                int idUbicacion;
                                if (int.TryParse(obj, out idUbicacion))
                                {
                                    sql = "SELECT abreviacion FROM Ubicacion WHERE idUbicacion = " + idUbicacion; 
                                    using (var readerUbication = sqlService.SelectQuery(sql))
                                    {
                                        if (readerUbication.Read())
                                        {
                                            buildingName = Convert.ToString(readerUbication["abreviacion"]);
                                        }
                                    }
                                }
                            }

                            bool isBuilding = false;

                            if (!isInsideBuilding)
                            {
                                sql = "SELECT NOD.edificio, UBI.abreviacion FROM Nodo NOD, Ubicacion UBI " +
                                    "WHERE NOD.idNodo = " + idNode + " AND NOD.idUbicacion = UBI.idUbicacion";
                                using (var readerNodoUbicacion = sqlService.SelectQuery(sql))
                                {
                                    if (readerNodoUbicacion.Read())
                                    {
                                        string edificio = Convert.ToString(readerNodoUbicacion["edificio"]);
                                        if (int.TryParse(edificio, out value))
                                        {
                                            isBuilding = true;
                                            buildingName = Convert.ToString(readerNodoUbicacion["abreviacion"]);
                                        }
                                    }
                                }
                            }

                            graph.Nodes.Add(new NodeDijkstra() 
                            { 
                                IdNode = idNode, 
                                Active = active, 
                                Name = nombre, 
                                Latitude = latitud, 
                                Longitude = longitud, 
                                IsInsideBuilding = isInsideBuilding,
                                BuildingName = buildingName,
                                PlantaBuilding = planta,
                                IsBuilding = isBuilding
                            });
                        }
                    }
                }

                sql = "SELECT NEI.idNodo, NEI.idNodoNeighbor " +
                     "FROM Neighbor NEI, Nodo NOD1, Nodo NOD2 " +
                     "WHERE NOD1.activo = 1 AND NOD2.activo = 1 AND NOD1.idNodo = NEI.idNodo AND NOD2.idNodo = NEI.idNodoNeighbor";

                using (var reader = sqlService.SelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        int idNodo = System.Convert.ToInt32(reader["idNodo"]);
                        int idNodoNeighbor = System.Convert.ToInt32(reader["idNodoNeighbor"]);

                        graph.AddNeighbor(idNodo, idNodoNeighbor);
                    }
                }
            }
        }

        #endregion
    }
}
