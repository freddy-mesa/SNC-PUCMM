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

            State.ChangeState(eState.MenuDirection);

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

                    float adjacentC = UIUtils.getXDistance(Nodes[i].StartNode.Longitude) - UIUtils.getXDistance(Nodes[i + 1].EndNode.Longitude);
                    float oppositeC = UIUtils.getZDistance(Nodes[i].StartNode.Latitude) - UIUtils.getZDistance(Nodes[i + 1].EndNode.Latitude);
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
                        //Verticalmente Recto
                        if (adjacentA >= -0.5 && adjacentA <= 0.5)
                        {
                            //hacia el sur
                            if (oppositeA > 0)
                            {
                                //hacia el oeste
                                if (adjacentC > 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.Right;
                                }
                                //hacia el este
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.Left;
                                }
                            }
                            //Hacia el norte
                            else
                            {
                                //hacia el oeste
                                if (adjacentC > 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.Left;
                                }
                                //hacia el este
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.Right;
                                }
                            }
                        }
                        //Horizontalmente Recto
                        else if (oppositeA >= -0.5 && oppositeA <= 0.5)
                        {
                            //hacia el este
                            if (adjacentA > 0)
                            {
                                //hacia el norte
                                if (adjacentC > 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.Left;
                                }
                                //hacia el sur
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.Right;
                                }
                            }
                            //hacia el oeste
                            else
                            {
                                //hacia el norte
                                if (adjacentC > 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.Right;
                                }
                                //hacia el sur
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.Left;
                                }
                            }
                        }
                        //hacia el sur
                        else if (oppositeA > 0)
                        {
                            //hacia el oeste
                            if (oppositeB <= 0)
                            {
                                if(adjacentC >= 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.Left;
                                }
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.Right;
                                }
                            }
                            //hacia el este
                            else
                            {
                                if (adjacentC > 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.Right;
                                }
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.Left;
                                }
                            }
                        }
                        //hacia el norte
                        else if (oppositeA < 0)
                        {
                            //Hacia el arriba y izquierda
                            if (oppositeB <= 0)
                            {
                                if (adjacentB <= 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.Left;
                                }
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.Right;
                                }
                            }
                            //hacia abajo y derecha
                            else
                            {
                                if (adjacentB <= 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.Right;
                                }
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.Left;
                                }
                            }
                        }

                        isStraight = false;
                    }
                    else if (degreeAB >= 140f && degreeAB < 165f)
                    {
                        //Verticalmente Recto
                        if (adjacentA >= -0.5 && adjacentA <= 0.5)
                        {
                            //hacia el sur
                            if (oppositeA > 0)
                            {
                                //hacia el oeste
                                if (adjacentC > 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.SlightRight;
                                }
                                //hacia el este
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.SlightLeft;
                                }
                            }
                            //Hacia el norte
                            else
                            {
                                //hacia el oeste
                                if (adjacentC > 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.SlightLeft;
                                }
                                //hacia el este
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.SlightRight;
                                }
                            }
                        }
                        //Horizontalmente Recto
                        else if (oppositeA >= -0.5 && oppositeA <= 0.5)
                        {
                            //hacia el este
                            if (adjacentA > 0)
                            {
                                //hacia el norte
                                if (adjacentC > 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.SlightLeft;
                                }
                                //hacia el sur
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.SlightRight;
                                }
                            }
                            //hacia el oeste
                            else
                            {
                                //hacia el norte
                                if (adjacentC > 0)
                                {
                                    Nodes[i + 1].Direction = eDirection.SlightRight;
                                }
                                //hacia el sur
                                else
                                {
                                    Nodes[i + 1].Direction = eDirection.SlightLeft;
                                }
                            }
                        }
                        //hacia el sur
                        if (oppositeA > 0)
                        {
                            //hacia el oeste
                            if (adjacentC > 0)
                            {
                                Nodes[i + 1].Direction = eDirection.SlightLeft;
                            }
                            //hacia el este
                            else
                            {
                                Nodes[i + 1].Direction = eDirection.SlightRight;
                            }
                        }
                        //hacia el norte
                        else
                        {
                            //hacia el oeste
                            if (adjacentC > 0)
                            {
                                Nodes[i + 1].Direction = eDirection.SlightRight;
                            }
                            //hacia el este
                            else
                            {
                                Nodes[i + 1].Direction = eDirection.SlightLeft;
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

        private static float GetDistanceFromUserCurrentPosition(Node node)
        {
            float adjacent = UIUtils.getXDistance(UIGPS.Longitude) - UIUtils.getXDistance(node.Longitude); //x1 - x2
            float opposite = UIUtils.getZDistance(UIGPS.Latitude) - UIUtils.getZDistance(node.Latitude);   //y1 - y2

            return Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));
        }
    }
}
