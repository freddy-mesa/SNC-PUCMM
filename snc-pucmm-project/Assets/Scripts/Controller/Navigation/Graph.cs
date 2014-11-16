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

        public List<NodeDijkstra> Nodes { get; set; }

        #endregion

        #region Constructor

        public Graph()
        {
            Nodes = new List<NodeDijkstra>();
        }

        #endregion

        #region Metodos

        public void AddNeighbor(NodeDijkstra node, NodeDijkstra neighbor)
        {
            if (node.Neighbors == null)
                node.Neighbors = new List<NeighborDijkstra>();

            if (neighbor.Neighbors == null)
                neighbor.Neighbors = new List<NeighborDijkstra>();

            float distance = UIUtils.getDirectDistance(UIUtils.getXDistance(node.Longitude), UIUtils.getZDistance(node.Latitude), UIUtils.getXDistance(neighbor.Longitude), UIUtils.getZDistance(neighbor.Latitude));
            node.Neighbors.Add(new NeighborDijkstra() { Node = neighbor, Distance = distance });
            neighbor.Neighbors.Add(new NeighborDijkstra() { Node = node, Distance = distance });
        }

        public void AddNeighbor(int idNode, int idNodeneighbor)
        {
            bool nodeFound = false, neighborFound = false;
            NodeDijkstra A = new NodeDijkstra(), B = new NodeDijkstra();

            foreach (NodeDijkstra nodeInList in Nodes)
            {
                if (!nodeFound && nodeInList.IdNode.Equals(idNode))
                {
                    A = nodeInList;
                    nodeFound = !nodeFound;
                }
                if (!neighborFound && nodeInList.IdNode.Equals(idNodeneighbor))
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
                A.Neighbors = new List<NeighborDijkstra>();

            if (B.Neighbors == null)
                B.Neighbors = new List<NeighborDijkstra>();

            float distance = UIUtils.getDirectDistance(UIUtils.getXDistance(A.Longitude), UIUtils.getZDistance(A.Latitude), UIUtils.getXDistance(B.Longitude), UIUtils.getZDistance(B.Latitude));
            A.Neighbors.Add(new NeighborDijkstra() { Node = B, Distance = distance });
            B.Neighbors.Add(new NeighborDijkstra() { Node = A, Distance = distance });
        }

        public List<PathDataDijkstra> Dijkstra(int startIdNode, int destinationIdNode)
        {
            bool startNodeFound = false, endNodeFound = false;
            List<NodeDijkstra> NodesDijkstra = new List<NodeDijkstra>();
            NodeDijkstra start = new NodeDijkstra(), destination = new NodeDijkstra();

            foreach (NodeDijkstra nodeInList in this.Nodes)
            {
                if (nodeInList.Active)
                {
                    NodeDijkstra node = new NodeDijkstra(nodeInList);

                    if (!startNodeFound && node.IdNode.Equals(startIdNode))
                    {
                        node.DijkstraDistance = 0;
                        start = node;
                        startNodeFound = !startNodeFound;
                    }
                    else
                    {
                        node.DijkstraDistance = Int32.MaxValue;
                    }
                    if (!endNodeFound && node.IdNode.Equals(destinationIdNode))
                    {
                        destination = node;
                        endNodeFound = !endNodeFound;
                    }

                    NodesDijkstra.Add(node);
                }
            }

            return Dijkstra(ref NodesDijkstra, start, destination);
        }

        public List<PathDataDijkstra> Dijkstra(String startName, String destinationName)
        {
            bool startNodeFound = false, endNodeFound = false;
            List<NodeDijkstra> NodesDijkstra = new List<NodeDijkstra>();
            NodeDijkstra start = new NodeDijkstra(), destination = new NodeDijkstra();

            foreach (NodeDijkstra nodeInList in this.Nodes)
            {
                if (nodeInList.Active)
                {
                    NodeDijkstra node = new NodeDijkstra(nodeInList);

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

            return Dijkstra(ref NodesDijkstra, start, destination);
        }

        private List<PathDataDijkstra> Dijkstra(ref List<NodeDijkstra> NodesDijkstra, NodeDijkstra start, NodeDijkstra destination)
        {
            foreach (NodeDijkstra node in NodesDijkstra)
            {
                foreach (NeighborDijkstra neighbor in node.Neighbors)
                {
                    foreach (NodeDijkstra nodeToFind in NodesDijkstra)
                    {
                        if (neighbor.Node.Name == nodeToFind.Name)
                        {
                            neighbor.Node = nodeToFind;
                            break;
                        }
                    }
                }
            }

            float distancePathed;
            start.DijkstraPath = new PathDijkstra(start.Name);

            while (NodesDijkstra.Count > 0)
            {
                NodeDijkstra minNode = (from node in NodesDijkstra orderby node.DijkstraDistance select node).First();

                NodesDijkstra.Remove(minNode);
                minNode.Visited = true;

                foreach (NeighborDijkstra neighbor in minNode.Neighbors)
                {
                    if (!neighbor.Node.Visited && neighbor.Node.Active)
                    {
                        distancePathed = minNode.DijkstraDistance + neighbor.Distance;
                        if (distancePathed < neighbor.Node.DijkstraDistance)
                        {
                            neighbor.Node.DijkstraDistance = distancePathed;
                            neighbor.Node.DijkstraPath = new PathDijkstra(
                                minNode.DijkstraPath.Nodes + "|" + neighbor.Distance + "|" + distancePathed + "," + neighbor.Node.Name
                            );
                        }
                    }
                }
            }

            return PathToNodeList(destination.DijkstraPath);
        }

        public List<PathDataDijkstra> PathToNodeList(PathDijkstra destinationPath)
        {
            List<PathDataDijkstra> nodeList = new List<PathDataDijkstra>();

            String[] nodosPath = destinationPath.Nodes.Split(',');

            for (int i = 0; i < nodosPath.Length; ++i)
            {
                if (i + 1 == nodosPath.Length)
                    continue;

                String[] nodeInPath = nodosPath[i].Split('|');
                String endNodePath = nodosPath[i + 1].Split('|')[0];
                nodeList.Add(new PathDataDijkstra()
                {
                    StartNode = Nodes.Find(x => x.Name == nodeInPath[0]),
                    DistanceToNeighbor = Convert.ToSingle(nodeInPath[1]),
                    DistancePathed = Convert.ToSingle(nodeInPath[2]),
                    EndNode = Nodes.Find(x => x.Name == endNodePath)
                });
            }

            return nodeList;
        }

        #endregion
    }
}