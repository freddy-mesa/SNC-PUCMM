using SncPucmm.Model.Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.View
{
    public class UIDirections : MonoBehaviour
    {
        public Transform PrefabCamino;
        public Transform PrefabCaminoSelected;

        public void PrintPath(List<PathDataDijkstra> pathList, PathDataDijkstra selected)
        {
            UIUtils.DestroyChilds("/PUCMM/Directions", false);

            foreach (PathDataDijkstra path in pathList)
            {
                float adjacent = UIUtils.getXDistance(path.StartNode.Longitude) - UIUtils.getXDistance(path.EndNode.Longitude);
                float opposite = UIUtils.getZDistance(path.StartNode.Latitude) - UIUtils.getZDistance(path.EndNode.Latitude);
                
                float hypotenuse = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));
                float degrees = Mathf.Asin(opposite / hypotenuse) * 180 / Mathf.PI;

                if (adjacent >= 0)
                {
                    degrees = 180 - degrees;
                }

                Quaternion rotation = Quaternion.AngleAxis(degrees, Vector3.up);
                Vector3 position = new Vector3(UIUtils.getXDistance(path.StartNode.Longitude), 1f, UIUtils.getZDistance(path.StartNode.Latitude));

                Transform prefabPlane = null;
                if (path.Equals(selected))
                {
                    prefabPlane = Instantiate(PrefabCaminoSelected, position, rotation) as Transform;
                }
                else
                {
                    prefabPlane = Instantiate(PrefabCamino, position, rotation) as Transform;
                }

                var camino = prefabPlane.gameObject.AddComponent<UICamino>();
                camino.Name = path.StartNode.Name + " - " + path.EndNode.Name;

                prefabPlane.transform.parent = this.transform;
                prefabPlane.transform.localScale = new Vector3(hypotenuse / 10, 1, 0.2f);
            }
        }

        private void PrintDirection()
        {

        }
    }
}
