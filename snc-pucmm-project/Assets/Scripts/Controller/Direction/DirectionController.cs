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
        public static void findNodeDirectionByOrientation(List<PathData> Nodes)
        {
            for (int i = 0; i < Nodes.Count; ++i)
            {
                if (i + 1 != Nodes.Count)
                {
                    float nodeA_PosX = UIUtils.getXDistance(Nodes[i].StartNode.Longitude);
                    float nodeA_PosZ = UIUtils.getZDistance(Nodes[i].StartNode.Latitude);

                    float nodeC_PosX = UIUtils.getXDistance(Nodes[i + 1].EndNode.Longitude);
                    float nodeC_PosZ = UIUtils.getZDistance(Nodes[i + 1].EndNode.Latitude);

                    float adjacent = nodeA_PosX - nodeC_PosX;
                    float opposite = nodeA_PosZ - nodeC_PosZ;


                    float hypotenuse = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));
                    float degrees = Mathf.Asin(opposite / hypotenuse) * 180 / Mathf.PI;
                
                    if (adjacent >= 0)
                    {
                        degrees = 180 - degrees;
                    }

                    if (degrees >= 0 && degrees < 80 || degrees >= 270 && degrees <= 360)
                    {
                        //Doblar a la derecha
                        Nodes[i + 1].Direction = eDirection.Right;                        
                    }
                    else if (degrees >= 100 && degrees < 270)
                    {
                        //Doblar a la izquierda
                        Nodes[i + 1].Direction = eDirection.Left;
                    }
                    else
                    {
                        //(degrees >= 80 && degrees < 100) -> Straight
                        Nodes[i + 1].Direction = eDirection.Straight;
                    }
                }
            }
        }
    }
}
