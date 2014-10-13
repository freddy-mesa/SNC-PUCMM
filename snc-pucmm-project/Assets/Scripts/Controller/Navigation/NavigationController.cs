using SncPucmm.Controller.Direction;
using SncPucmm.Controller.Navigation;
using SncPucmm.Model;
using SncPucmm.Model.Navigation;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            graph = new Graph();
            InitGraph();
        }

        public void StartNavigation(String destinationName)
        {
            UIUtils.DestroyChilds("/PUCMM/Directions", false);

            //List<PathData> bestPath = GetBestPathData(destinationName);
            List<PathData> bestPath = graph.Dijkstra("Aulas 3", destinationName);
            DirectionController.StartDirection(bestPath);

        }

        private List<PathData> GetBestPathData(String destinationName)
        {
            List<Node> startNodeList = findClosestNodes();

            List<PathData> bestPath = null;
            float minDistance = float.MaxValue;

            foreach (Node startNode in startNodeList)
            {
                var dataPath = graph.Dijkstra(startNode.Name, destinationName);

                if (dataPath[dataPath.Count - 1].DistancePathed < minDistance)
                {
                    bestPath = dataPath;
                    minDistance = dataPath[dataPath.Count - 1].DistancePathed;
                }
            }

            return bestPath;
        }

        private List<Node> findClosestNodes()
        {
            float currentUserPosX = UIUtils.getXDistance(UIGPS.Longitude);
            float currentUserPosZ = UIUtils.getZDistance(UIGPS.Latitude);

            //Los nodos mas cercanos en un area de 20 metros
            List<Node> nodeList = graph.Nodes.FindAll(node =>
            {
                bool isNear = false;

                float nodePosX = UIUtils.getXDistance(node.Longitude);
                float nodePosY = UIUtils.getZDistance(node.Latitude);

                float resultant = UIUtils.getDirectDistance(currentUserPosX, currentUserPosZ, nodePosX, nodePosY);

                if (resultant <= 20f)
                {
                    isNear = true;
                }

                return isNear;
            });

            return nodeList;
        }

        #endregion

        #region Graph Insertion

        private void InitGraph()
        {
            graph.Nodes.Add(new Node() { Name = "Aulas 1", Latitude = 19.442731f, Longitude = -70.683049f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Aulas 2", Latitude = 19.443009f, Longitude = -70.681736f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Aulas 3", Latitude = 19.441522f, Longitude = -70.683402f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Aulas 4", Latitude = 19.443083f, Longitude = -70.683407f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Centro de Estudiantes", Latitude = 19.443879f, Longitude = -70.682780f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Ciencias de la Salud", Latitude = 19.443699f, Longitude = -70.681666f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Ciencias Básicas I", Latitude = 19.442237f, Longitude = -70.683398f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Ciencias Básicas II", Latitude = 19.442070f, Longitude = -70.683046f, Active = true });
            //graph.Nodes.Add(new Node() { Name = "Padre Arroyo", Latitude = 19.442303f, Longitude = -70.684772f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Departamentos de Ingeniería", Latitude = 19.441756f, Longitude = -70.683045f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Laboratorios de Ingeniería", Latitude = 19.441074f, Longitude = -70.682723f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Departamento de Ingenierías Electrónica y Electromecánica", Latitude = 19.440198f, Longitude = -70.683129f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Suministro y Talleres", Latitude = 19.440648f, Longitude = -70.683352f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Biblioteca", Latitude = 19.443727f, Longitude = -70.684183f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Talleres de Ingeniería Eléctrica y Electromecánica", Latitude = 19.440310f, Longitude = -70.682706f, Active = true });

            graph.Nodes.Add(new Node() { Name = "Node 1", Latitude = 19.440313f, Longitude = -70.683129f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 2", Latitude = 19.440400f, Longitude = -70.683125f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 3", Latitude = 19.440573f, Longitude = -70.683132f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 3.5", Latitude = 19.440544f, Longitude = -70.683199f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 4", Latitude = 19.440697f, Longitude = -70.683132f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 4.5", Latitude = 19.440652f, Longitude = -70.683205f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 5", Latitude = 19.440788f, Longitude = -70.682727f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 6", Latitude = 19.440315f, Longitude = -70.682848f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 6.5", Latitude = 19.440409f, Longitude = -70.682754f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 7", Latitude = 19.440293f, Longitude = -70.683218f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 8", Latitude = 19.440270f, Longitude = -70.683475f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 8.5", Latitude = 19.440301f, Longitude = -70.683658f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 9", Latitude = 19.440711f, Longitude = -70.683580f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 9.5", Latitude = 19.440665f, Longitude = -70.683498f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 10", Latitude = 19.441075f, Longitude = -70.683132f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 11", Latitude = 19.441135f, Longitude = -70.683132f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 12", Latitude = 19.441468f, Longitude = -70.683127f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 13", Latitude = 19.441459f, Longitude = -70.683335f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 14", Latitude = 19.441140f, Longitude = -70.682723f, Active = true });
            //graph.Nodes.Add(new Node() { Name = "Node 15", Latitude = f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 16", Latitude = 19.441480f, Longitude = -70.682723f, Active = true });
            //graph.Nodes.Add(new Node() { Name = "Node 17", Latitude = f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 18", Latitude = 19.441679f, Longitude = -70.682722f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 19", Latitude = 19.441684f, Longitude = -70.683045f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 20", Latitude = 19.441683f, Longitude = -70.683398f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 20.5", Latitude = 19.441608f, Longitude = -70.683400f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 21", Latitude = 19.441608f, Longitude = -70.683400f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 21.5", Latitude = 19.441145f, Longitude = -70.683655f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 22", Latitude = 19.441930f, Longitude = -70.683395f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 23", Latitude = 19.441930f, Longitude = -70.683140f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 24", Latitude = 19.441979f, Longitude = -70.683082f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 24.5", Latitude = 19.442012f, Longitude = -70.683082f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 25", Latitude = 19.441850f, Longitude = -70.683060f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 25.5", Latitude = 19.441853f, Longitude = -70.683066f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 26", Latitude = 19.442589f, Longitude = -70.683049f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 27", Latitude = 19.442570f, Longitude = -70.683402f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 28", Latitude = 19.442305f, Longitude = -70.683405f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 29", Latitude = 19.442743f, Longitude = -70.682186f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 30", Latitude = 19.442811f, Longitude = -70.681733f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 31", Latitude = 19.443074f, Longitude = -70.681760f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 32", Latitude = 19.442917f, Longitude = -70.681744f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 33", Latitude = 19.443790f, Longitude = -70.682307f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 34", Latitude = 19.443403f, Longitude = -70.682822f, Active = true });
            //graph.Nodes.Add(new Node() { Name = "Node 35", Latitude = f, Active = true });
            //graph.Nodes.Add(new Node() { Name = "Node 36", Latitude = f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 37", Latitude = 19.443418f, Longitude = -70.683270f, Active = true });
            //graph.Nodes.Add(new Node() { Name = "Node 38", Latitude = f, Active = true });
            //graph.Nodes.Add(new Node() { Name = "Node 39", Latitude = f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 40", Latitude = 19.443836f, Longitude = -70.683149f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 41", Latitude = 19.443775f, Longitude = -70.683777f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 42", Latitude = 19.443732f, Longitude = -70.683898f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 43", Latitude = 19.443671f, Longitude = -70.684035f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 44", Latitude = 19.443652f, Longitude = -70.684129f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 45", Latitude = 19.443666f, Longitude = -70.684180f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 46", Latitude = 19.443378f, Longitude = -70.684180f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 47", Latitude = 19.443375f, Longitude = -70.684515f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 48", Latitude = 19.443272f, Longitude = -70.684155f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 48.5", Latitude = 19.442963f, Longitude = -70.683967f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 49", Latitude = 19.443165f, Longitude = -70.683640f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 50", Latitude = 19.443158f, Longitude = -70.683489f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 51", Latitude = 19.443319f, Longitude = -70.683390f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 52", Latitude = 19.443167f, Longitude = -70.683107f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 53", Latitude = 19.443163f, Longitude = -70.683390f, Active = true });
            graph.Nodes.Add(new Node() { Name = "Node 54", Latitude = 19.442731f, Longitude = -70.682021f, Active = true });

            //New Nodes
            graph.Nodes.Add(new Node() { Name = "N1", Latitude = 19.440695f, Longitude = -70.683214f, Active = true });
            graph.Nodes.Add(new Node() { Name = "N2", Latitude = 19.440589f, Longitude = -70.683217f, Active = true });
            graph.Nodes.Add(new Node() { Name = "N3", Latitude = 19.440715f, Longitude = -70.683501f, Active = true });
            graph.Nodes.Add(new Node() { Name = "N4", Latitude = 19.440581f, Longitude = -70.683506f, Active = true });
            graph.Nodes.Add(new Node() { Name = "N5", Latitude = 19.440582f, Longitude = -70.683577f, Active = true });

            //New Neighbors
            graph.AddNeighbor("N1", "Node 4");
            graph.AddNeighbor("N1", "Suministro y Talleres");
            graph.AddNeighbor("N2", "Node 3");
            graph.AddNeighbor("N2", "Suministro y Talleres");
            graph.AddNeighbor("N3", "Node 9");
            graph.AddNeighbor("N3", "Suministro y Talleres");
            graph.AddNeighbor("N4", "N5");
            graph.AddNeighbor("N4", "Suministro y Talleres");
            graph.AddNeighbor("N5", "Node 8.5");
            graph.AddNeighbor("N5", "Node 9");
            
            graph.AddNeighbor("Node 1", "Departamento de Ingenierías Electrónica y Electromecánica");
            graph.AddNeighbor("Node 1", "Node 2");
            graph.AddNeighbor("Node 1", "Node 7");
            graph.AddNeighbor("Node 1", "Node 6");
            graph.AddNeighbor("Node 2", "Node 3");
            graph.AddNeighbor("Node 2", "Node 7");
            graph.AddNeighbor("Node 3", "Node 4");
            graph.AddNeighbor("Node 3", "Node 3.5");
            graph.AddNeighbor("Node 3", "Node 6");
            graph.AddNeighbor("Node 3.5", "Suministro y Talleres");
            graph.AddNeighbor("Node 4", "4.5");
            graph.AddNeighbor("Node 4", "Node 5");
            graph.AddNeighbor("Node 4", "Node 10");
            graph.AddNeighbor("Node 4.5", "Suministro y Talleres");
            graph.AddNeighbor("Node 5", "Node 6.5");
            graph.AddNeighbor("Node 5", "Laboratorios de Ingeniería");
            graph.AddNeighbor("Node 6", "6.5");
            graph.AddNeighbor("Node 6", "Talleres de Ingeniería Eléctrica y Electromecánica");
            graph.AddNeighbor("Node 6.5", "Talleres de Ingeniería Eléctrica y Electromecánica");
            graph.AddNeighbor("Node 7", "Node 8");
            graph.AddNeighbor("Node 8", "Node 8.5");
            graph.AddNeighbor("Node 9", "Node 21");
            graph.AddNeighbor("Node 9", "9.5");
            graph.AddNeighbor("Node 9.5", "Suministro y Talleres");
            graph.AddNeighbor("Node 10", "Node 11");
            graph.AddNeighbor("Node 10", "Node 13");
            graph.AddNeighbor("Node 11", "Node 14");
            graph.AddNeighbor("Node 11", "Node 12");
            graph.AddNeighbor("Node 12", "Node 13");
            graph.AddNeighbor("Node 12", "Node 16");
            graph.AddNeighbor("Node 12", "Node 18");
            graph.AddNeighbor("Node 12", "Node 19");
            graph.AddNeighbor("Node 13", "Aulas 3");
            graph.AddNeighbor("Node 14", "Laboratorios de Ingeniería");
            graph.AddNeighbor("Node 14", "Node 16");
            graph.AddNeighbor("Node 16", "Node 17");//Revisar estos nodos. Como que hay uno de más.
            graph.AddNeighbor("Node 17", "Node 18");
            graph.AddNeighbor("Node 18", "Node 54");
            graph.AddNeighbor("Node 19", "Node 20");
            graph.AddNeighbor("Node 19", "Departamentos de Ingeniería");
            graph.AddNeighbor("Node 20", "Node 22");
            graph.AddNeighbor("Node 20", "Node 20.5");
            graph.AddNeighbor("Node 20.5", "Aulas 3");
            graph.AddNeighbor("Node 21", "Node 21.5");
            graph.AddNeighbor("Node 21", "Aulas 3");
            graph.AddNeighbor("Node 22", "Node 23");
            graph.AddNeighbor("Node 22", "Ciencias Básicas I");
            graph.AddNeighbor("Node 23", "Node 24");
            graph.AddNeighbor("Node 23", "Node 25");
            graph.AddNeighbor("Node 24", "Node 25");
            graph.AddNeighbor("Node 24", "Node 24.5");
            graph.AddNeighbor("Node 24.5", "Ciencias Básicas II");
            graph.AddNeighbor("Node 25", "Node 25.5");
            graph.AddNeighbor("Node 25.5", "Departamentos de Ingeniería");
            graph.AddNeighbor("Node 26", "Ciencias Básicas II");
            graph.AddNeighbor("Node 26", "Aulas 1");
            graph.AddNeighbor("Node 26", "Node 27");
            graph.AddNeighbor("Node 26", "Node 28");
            graph.AddNeighbor("Node 26", "Node 29");
            graph.AddNeighbor("Node 27", "Node 28");
            graph.AddNeighbor("Node 27", "Aulas 4");
            graph.AddNeighbor("Node 28", "Ciencias Básicas I");
            graph.AddNeighbor("Node 29", "Node 54");
            graph.AddNeighbor("Node 29", "Node 34");
            graph.AddNeighbor("Node 30", "Node 54");
            graph.AddNeighbor("Node 30", "Aulas 2");
            graph.AddNeighbor("Node 31", "Aulas 2");
            graph.AddNeighbor("Node 31", "Ciencias de la Salud");
            graph.AddNeighbor("Node 31", "Node 32");
            graph.AddNeighbor("Node 32", "Node 33");
            graph.AddNeighbor("Node 32", "Node 34");
            graph.AddNeighbor("Node 33", "Centro de Estudiantes");
            graph.AddNeighbor("Node 33", "Ciencias de la Salud");
            graph.AddNeighbor("Node 34", "Node 35");
            graph.AddNeighbor("Node 35", "Node 36");
            graph.AddNeighbor("Node 35", "Node 39");
            graph.AddNeighbor("Node 36", "Node 37");
            graph.AddNeighbor("Node 36", "Node 38");
            graph.AddNeighbor("Node 36", "Node 52");
            graph.AddNeighbor("Node 37", "Node 38");
            graph.AddNeighbor("Node 37", "Node 51");
            graph.AddNeighbor("Node 38", "Node 39");
            graph.AddNeighbor("Node 39", "Node 40");
            graph.AddNeighbor("Node 40", "Centro de Estudiantes");
            graph.AddNeighbor("Node 40", "Node 41");
            graph.AddNeighbor("Node 41", "Node 42");
            graph.AddNeighbor("Node 42", "Node 43");
            graph.AddNeighbor("Node 42", "Node 51");
            graph.AddNeighbor("Node 43", "Node 44");
            graph.AddNeighbor("Node 43", "Biblioteca");
            graph.AddNeighbor("Node 44", "Node 45");
            graph.AddNeighbor("Node 44", "Node 48");
            graph.AddNeighbor("Node 45", "Node 46");
            graph.AddNeighbor("Node 46", "Node 47");
            graph.AddNeighbor("Node 48", "Node 48.5");
            graph.AddNeighbor("Node 48.5", "Node 49");
            graph.AddNeighbor("Node 49", "Node 50");
            graph.AddNeighbor("Node 49", "Node 51");
            graph.AddNeighbor("Node 50", "Aulas 4");
            graph.AddNeighbor("Node 51", "Node 53");
            graph.AddNeighbor("Node 52", "Node 53");
            graph.AddNeighbor("Node 53", "Aulas 4");
        }

        #endregion
    }
}
