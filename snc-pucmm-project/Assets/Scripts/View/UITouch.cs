using UnityEngine;
using SncPucmm.Controller;
using SncPucmm.Model;
using SncPucmm.Controller.Control;

namespace SncPucmm.View
{
    public class UITouch : MonoBehaviour
    {
        /// <summary>
        /// The current touch.
        /// </summary>
        public static int currentTouch = 0;

        /// <summary>
        /// The ray.
        /// </summary>
        private Ray ray;

        /// <summary>
        /// The ray hit info.
        /// </summary>
        private RaycastHit rayHitInfo = new RaycastHit();

        /// <summary>
        /// Tapped Building Id
        /// </summary>
        private static int locationTapped;

        /// <summary>
        /// Tapped Inside Building Location Name
        /// </summary>
        private static string insideLocationTapped;

        public static bool isMoving = false;
        public static bool isZooming = false;
        public static bool isRotating = false;
        public static bool isButtonTapped = false;
        public static bool isModelTapped = false;

        public void Update()
        {
            //is there a touch on screen?
            if (Input.touches.Length > 0)
            {
                //loop through all the the touches on screen
                for (int i = 0; i < Input.touchCount; i++)
                {
                    currentTouch = i;

                    //obteniendo la posicion del objecto
                    Vector3 objectPosition = new Vector3(
                        Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0f
                    );
                    
                    if (
                            State.GetCurrentState() == eState.Exploring || 
                            State.GetCurrentState() == eState.MenuNavigation ||
                            State.GetCurrentState() == eState.MenuInsideBuilding
                        )
                    {
                        
                        if (this is UIMovement || this is UIZoomRotation)
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Began && this is UIMovement)
                            {
                                ((UIMovement)this).OnTouchBeganAnyWhere();
                            }
                            if (Input.GetTouch(i).phase == TouchPhase.Moved)
                            {
                                this.SendMessage("OnTouchMovedAnywhere");
                            }
                            if (Input.GetTouch(i).phase == TouchPhase.Stationary && this is UIZoomRotation)
                            {
                                ((UIZoomRotation)this).OnTouchStayedAnywhere();
                            }
                        }
                    }

                    if (State.GetCurrentState() == eState.Exploring)
                    {
                        if (this is UIModel && !isMoving && !isRotating && !isZooming)
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Stationary && !isButtonTapped)
                            {
                                ray = Camera.main.ScreenPointToRay(objectPosition);
                                if (Physics.Raycast(ray, out rayHitInfo))
                                {
                                    var locationObject = rayHitInfo.transform.gameObject;
                                    if (locationObject != null && locationObject.tag.Equals("Building"))
                                    {
                                        locationTapped = locationObject.GetComponent<ModelObject>().Id;
                                        isButtonTapped = true;
                                    }
                                }
                            }
                            else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                            {
                                ray = Camera.main.ScreenPointToRay(objectPosition);
                                if (Physics.Raycast(ray, out rayHitInfo))
                                {
                                    var locationObject = rayHitInfo.transform.gameObject;
                                    var modelObject = locationObject.GetComponent<ModelObject>();

                                    if (locationObject != null && locationObject.tag.Equals("Building") && locationTapped.Equals(modelObject.Id))
                                    {
                                        var obj = new { location = modelObject.ObjectTag, button = "ModelController" };
                                        ((UIModel)this).OnTouchBuilding(obj);
                                    }
                                }
                            }
                        }
                    }
                    else if (State.GetCurrentState() == eState.MenuInsideBuilding)
                    {
                        if (this is UIInsideBuilding && !isMoving && !isRotating && !isZooming)
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Stationary && !isButtonTapped)
                            {
                                ray = Camera.main.ScreenPointToRay(objectPosition);
                                if (Physics.Raycast(ray, out rayHitInfo))
                                {
                                    var locationObject = rayHitInfo.transform.gameObject;
                                    if (locationObject != null && locationObject.tag.Equals("InsideBuilding"))
                                    {
                                        insideLocationTapped = locationObject.name;
                                        isButtonTapped = true;
                                    }
                                }
                            }
                            else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                            {
                                ray = Camera.main.ScreenPointToRay(objectPosition);
                                if (Physics.Raycast(ray, out rayHitInfo))
                                {
                                    var locationObject = rayHitInfo.transform.gameObject;
                                    if (locationObject != null && locationObject.tag.Equals("InsideBuilding") && insideLocationTapped.Equals(this.name))
                                    {
                                        ((UIInsideBuilding) this).OnTouchBuildingInsideLocation(this.name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                isMoving = isRotating = isZooming = isButtonTapped = false;
                locationTapped = 0;
                insideLocationTapped = "";
            }
        }
    }
}