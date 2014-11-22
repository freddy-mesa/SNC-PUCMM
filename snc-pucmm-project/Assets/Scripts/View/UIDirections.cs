using SncPucmm.Model.Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.View
{
    public class UIDirections : MonoBehaviour
    {
        public Transform PrefabCamino;
        public Transform PrefabCaminoSelected;
        public Material MaterialPrefabCamino;
        public Material MaterialPrefabCaminoSelected;

        public void PrintPath(List<PathDataDijkstra> pathList, PathDataDijkstra selected)
        {
            DestroyChilds();

            foreach (PathDataDijkstra path in pathList)
            {
                Transform prefabPlane = null;

                if (path.IsInsideBuilding)
                {
                    if (selected.IsInsideBuilding && path.StartNode.PlantaBuilding == selected.StartNode.PlantaBuilding)
                    {
                        var strPath = "/PUCMM/Model3D/" + path.StartNode.BuildingName + "/Caminos/Planta" + path.StartNode.PlantaBuilding;
                        var camino = UIUtils.Find(strPath + "/" + path.StartNode.Name).transform;

                        prefabPlane = Instantiate(camino, camino.position, camino.rotation) as Transform;
                        prefabPlane.name = camino.name;

                        prefabPlane.parent = this.transform;

                        if (path.Equals(selected))
                        {
                            prefabPlane.Translate(new Vector3(0, 0.2f, 0), Space.Self);
                            var plane = prefabPlane.FindChild("Plane");
                            plane.GetComponent<MeshRenderer>().material = MaterialPrefabCaminoSelected;
                        }
                        else
                        {
                            prefabPlane.Translate(new Vector3(0, 0.1f, 0), Space.Self);
                            var plane = prefabPlane.FindChild("Plane");
                            plane.GetComponent<MeshRenderer>().material = MaterialPrefabCamino;
                        }
                    }
                }
                else
                {
                    float nodeStartPosX = 0, nodeStartPosZ = 0, nodeEndPosX = 0, nodeEndPosZ = 0;

                    if (path.StartNode.IsInsideBuilding)
                    {
                        nodeStartPosX = path.StartNode.Longitude;
                        nodeStartPosZ = path.StartNode.Latitude;
                    }
                    else
                    {
                        nodeStartPosX = UIUtils.getXDistance(path.StartNode.Longitude);
                        nodeStartPosZ = UIUtils.getZDistance(path.StartNode.Latitude);
                    }

                    if (path.EndNode.IsInsideBuilding)
                    {
                        nodeEndPosX = path.EndNode.Longitude;
                        nodeEndPosZ = path.EndNode.Latitude;
                    }
                    else
                    {
                        nodeEndPosX = UIUtils.getXDistance(path.EndNode.Longitude);
                        nodeEndPosZ = UIUtils.getZDistance(path.EndNode.Latitude);
                    }

                    float adjacent = nodeStartPosX - nodeEndPosX;
                    float opposite = nodeStartPosZ - nodeEndPosZ;

                    float hypotenuse = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));
                    float degrees = Mathf.Asin(opposite / hypotenuse) * 180 / Mathf.PI;

                    if (adjacent >= 0)
                    {
                        degrees = 180 - degrees;
                    }

                    Quaternion rotation = Quaternion.AngleAxis(degrees, Vector3.up);
                    Vector3 position = new Vector3(nodeStartPosX, 0.6f, nodeStartPosZ);

                    if (path.Equals(selected))
                    {
                        prefabPlane = Instantiate(PrefabCaminoSelected, position, rotation) as Transform;
                    }
                    else
                    {
                        prefabPlane = Instantiate(PrefabCamino, position, rotation) as Transform;
                    }

                    prefabPlane.localScale = new Vector3(hypotenuse / 10, 1, 0.2f);
                    prefabPlane.parent = this.transform;

                    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        var caminoPrefab = prefabPlane.gameObject.AddComponent<UICamino>();
                        caminoPrefab.Name = path.StartNode.Name + " - " + path.EndNode.Name;
                        caminoPrefab.Distance = path.DistanceToNeighbor;
                        caminoPrefab.DistancePath = path.DistancePathed;
                    }
                }
            }
        }

        private void DestroyChilds()
        {
            foreach (Transform item in this.transform)
            {
                item.gameObject.name = "ObjectToDestroy";
                Destroy(item.gameObject);
            }
        }
    }
}
