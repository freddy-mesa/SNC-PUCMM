using SncPucmm.Model;
using UnityEngine;

namespace SncPucmm.View
{
    class UICharacter : MonoBehaviour
    {
        public static bool isCollidingWithBuilding { get; set; }
        public static string BuildingColliding { get; set; }

        void Start()
        {
            isCollidingWithBuilding = false;
            BuildingColliding = string.Empty;
        }

        void OnTriggerEnter(Collider objectCollider)
        {
            if (objectCollider.gameObject.tag == "Building")
            {
                isCollidingWithBuilding = true;
                var modelNode = objectCollider.GetComponent<ModelObject>().ObjectTag as ModelNode;

                BuildingColliding = modelNode.name;
            }
        }

        void OnTriggerExit(Collider objectCollider) 
        {
            if (objectCollider.gameObject.tag == "Building")
            {
                isCollidingWithBuilding = false;
                BuildingColliding = string.Empty;
            }
        }
    }
}
