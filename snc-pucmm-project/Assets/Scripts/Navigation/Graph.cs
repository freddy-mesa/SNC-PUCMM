using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Navigation
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

        public void AddNeighbor(Node node, Node neighbor, int distance)
        {
            if (node.Neighbors == null)
                node.Neighbors = new List<Neighbor>();

            if (neighbor.Neighbors == null)
                neighbor.Neighbors = new List<Neighbor>();

            node.Neighbors.Add(new Neighbor() { Node = neighbor, Distance = distance });
            neighbor.Neighbors.Add(new Neighbor() { Node = node, Distance = distance });
        }

        public void AddNeighbor(String node, String neighbor, int distance)
        {
            Node A = Nodes.Find(n => n.Name == node);
            Node B = Nodes.Find(n => n.Name == neighbor);

            if (A.Neighbors == null)
                A.Neighbors = new List<Neighbor>();

            if (B.Neighbors == null)
                B.Neighbors = new List<Neighbor>();
            A.Neighbors.Add(new Neighbor() { Node = B, Distance = distance });
            B.Neighbors.Add(new Neighbor() { Node = A, Distance = distance });
        }

        public Path Dijkstra(String startName, String destinationName)
        {
            Node start = Nodes.Find(n => n.Name == startName);
            Node destination = Nodes.Find(n => n.Name == destinationName);
            List<Node> nodes = new List<Node>();
            int dist;
            start.Distance = 0;

            foreach (Node node in Nodes)
            {
                node.Path = new Path();
                if (node != start)
                    node.Distance = Int32.MaxValue;
                nodes.Add(node);
            }
            start.Path = new Path(start.Name, start.Distance);

            while (nodes.Count > 0)
            {
                Node minNode = (from node in nodes
                                orderby node.Distance
                                select node).First();

                nodes.Remove(minNode);
                minNode.Visited = true;

                foreach (Neighbor neighbor in minNode.Neighbors)
                {
                    if (!neighbor.Node.Visited)
                    {
                        dist = minNode.Distance + neighbor.Distance;
                        if (dist < neighbor.Node.Distance)
                        {
                            neighbor.Node.Distance = dist;
                            neighbor.Node.Path = new Path(minNode.Path.Nodes + " -> " + neighbor.Node.Name, dist);
                        }
                    }
                }
            }
            return destination.Path;
        }

        #endregion
    }
}
