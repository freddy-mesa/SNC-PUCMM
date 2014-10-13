using SncPucmm.Model.Navigation;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.Direction
{
    class DirectionController
    {
        public static void StartDirection(List<PathData> Nodes)
        {
            findNodeDirectionByOrientation(ref Nodes);
            var directions = (UIDirections)(GameObject.Find("/GUI")).GetComponent<UIDirections>();
            directions.PrintDirections(Nodes);
        }

        private static void findNodeDirectionByOrientation(ref List<PathData> Nodes)
        {
            for (int i = 0; i < Nodes.Count; ++i)
            {
                float nodeA_PosX = UIUtils.getXDistance(Nodes[i].StartNode.Longitude);
                float nodeA_PosZ = UIUtils.getZDistance(Nodes[i].StartNode.Latitude);

                float nodeB_PosX = UIUtils.getXDistance(Nodes[i].EndNode.Longitude);
                float nodeB_PosZ = UIUtils.getZDistance(Nodes[i].EndNode.Latitude);

                float adjacent = nodeA_PosX - nodeB_PosX;
                float opposite = nodeA_PosZ - nodeB_PosZ;
                

                float hypotenuse = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));
                float nodesDegree = Mathf.Asin(opposite / hypotenuse) * 180 / Mathf.PI;

                if (adjacent >= 0)
                {
                    nodesDegree = 180 - nodesDegree;
                }

                if (nodesDegree >= 0 && nodesDegree < 50 || nodesDegree >= 345 && nodesDegree <= 360)
                {
                    Nodes[i].Direction = eDirection.Right;
                }
                else if (nodesDegree >= 50 && nodesDegree < 75)
                {
                    Nodes[i].Direction = eDirection.SlightRight;
                }
                else if (nodesDegree >= 105 && nodesDegree < 130)
                {
                    Nodes[i].Direction = eDirection.SlightLeft;
                }
                else if (nodesDegree >= 130 && nodesDegree < 235)
                {
                    Nodes[i].Direction = eDirection.Left;
                }
                else
                {
                    //(degrees >= 75 && degrees < 105) -> Straight
                    Nodes[i].Direction = eDirection.Straight;
                }
            }

            for (int i = 0; i < Nodes.Count; ++i)
            {
                Debug.Log("Next " + Nodes[i].Direction.ToString() + " from " + Nodes[i].StartNode.Name + " to " + Nodes[i].EndNode.Name);
            }

            //for (int i = 0; i < Nodes.Length; ++i)
            //{
            //    float nodeA_PosX = UIUtils.getXDistance(Nodes[i].StartNode.Longitude);
            //    float nodeA_PosZ = UIUtils.getZDistance(Nodes[i].StartNode.Latitude);

            //    float nodeB_PosX = UIUtils.getXDistance(Nodes[i].EndNode.Longitude);
            //    float nodeB_PosZ = UIUtils.getZDistance(Nodes[i].EndNode.Latitude);

            //    float adjacent = nodeA_PosX - nodeB_PosX;
            //    float opposite = nodeA_PosZ - nodeB_PosZ;


            //    float hypotenuse = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));
            //    float degrees = Mathf.Asin(opposite / hypotenuse) * 180 / Mathf.PI;
            //    if (adjacent >= 0)
            //    {
            //        degrees = 180 - degrees;
            //    }


            //    if (degrees >= 0 && degrees < 50 || degrees >= 345 && degrees <= 360)
            //    {
            //        Nodes[i].Direction = eDirection.Right;
            //    }
            //    else if (degrees >= 50 && degrees < 75)
            //    {
            //        Nodes[i].Direction = eDirection.SlightRight;
            //    }
            //    else if (degrees >= 105 && degrees < 130)
            //    {
            //        Nodes[i].Direction = eDirection.SlightLeft;
            //    }
            //    else if (degrees >= 130 && degrees < 235)
            //    {
            //        Nodes[i].Direction = eDirection.Left;
            //    }
            //    else
            //    {
            //        //(degrees >= 75 && degrees < 105) -> Straight
            //        Nodes[i].Direction = eDirection.Straight;
            //    }
            //}
        }

        private static float GetPreviousDegreeFromCurrentPosition(PathData node)
        {
            float currentUserPosX = UIUtils.getXDistance(UIGPS.Longitude);
            float currentUserPosZ = UIUtils.getZDistance(UIGPS.Latitude);

            float node_PosX = UIUtils.getXDistance(node.StartNode.Longitude);
            float node_PosZ = UIUtils.getZDistance(node.StartNode.Latitude);

            float adjacent = currentUserPosX - node_PosX;
            float opposite = currentUserPosZ - node_PosZ;

            float hypotenuse = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));

            float alfaDegree = Mathf.Acos(adjacent / hypotenuse) * 180 / Mathf.PI;
            if (opposite >= 0)
            {
                alfaDegree = 180 - alfaDegree;
            }

            return alfaDegree;
        }
    }
}
