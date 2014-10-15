using SncPucmm.Controller.GUI;
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
            //Buscar las direcciones
            findDirections(ref Nodes);

            //Mostrar el menu de direciones
            GUIMenuDirection menuDirection = new GUIMenuDirection("GUIMenuDirection", Nodes);
            MenuManager.GetInstance().AddMenu(menuDirection);
        }

        private static void findDirections(ref List<PathData> Nodes)
        {
            for (int i = 0; i < Nodes.Count; ++i)
            {
                if (i + 1 != Nodes.Count)
                {
                    float adjacentA = UIUtils.getXDistance(Nodes[i].StartNode.Longitude) - UIUtils.getXDistance(Nodes[i].EndNode.Longitude);
                    float oppositeA = UIUtils.getZDistance(Nodes[i].StartNode.Latitude) - UIUtils.getZDistance(Nodes[i].EndNode.Latitude);
                    float distanceA = Mathf.Sqrt(Mathf.Pow(adjacentA, 2) + Mathf.Pow(oppositeA, 2));

                    float adjacentB = UIUtils.getXDistance(Nodes[i + 1].StartNode.Longitude) - UIUtils.getXDistance(Nodes[i + 1].EndNode.Longitude);
                    float oppositeB = UIUtils.getZDistance(Nodes[i + 1].StartNode.Latitude) - UIUtils.getZDistance(Nodes[i + 1].EndNode.Latitude);
                    float distanceB = Mathf.Sqrt(Mathf.Pow(adjacentB, 2) + Mathf.Pow(oppositeB, 2));

                    float adjacentC = UIUtils.getXDistance(Nodes[i + 1].EndNode.Longitude) - UIUtils.getXDistance(Nodes[i].StartNode.Longitude);
                    float oppositeC = UIUtils.getZDistance(Nodes[i + 1].EndNode.Latitude) - UIUtils.getZDistance(Nodes[i].StartNode.Latitude);
                    float distanceC = Mathf.Sqrt(Mathf.Pow(adjacentC, 2) + Mathf.Pow(oppositeC, 2));

                    //Teorema del Coseno
                    float operation;

                    operation = (Mathf.Pow(distanceA, 2) + Mathf.Pow(distanceB, 2) - Mathf.Pow(distanceC, 2)) / (2 * distanceA * distanceB);
                    float degreeAB = Mathf.Acos(operation) * 180 / Mathf.PI;

                    //operation = (Mathf.Pow(distanceB, 2) + Mathf.Pow(distanceC, 2) - Mathf.Pow(distanceA, 2)) / (2 * distanceB * distanceC);
                    //float degreeBC = Mathf.Acos(operation) * 180 / Mathf.PI;

                    //operation = (Mathf.Pow(distanceC, 2) + Mathf.Pow(distanceA, 2) - Mathf.Pow(distanceB, 2)) / (2 * distanceC * distanceA);
                    //float degreeCA = Mathf.Acos(operation) * 180 / Mathf.PI;

                    bool isStraight = true;

                    if (degreeAB >= 0f && degreeAB < 140f)
                    {
                        //hacia el sur
                        if (oppositeA > 0)
                        {
                            //hacia el oeste
                            if (adjacentC > 0)
                            {
                                Nodes[i + 1].Direction = eDirection.Left;
                            }
                            //hacia el este
                            else if(adjacentC < 0)
                            {
                                Nodes[i + 1].Direction = eDirection.Right;
                            }
                        }
                        //hacia el norte
                        else if (oppositeA < 0)
                        {
                            //hacia el oeste
                            if (adjacentC >= 0)
                            {
                                Nodes[i + 1].Direction = eDirection.Right;
                            }
                            //hacia el este
                            else
                            {
                                Nodes[i + 1].Direction = eDirection.Left;
                            }
                        }

                        isStraight = false;
                    }
                    else if (degreeAB >= 140f && degreeAB < 165f)
                    {
                        //hacia el sur
                        if (oppositeA >= 0)
                        {
                            //hacia el oeste
                            if (adjacentC >= 0)
                            {
                                Nodes[i + 1].Direction = eDirection.SlightRight;
                            }
                            //hacia el este
                            else
                            {
                                Nodes[i + 1].Direction = eDirection.SlightLeft;
                            }
                        }
                        //hacia el norte
                        else
                        {
                            //hacia el oeste
                            if (adjacentC >= 0)
                            {
                                Nodes[i + 1].Direction = eDirection.SlightLeft;
                            }
                            //hacia el este
                            else
                            {
                                Nodes[i + 1].Direction = eDirection.SlightRight;
                            }
                        }

                        isStraight = false;
                    }

                    if(isStraight)
                    {
                        Nodes[i + 1].Direction = eDirection.Straight;
                    }
                }
            }
        }

        private static float GetPreviousDegreeFromUserCurrentPosition(Node node)
        {
            float adjacent = UIUtils.getXDistance(UIGPS.Longitude) - UIUtils.getXDistance(node.Longitude); //x1 - x2
            float opposite = UIUtils.getZDistance(UIGPS.Latitude) - UIUtils.getZDistance(node.Latitude);   //y1 - y2

            float hypotenuse = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));

            float degree = Mathf.Asin(opposite / hypotenuse) * 180 / Mathf.PI;

            return GetSinDegree(degree, adjacent);
        }

        private static float GetSinDegree(float degree, float adjacent)
        {
            if (adjacent >= 0)
            {
                degree = Mathf.Abs(90 - degree);
            }
            else
            {
                degree = Mathf.Abs(degree - 90);
            }

            return degree;
        }
    }
}
