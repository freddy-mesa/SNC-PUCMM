using SncPucmm.Model.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SncPucmm.View;

namespace SncPucmm.Controller.Navigation
{
    public class Graph
    {
        #region Propiedades

        public List<Node> Nodes { get; set; }
        
        #endregion

        #region Constructor

        public Graph()
        {
            Nodes = new List<Node>();
        }

        #endregion

        #region Metodos

        public void AddNeighbor(Node node, Node neighbor)
        {
            if (node.Neighbors == null)
                node.Neighbors = new List<Neighbor>();

            if (neighbor.Neighbors == null)
                neighbor.Neighbors = new List<Neighbor>();

            float distance = UIUtils.getDirectDistance(UIUtils.getXDistance(node.Longitude), UIUtils.getZDistance(node.Latitude), UIUtils.getXDistance(neighbor.Longitude), UIUtils.getZDistance(neighbor.Latitude));
            node.Neighbors.Add(new Neighbor() { Node = neighbor, Distance = distance });
            neighbor.Neighbors.Add(new Neighbor() { Node = node, Distance = distance });
        }

        public void AddNeighbor(String node, String neighbor)
        {
            bool nodeFound = false, neighborFound = false;
            Node A = new Node(), B = new Node();

            foreach (Node nodeInList in Nodes)
            {
                if (!nodeFound && nodeInList.Name.Equals(node))
                {
                    A = nodeInList;
                    nodeFound = !nodeFound;
                }
                if (!neighborFound && nodeInList.Name.Equals(neighbor))
                {
                    B = nodeInList;
                    neighborFound = !neighborFound;
                }
                if (nodeFound && neighborFound)
                {
                    break;
                }
            }

            if (A.Neighbors == null)
                A.Neighbors = new List<Neighbor>();

            if (B.Neighbors == null)
                B.Neighbors = new List<Neighbor>();

            float distance = UIUtils.getDirectDistance(UIUtils.getXDistance(A.Longitude), UIUtils.getZDistance(A.Latitude), UIUtils.getXDistance(B.Longitude), UIUtils.getZDistance(B.Latitude));
            A.Neighbors.Add(new Neighbor() { Node = B, Distance = distance });
            B.Neighbors.Add(new Neighbor() { Node = A, Distance = distance });
        }

        public List<PathData> Dijkstra(String startName, String destinationName)
        {
            bool startNodeFound = false, endNodeFound = false;
            List<Node> NodesDijkstra = new List<Node>();
            Node start = new Node(), destination = new Node();

            foreach (Node nodeInList in this.Nodes)
            {
                if (nodeInList.Active)
                {
                    Node node = new Node(nodeInList);
                
                    if (!startNodeFound && node.Name.Equals(startName))
                    {
                        node.DijkstraDistance = 0;
                        start = node;
                        startNodeFound = !startNodeFound;
                    }
                    else
                    {
                        node.DijkstraDistance = Int32.MaxValue;
                    }
                    if (!endNodeFound && node.Name.Equals(destinationName))
                    {
                        destination = node;
                        endNodeFound = !endNodeFound;
                    }
                    NodesDijkstra.Add(node);
                }
            }

            foreach (Node node in NodesDijkstra)
                foreach (Neighbor neighbor in node.Neighbors)
                    foreach (Node nodeToFind in NodesDijkstra)
                        if (neighbor.Node.Name == nodeToFind.Name) 
                        { 
                            neighbor.Node = nodeToFind;
                            break;
                        }
            
            float distancePathed;
            start.DijkstraPath = new DijkstraPath(start.Name);

            while (NodesDijkstra.Count > 0)
            {
                Node minNode = (from node in NodesDijkstra orderby node.DijkstraDistance select node).First();

                NodesDijkstra.Remove(minNode);
                minNode.Visited = true;

                foreach (Neighbor neighbor in minNode.Neighbors)
                {
                    if (!neighbor.Node.Visited && neighbor.Node.Active)
                    {
                        distancePathed = minNode.DijkstraDistance + neighbor.Distance;
                        if (distancePathed < neighbor.Node.DijkstraDistance)
                        {
                            neighbor.Node.DijkstraDistance = distancePathed;
                            neighbor.Node.DijkstraPath = new DijkstraPath(
                                minNode.DijkstraPath.Nodes + "|" + neighbor.Distance + "|" + distancePathed + "," + neighbor.Node.Name
                            );
                        }
                    }
                }
            }
            
            return PathToNodeList(destination.DijkstraPath);
        }

        public List<PathData> PathToNodeList(DijkstraPath destinationPath)
        {
            List<PathData> nodeList = new List<PathData>();

            String[] nodosPath = destinationPath.Nodes.Split(',');

            for (int i = 0; i < nodosPath.Length; ++i)
            {
                if (i + 1 == nodosPath.Length)
                    continue;

                String[] nodeInPath = nodosPath[i].Split('|');
                nodeList.Add(new PathData () {
                    StartNode = Nodes.Find(x => x.Name == nodeInPath[0]),
                    DistanceToNeighbor = Convert.ToSingle(nodeInPath[1]),
                    DistancePathed = Convert.ToSingle(nodeInPath[2]),
                    EndNode = Nodes.Find(x => x.Name == nodosPath[i+1].Split('|')[0])
                });
            }

            return nodeList;
        }

        #endregion
    }
}
