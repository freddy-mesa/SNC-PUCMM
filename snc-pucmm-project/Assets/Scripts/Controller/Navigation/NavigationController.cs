﻿using SncPucmm.Controller.GUI;
using SncPucmm.Controller.Navigation;
using SncPucmm.Database;
using SncPucmm.Model;
using SncPucmm.Model.Navigation;
using SncPucmm.View;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.Controller.Navigation
{
    class NavigationController
    {
        #region Atributos
        Graph graph;

        public static bool isUsingNavigation; 

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
            isUsingNavigation = false;
            graph = new Graph();
            CreateGraph();
        }

        public void StartNavigation(string destinationName)
        {
            isUsingNavigation = true;
            
            //List<PathData> bestPath = GetBestPathData(destinationName);
            List<PathDataDijkstra> bestPath = graph.Dijkstra("Aulas 3", destinationName);

            //Mostrar el menu de direciones
            GUIMenuDirection menuDirection = new GUIMenuDirection("GUIMenuDirection", bestPath);
            MenuManager.GetInstance().AddMenu(menuDirection);

            State.ChangeState(eState.MenuDirection);

            isUsingNavigation = false;
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
            var sqliteService = SQLiteService.GetInstance();
            var reader = sqliteService.Query(
                true,
                "SELECT NOD.activo, NOD.nombre, NOD.idNodo, COO.latitud, COO.longitud FROM Nodo NOD, CoordenadaNodo COO " + 
                "WHERE NOD.idNodo = COO.idNodo ORDER BY NOD.idNodo"
            );

            while (reader.Read())
            {
                bool active = (System.Convert.ToInt32(reader["activo"]) == 0 ? false : true);

                if (active)
                {
                    string nombre = System.Convert.ToString(reader["nombre"]);
                    int idNode = System.Convert.ToInt32(reader["idNodo"]);
                    float latitud = System.Convert.ToSingle(reader["latitud"]);
                    float longitud = System.Convert.ToSingle(reader["longitud"]);
                                    
                    graph.Nodes.Add(new NodeDijkstra() { IdNode = idNode, Active = active, Name = nombre, Latitude = latitud, Longitude = longitud });
                }
            }

            reader = null;

            reader = sqliteService.Query(true, 
                "SELECT NEI.idNodo, NEI.idNodoNeighbor " + 
                "FROM Neighbor NEI, Nodo NOD1, Nodo NOD2 " + 
                "WHERE NOD1.activo = 1 AND NOD2.activo = 1 AND NOD1.idNodo = NEI.idNodo AND NOD2.idNodo = NEI.idNodoNeighbor"
            );

            while (reader.Read())
            {
                int idNodo = System.Convert.ToInt32(reader["idNodo"]);
                int idNodoNeighbor = System.Convert.ToInt32(reader["idNodoNeighbor"]);

                graph.AddNeighbor(idNodo, idNodoNeighbor);
            }
        }

        #endregion
    }
}
