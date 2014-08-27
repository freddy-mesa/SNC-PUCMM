using UnityEngine;
using System.Collections;
using SncPucmm.Controller;
using SncPucmm.Controller.Utils;

namespace SncPucmm.Utils
{
	public class TouchManager : MonoBehaviour
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
        private string buildingTapped = string.Empty;

		public static bool isMoving = false;
		public static bool isZooming = false;
		public static bool isRotating = false;

		public void Update ()
		{
			//is there a touch on screen?
			if(Input.touches.Length > 0)
            {
				//loop through all the the touches on screen
				for(int i = 0; i < Input.touchCount; i++)
                {
					currentTouch = i;

					//obteniendo la posicion del objecto
					Vector3 objectPosition = new Vector3(
						Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0f
					);

					if(State.GetCurrentState().Equals(eState.Exploring))
					{
                        if (this is UIModelController)
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Began)
                            {
                                ray = Camera.main.ScreenPointToRay(objectPosition);
                                if (Physics.Raycast(ray, out rayHitInfo))
                                {
                                    var buildingObject = rayHitInfo.transform.gameObject;
                                    if (buildingObject != null && buildingObject.tag.Equals("Building")) 
                                    {
                                        buildingTapped = buildingObject.name;
                                    }            
                                }
                            }
                            else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                            {
                                ray = Camera.main.ScreenPointToRay(objectPosition);
                                if (Physics.Raycast(ray, out rayHitInfo))
                                {
                                    var buildingObject = rayHitInfo.transform.gameObject;

                                    if (buildingObject != null && buildingObject.tag.Equals("Building") && buildingTapped.Equals(buildingObject.name))
                                    {
                                        this.SendMessage("OnTouchBuilding", buildingObject.name);
                                    }
                                }
                            }
                        }
						else if (this is MovementManager || this is ZoomRotationManager)
                        {
							if(Input.GetTouch(i).phase == TouchPhase.Began && this is MovementManager)
                            {
								this.SendMessage("OnTouchBeganAnyWhere");
							}
							if(Input.GetTouch(i).phase == TouchPhase.Moved)
                            {
								this.SendMessage("OnTouchMovedAnywhere");
							}
							if(Input.GetTouch(i).phase == TouchPhase.Stationary && this is ZoomRotationManager)
                            {
								this.SendMessage("OnTouchStayedAnywhere");
							}
						}
					}
                    if (this is UIButtonController)
                    {
                        bool isHover = false;

                        if (this.guiTexture != null && this.guiTexture.HitTest(objectPosition))
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Began)
                            {
                                this.SendMessage("OnTouchHoverButton");
                                isHover = true;
                            }
                            else if (Input.GetTouch(i).phase == TouchPhase.Stationary)
                            {
                                this.SendMessage("OnTouchHoverButton");
                                isHover = true;
                            }
                            else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                            {
                                this.SendMessage("OnTouchButton");
                            }

                            if (this.guiTexture.gameObject.activeSelf && !isHover)
                            {
                                this.SendMessage("OnTouchNormalButton");
                            }
                        }
                    }
                    if (this is TextSearchController)
                    {
                        //Touch for open the keyboard
                        if (this.guiTexture != null && this.guiTexture.HitTest(objectPosition))
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Began)
                            {
                                this.SendMessage("InitializeKeyboard");
                            }
                        }
                    }
                    if(this is TreeViewController)
                    {
                        //Touch for Scrolling in the TreeViewList
                        if (this.guiTexture != null && this.guiTexture.HitTest(objectPosition))
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Moved)
                            {
                                var controller = GetComponent<TreeViewController>();
                                controller.ScrollPosition = new Vector2(
                                    controller.ScrollPosition.x,
                                    Input.GetTouch(i).deltaPosition.y
                                );
                            }
                        }
                        //Isn't touching the TreeViewList
                        else 
                        {
                            //If isn't the Touch Keyboard Open
                            if(!KeyboardManager.IsTouchKeyboardOpen)
                            {
                                UIUtils.DestroyChilds("GUIMainMenu/HorizontalBar/TreeViewList", true);
                                State.ChangeState(eState.Exploring);
                            }
                        }
                    }
				}
			}
		}
	}
}