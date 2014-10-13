using SncPucmm.Model.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.View
{
    public class UIDirections : MonoBehaviour
    {
        public Transform Prefab;
        public Transform Parent;

        public void PrintDirections(List<PathData> pathList)
        {
            foreach (PathData path in pathList)
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
                Vector3 position = new Vector3(UIUtils.getXDistance(path.StartNode.Longitude), 0.7f, UIUtils.getZDistance(path.StartNode.Latitude));

                Transform prefab = (Transform)Instantiate(Prefab, position, rotation);
                var camino = prefab.gameObject.AddComponent<UICamino>();
                camino.Name = path.StartNode.Name + " - " + path.EndNode.Name;
                prefab.transform.parent = Parent.transform;
                prefab.transform.localScale = new Vector3(hypotenuse / 10, 1, 0.2f);
            }
        }
    }
}
