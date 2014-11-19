using SncPucmm.Controller;
using SncPucmm.Controller.GUI;
using SncPucmm.Controller.Navigation;
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

        void Update()
        {
            if (UIGPS.GPSEnterAccessControl && Application.platform == RuntimePlatform.Android)
            {
                var currentUserPlanePosX = UIUtils.getXDistance(UIGPS.Longitude);
                var currentUserPlanePosZ = UIUtils.getZDistance(UIGPS.Latitude);
                var accuracy = Mathf.Clamp(UIGPS.Accuracy, 4f, 30f);

                this.transform.position = new Vector3(currentUserPlanePosX, 0.1f, currentUserPlanePosZ);

                this.transform.FindChild("Range").localScale = new Vector3(
                    (accuracy / 1.5f), 0.1f, (accuracy / 1.5f)
                );

                //Si hay un recorrido y no es un tour
                if (!ModelPoolManager.GetInstance().Contains("tourCtrl") && State.GetCurrentState().Equals(eState.MenuNavigation))
                {

                    //Verificar si el usuario se esta lejando del proximo nodo
                    var menuNavigation = MenuManager.GetInstance().GetCurrentMenu() as MenuNavigation;

                    var nodeStartPosX = UIUtils.getXDistance(menuNavigation.directionPath[menuNavigation.currentDirectionPath].EndNode.Longitude);
                    var nodeStartPosZ = UIUtils.getZDistance(menuNavigation.directionPath[menuNavigation.currentDirectionPath].EndNode.Latitude);
                    float resultantNodeStart = UIUtils.getDirectDistance(currentUserPlanePosX, currentUserPlanePosZ, nodeStartPosX, nodeStartPosZ);

                    var nodeEndPosX = UIUtils.getXDistance(menuNavigation.directionPath[menuNavigation.currentDirectionPath].EndNode.Longitude);
                    var nodeEndPosZ = UIUtils.getZDistance(menuNavigation.directionPath[menuNavigation.currentDirectionPath].EndNode.Latitude);
                    float resultantNodeEnd = UIUtils.getDirectDistance(currentUserPlanePosX, currentUserPlanePosZ, nodeEndPosX, nodeEndPosZ);

                    var meters = 50f;

                    //Si tanto el nodo inicial como el final estan a una distancia de 50 metros crar una nueva ruta
                    if (resultantNodeStart >= meters && resultantNodeEnd >= meters)
                    {
                        //Recuperar destino
                        var idNodoDestino = menuNavigation.directionPath[menuNavigation.directionPath.Count - 1].EndNode.IdNode;

                        //Eliminar antigua ruta
                        menuNavigation = null;
                        MenuManager.GetInstance().RemoveCurrentMenu();

                        //Crear una nueva ruta
                        var navigationCtrl = ModelPoolManager.GetInstance().GetValue("navigationCtrl") as NavigationController;
                        navigationCtrl.StartNavigation(idNodoDestino);
                    }
                }
            }
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
