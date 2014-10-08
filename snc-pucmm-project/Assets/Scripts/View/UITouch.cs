using UnityEngine;
using System.Collections;
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

        /// <summary>
        /// Tapped Button 
        /// </summary>
        private string buttonTapped = string.Empty;

        public static bool isMoving = false;
        public static bool isZooming = false;
        public static bool isRotating = false;

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

                    if (this is UIButton)
                    {
                        if (!UIScrollTreeView.isScrolling)
                        {
                            bool isHover = false;

                            if (this.guiTexture != null && this.guiTexture.HitTest(objectPosition))
                            {
                                if (Input.GetTouch(i).phase == TouchPhase.Began)
                                {
                                    this.SendMessage("OnTouchHoverButton");
                                    isHover = true;
                                    buttonTapped = this.name;
                                }
                                else if (Input.GetTouch(i).phase == TouchPhase.Stationary)
                                {
                                    this.SendMessage("OnTouchHoverButton");
                                    isHover = true;
                                    buttonTapped = this.name;
                                }
                                else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                                {
                                    if (this.buttonTapped.Equals(this.name))
                                    {
                                        this.SendMessage("OnTouchButton", this.name);
                                        isHover = false;
                                    }
                                }

                                if (this.guiTexture.gameObject.activeInHierarchy && !isHover)
                                {
                                    this.SendMessage("OnTouchNormalButton");
                                }
                            }
                            else
                            {
                                this.SendMessage("OnTouchNormalButton");
                            }
                        }
                    }

                    if (this is UITextBox)
                    {
                        //Touch for open the keyboard
                        if (this.guiTexture != null && this.guiTexture.HitTest(objectPosition))
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Began)
                            {
                                this.SendMessage("OnTouchTextSearchBox", this.name);
                            }
                        }
                    }

                    if (State.GetCurrentState().Equals(eState.Navigation))
                    {
                        if (this is UIModel)
                        {
                            if (!UITouch.isMoving && !UITouch.isRotating && !UITouch.isZooming)
                            {
                                if (Input.GetTouch(i).phase == TouchPhase.Began)
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
                                            this.SendMessage("OnTouchBuilding", obj);
                                        }
                                    }
                                }
                            }
                        }
                        else if (this is UIMovement || this is UIZoomRotation)
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Began && this is UIMovement)
                            {
                                this.SendMessage("OnTouchBeganAnyWhere");
                            }
                            if (Input.GetTouch(i).phase == TouchPhase.Moved)
                            {
                                this.SendMessage("OnTouchMovedAnywhere");
                            }
                            if (Input.GetTouch(i).phase == TouchPhase.Stationary && this is UIZoomRotation)
                            {
                                this.SendMessage("OnTouchStayedAnywhere");
                            }
                        }
                    }
                    else if (State.GetCurrentState().Equals(eState.GUIMenuMain))
                    {
                        if (this is UIScrollTreeView)
                        {
                            if (this.gameObject.activeInHierarchy)
                            {
                                //Touch for Scrolling in the TreeViewList
                                var UItreeView = this.transform.parent;
                                if (UItreeView.guiTexture != null && UItreeView.guiTexture.HitTest(objectPosition))
                                {
                                    if (Input.GetTouch(i).phase == TouchPhase.Moved)
                                    {
                                        var component = GetComponent<UIScrollTreeView>();
                                        component.ScrollPosition = new Vector2(
                                            component.ScrollPosition.x,
                                            Input.GetTouch(i).deltaPosition.y * 0.0064f
                                        );

                                        UIScrollTreeView.isScrolling = true;
                                    }

                                    if (Input.GetTouch(i).phase == TouchPhase.Ended)
                                    {
                                        UIScrollTreeView.isScrolling = false;
                                    }
                                }
                                //Isn't touching the TreeViewList
                                else
                                {
                                    //If isn't the Touch Keyboard Open
                                    if (!UIKeyboard.IsTouchKeyboardOpen && !UIScrollTreeView.isScrolling)
                                    {
                                        var treeView = (MenuManager.GetInstance().GetCurrentMenu() as IScrollTreeView).GetScrollTreeView();
                                        treeView.OnClose(null);
                                        State.ChangeState(eState.Navigation);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                UITouch.isMoving = UITouch.isRotating = UITouch.isZooming = false;
            }
        }
    }
}