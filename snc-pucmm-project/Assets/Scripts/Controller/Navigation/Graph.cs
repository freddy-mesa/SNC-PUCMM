using SncPucmm.Model.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Navigation
{
    public class Graph
    {
        #region Propiedades

        public List<SectionNodes> SectionNodesList { get; set; }
        
        #endregion

        #region Constructor

        public Graph()
        {
            SectionNodesList = new List<SectionNodes>();
        }

        #endregion

        #region Metodos

        public void AddNeighbor(Node node, Node neighbor, float distance)
        {
            if (node.Neighbors == null)
                node.Neighbors = new List<Neighbor>();

            if (neighbor.Neighbors == null)
                neighbor.Neighbors = new List<Neighbor>();

            node.Neighbors.Add(new Neighbor() { Node = neighbor, Distance = distance });
            neighbor.Neighbors.Add(new Neighbor() { Node = node, Distance = distance });
        }

        public void AddNeighbor(String node, String neighbor, float distance)
        {
            bool nodeFound = false, neighborFound = false;
            Node A = new Node(), B = new Node();

            foreach (SectionNodes section in SectionNodesList)
            {
                foreach (var nodeInList in section.NodeList)
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

            A.Neighbors.Add(new Neighbor() { Node = B, Distance = distance });
            B.Neighbors.Add(new Neighbor() { Node = A, Distance = distance });
        }

        public List<PathData> Dijkstra(String startName, String destinationName)
        {
            bool startNodeFound = false, endNodeFound = false;
            List<Node> Nodes = new List<Node>();
            Node start = new Node(), destination = new Node();

            foreach (SectionNodes section in SectionNodesList)
            {
                foreach (var nodeInList in section.NodeList)
                {
                    Node node = new Node(nodeInList);

                    node.DijkstraDistance = Int32.MaxValue;
                    Nodes.Add(new Node(node));

                    if (!startNodeFound && node.Name.Equals(startName))
                    {
                        start = node;
                        start.DijkstraDistance = 0;
                        startNodeFound = !startNodeFound;
                    }
                    if (!endNodeFound && node.Name.Equals(destinationName))
                    {
                        destination = node;
                        endNodeFound = !endNodeFound;
                    }
                }
            }

            float distancePathed;
            start.DijkstraPath = new DijkstraPath(start.Name);

            while (Nodes.Count > 0)
            {
                Node minNode = (from node in Nodes orderby node.DijkstraDistance select node).First();

                Nodes.Remove(minNode);
                minNode.Visited = true;

                foreach (Neighbor neighbor in minNode.Neighbors)
                {
                    if (!neighbor.Node.Visited)
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
            
            return PathToNodeList(destination.DijkstraPath, Nodes);
        }

        public List<PathData> PathToNodeList(DijkstraPath destinationPath, List<Node> Nodes)
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
                    EndNode = Nodes.Find(x => x.Name == nodosPath[i + 1])
                });
            }

            return nodeList;
        }

        #endregion
    }
}
