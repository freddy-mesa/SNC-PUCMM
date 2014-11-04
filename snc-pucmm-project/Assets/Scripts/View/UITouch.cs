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
        /// Tapped Building Name
        /// </summary>
        private int locationTapped;

        public static bool isMoving = false;
        public static bool isZooming = false;
        public static bool isRotating = false;
        public static bool isTapped = false;

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
                            State.GetCurrentState().Equals(eState.Navigation) || 
                            State.GetCurrentState().Equals(eState.MenuNavigation)
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

                    if (State.GetCurrentState().Equals(eState.Navigation))
                    {
                        if (this is UIModel && !isMoving && !isRotating && !isZooming && !isTapped)
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Stationary)
                            {
                                ray = Camera.main.ScreenPointToRay(objectPosition);
                                if (Physics.Raycast(ray, out rayHitInfo))
                                {
                                    var locationObject = rayHitInfo.transform.gameObject;
                                    if (locationObject != null && locationObject.tag.Equals("Building"))
                                    {
                                        locationTapped = locationObject.GetComponent<ModelObject>().Id;
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
                }
            }
            else
            {
                isMoving = isRotating = isZooming = isTapped = false;
            }
        }
    }
}