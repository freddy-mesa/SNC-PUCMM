﻿using SncPucmm.Model.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SncPucmm.View;

namespace Assets.Scripts.View
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
                
                    degrees = 180 - degrees;

                Quaternion rotation = Quaternion.AngleAxis(degrees, Vector3.up);
                Vector3 position = new Vector3(UIUtils.getXDistance(path.StartNode.Longitude), 0.7f, UIUtils.getZDistance(path.StartNode.Latitude));

                Transform prefab = (Transform)Instantiate(Prefab, position, rotation);
                prefab.transform.parent = Parent.transform;
                prefab.transform.localScale = new Vector3(hypotenuse / 10, 1, 0.2f);
            }
        }

        public void UpdateColor()
        {
            //GameObject allDirections = (GameObject.Find("Directions")).GetComponents();
        }
    }
}
