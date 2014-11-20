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

            float nodePosX = 0, nodePosZ = 0, neighborPosX = 0, neighborPosZ = 0;
            
            if (node.IsInsideBuilding)
            {
                nodePosX = node.Longitude;
                nodePosZ = node.Latitude;
            }
            else
            {
                nodePosX = UIUtils.getXDistance(node.Longitude);
                nodePosZ = UIUtils.getZDistance(node.Latitude);
            }

            if(neighbor.IsInsideBuilding)
            {
                neighborPosX = neighbor.Longitude;
                neighborPosZ = neighbor.Latitude;
            }
            else 
            {
                neighborPosX = UIUtils.getXDistance(neighbor.Longitude);
                neighborPosZ = UIUtils.getZDistance(neighbor.Latitude);
            }

            float distance = UIUtils.getDirectDistance( nodePosX, nodePosZ, neighborPosX, neighborPosZ);

            node.Neighbors.Add(new NeighborDijkstra() { Node = neighbor, Distance = distance });
            neighbor.Neighbors.Add(new NeighborDijkstra() { Node = node, Distance = distance });
        }

        public void AddNeighbor(int idNode, int idNodeneighbor)
        {
            bool nodeFound = false, neighborFound = false;
            NodeDijkstra node = new NodeDijkstra(), neighbor = new NodeDijkstra();

            foreach (NodeDijkstra nodeInList in Nodes)
            {
                if (!nodeFound && nodeInList.IdNode.Equals(idNode))
                {
                    node = nodeInList;
                    nodeFound = !nodeFound;
                }
                if (!neighborFound && nodeInList.IdNode.Equals(idNodeneighbor))
                {
                    neighbor = nodeInList;
                    neighborFound = !neighborFound;
                }

                if (nodeFound && neighborFound)
                {
                    break;
                }
            }

            if (node.Neighbors == null)
                node.Neighbors = new List<NeighborDijkstra>();

            if (neighbor.Neighbors == null)
                neighbor.Neighbors = new List<NeighborDijkstra>();

            float nodePosX = 0, nodePosZ = 0, neighborPosX = 0, neighborPosZ = 0;

            if (node.IsInsideBuilding)
            {
                nodePosX = node.Longitude;
                nodePosZ = node.Latitude;
            }
            else
            {
                nodePosX = UIUtils.getXDistance(node.Longitude);
                nodePosZ = UIUtils.getZDistance(node.Latitude);
            }

            if (neighbor.IsInsideBuilding)
            {
                neighborPosX = neighbor.Longitude;
                neighborPosZ = neighbor.Latitude;
            }
            else
            {
                neighborPosX = UIUtils.getXDistance(neighbor.Longitude);
                neighborPosZ = UIUtils.getZDistance(neighbor.Latitude);
            }

            float distance = UIUtils.getDirectDistance(nodePosX, nodePosZ, neighborPosX, neighborPosZ);

            node.Neighbors.Add(new NeighborDijkstra() { Node = neighbor, Distance = distance });
            neighbor.Neighbors.Add(new NeighborDijkstra() { Node = node, Distance = distance });
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
                if (node.Neighbors != null)
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
            }

            float distancePathed;
            start.DijkstraPath = new PathDijkstra(start.Name);

            while (NodesDijkstra.Count > 0)
            {
                NodeDijkstra minNode = (from node in NodesDijkstra orderby node.DijkstraDistance select node).First();

                NodesDijkstra.Remove(minNode);
                minNode.Visited = true;

                if (minNode.Neighbors != null)
                {
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

            var nodeToDelete = new List<PathDataDijkstra>();

            foreach (var node in nodeList)
            {
                if (node.StartNode.IsInsideBuilding)
                {
                    node.IsInsideBuilding = true;
                }

                if (node.StartNode.IsBuilding)
                {
                    if (IsNodeInsideBuilding(node.StartNode.BuildingName, nodeList))
                    {
                        nodeToDelete.Add(node);
                    }
                }
            }

            foreach (var node in nodeToDelete)
            {
                nodeList.Remove(node);
            }

            return nodeList;
        }

        private bool IsNodeInsideBuilding(string buildingName, List<PathDataDijkstra> nodeList)
        {
            bool exist = false;

            foreach (var node in nodeList)
            {
                if (node.StartNode.IsInsideBuilding && node.StartNode.BuildingName == buildingName)
                {
                    exist = true;
                    break;
                }
            }

            return exist;
        }

        #endregion
    }
}