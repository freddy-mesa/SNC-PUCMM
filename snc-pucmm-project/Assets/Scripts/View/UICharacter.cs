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

        public float metersBetweenNodesAlternativeRoute = 25;
        public float metersBetweenUserAndNodesToBeVisited = 15;

        void Start()
        {
            isCollidingWithBuilding = false;
            BuildingColliding = string.Empty;
        }

        void Update()
        {
            var currentUserPlanePosX = UIUtils.getXDistance(UIGPS.Longitude);
            var currentUserPlanePosZ = UIUtils.getZDistance(UIGPS.Latitude);
            var accuracy = Mathf.Clamp(UIGPS.Accuracy, 4f, 30f);

            this.transform.position = new Vector3(currentUserPlanePosX, 1f, currentUserPlanePosZ);

            this.transform.FindChild("Range").localScale = new Vector3(
                (accuracy / 1.5f), 0.1f, (accuracy / 1.5f)
            );

            //Si hay un recorrido
            if (State.GetCurrentState() == eState.MenuNavigation)
            {
                var menuNavigation = MenuManager.GetInstance().GetCurrentMenu() as MenuNavigation;

                if (!menuNavigation.isFreeModeActive)
                {
                    var path = menuNavigation.directionPath[menuNavigation.currentDirectionPath];
                    bool control = false;

                    var nodeStartPosX = menuNavigation.directionPath[menuNavigation.currentDirectionPath].StartNode.Longitude;
                    var nodeStartPosZ = menuNavigation.directionPath[menuNavigation.currentDirectionPath].StartNode.Latitude;
                    float resultantUserToStartNode = UIUtils.getDirectDistance(currentUserPlanePosX, currentUserPlanePosZ, nodeStartPosX, nodeStartPosZ);

                    var nodeEndPosX = menuNavigation.directionPath[menuNavigation.currentDirectionPath].EndNode.Longitude;
                    var nodeEndPosZ = menuNavigation.directionPath[menuNavigation.currentDirectionPath].EndNode.Latitude;
                    float resultantUserToEndNode = UIUtils.getDirectDistance(currentUserPlanePosX, currentUserPlanePosZ, nodeEndPosX, nodeEndPosZ);

                    if (path.StartNode.Visited && path.EndNode.Visited)
                    {
                        //Ir al proximo path
                        if (++menuNavigation.currentDirectionPath < menuNavigation.directionPath.Count)
                        {
                            var pathNext = menuNavigation.directionPath[menuNavigation.currentDirectionPath];
                            menuNavigation.ShowDirectionMenu(pathNext);
                            menuNavigation.ShowNavigationDirection(pathNext);
                        }
                        else
                        {
                            //Ha finalizado un recorrido
                            menuNavigation.currentDirectionPath = menuNavigation.directionPath.Count - 1;
                        }

                        control = true;
                    }
                    else
                    {
                        if (!path.StartNode.Visited && resultantUserToStartNode <= metersBetweenUserAndNodesToBeVisited)
                        {
                            path.StartNode.Visited = true;
                            control = true;
                        }

                        if (!path.EndNode.Visited && resultantUserToEndNode <= metersBetweenUserAndNodesToBeVisited)
                        {
                            path.EndNode.Visited = true;
                            control = true;
                        }
                    }

                    //Busqueda de Ruta Alterna si no es un tour
                    if (!ModelPoolManager.GetInstance().Contains("tourCtrl") && !control)
                    {
                        //Verificar si el usuario se esta lejando de los nodos de un path                   
                        //Si tanto el nodo inicial como el final estan a una distancia X en metros, crear una nueva ruta
                        if (resultantUserToStartNode >= metersBetweenNodesAlternativeRoute && resultantUserToEndNode >= metersBetweenNodesAlternativeRoute)
                        {
                            UINotification.StartNotificationRecalcularRuta = true;

                            //Recuperar nodo destino
                            var destinoName = menuNavigation.directionPath[menuNavigation.directionPath.Count - 1].EndNode.Name;

                            //Eliminar antigua ruta
                            menuNavigation = null;
                            MenuManager.GetInstance().RemoveCurrentMenu();

                            //Recalcular la ruta
                            var navigationCtrl = ModelPoolManager.GetInstance().GetValue("navigationCtrl") as NavigationController;
                            navigationCtrl.StartNavigation(destinoName);
                        }
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
