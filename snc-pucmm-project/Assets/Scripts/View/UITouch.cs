using UnityEngine;
using System.Collections;
using SncPucmm.Controller;
using SncPucmm.Model;

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
                        if (this is UIModel)
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
                                    var idLocation = locationObject.GetComponent<ModelObject>().Id;

                                    if (locationObject != null && locationObject.tag.Equals("Building") && locationTapped.Equals(idLocation))
                                    {
                                        var obj = new { location = idLocation, button = "ModelController" };
                                        this.SendMessage("OnTouchBuilding", obj);
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
							if(Input.GetTouch(i).phase == TouchPhase.Moved)
                            {
								this.SendMessage("OnTouchMovedAnywhere");
							}
                            if (Input.GetTouch(i).phase == TouchPhase.Stationary && this is UIZoomRotation)
                            {
								this.SendMessage("OnTouchStayedAnywhere");
							}
						}
					}
                    if (this is UIButton)
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
                                this.SendMessage("OnTouchButton", this.name);
                            }

                            if (this.guiTexture.gameObject.activeSelf && !isHover)
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
                    if(this is UITreeView)
                    {
                        //Touch for Scrolling in the TreeViewList
                        if (this.guiTexture != null && this.guiTexture.HitTest(objectPosition))
                        {
                            if (Input.GetTouch(i).phase == TouchPhase.Moved)
                            {
                                var controller = GetComponent<UITreeView>();
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
                            if(!UIKeyboard.IsTouchKeyboardOpen)
                            {
                                var treeView = (MenuManager.GetInstance().GetCurrentMenu() as ITreeView).GetTreeView();
                                treeView.OnClose(null);
                            }`
                        }
                    }
				}
			}
		}
	}
}