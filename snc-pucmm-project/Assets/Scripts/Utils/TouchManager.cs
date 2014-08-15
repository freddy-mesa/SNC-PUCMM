using UnityEngine;
using System.Collections;
using SncPucmm.Controller;

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

		public static bool isMoving = false;
		public static bool isZooming = false;
		public static bool isRotating = false;

		void Update ()
		{
			//is there a touch on screen?
			if(Input.touches.Length <= 0)
			{
				//if no touches then execute this code
			}
			else //if there is a touch
			{
				//loop through all the the touches on screen
				for(int i = 0; i < Input.touchCount; i++)
				{
					currentTouch = i;

					if(State.GetCurrentState().Equals(eState.Exploring))
					{
						if(Input.GetTouch(i).phase == TouchPhase.Ended && this is UIManager)
						{
							Vector3 objectPosition = new Vector3(
								Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0f
							);

							ray = Camera.main.ScreenPointToRay(objectPosition);

							if(Physics.Raycast(ray, out rayHitInfo)){

								var buildingObject = rayHitInfo.transform.gameObject;

								if(buildingObject != null && buildingObject.tag.Equals("Building")){
									this.SendMessage("OnTouchBuilding", buildingObject.name);	
								}
							}
						} 
						else if (this is Movement || this is ZoomRotation)
						{
							if(Input.GetTouch(i).phase == TouchPhase.Began && this is Movement)
							{
								this.SendMessage("OnTouchBeganAnyWhere");
							}
							if(Input.GetTouch(i).phase == TouchPhase.Ended && this is Movement)
							{
								this.SendMessage("OnTouchEndedAnywhere");
							}
							if(Input.GetTouch(i).phase == TouchPhase.Moved)
							{
								this.SendMessage("OnTouchMovedAnywhere");
							}
							if(Input.GetTouch(i).phase == TouchPhase.Stationary && this is ZoomRotation)
							{
								this.SendMessage("OnTouchStayedAnywhere");
							}
						}
					} 

					if(State.GetCurrentState().Equals(eState.GUISystem))
					{
						if (this is UIButtonManager)
						{
							Vector3 buttonPosition = new Vector3(
								Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0f
							);

							bool isHover = false;

							if(this.guiTexture != null && this.guiTexture.HitTest(buttonPosition))
							{
								if(Input.GetTouch(i).phase == TouchPhase.Began)
								{
									this.SendMessage("OnTouchHoverButton");
									isHover = true;
								}
								if(Input.GetTouch(i).phase == TouchPhase.Stationary)
								{
									this.SendMessage("OnTouchHoverButton");
									isHover = true;
								}
								if(Input.GetTouch(i).phase == TouchPhase.Ended)
								{
									this.SendMessage("OnTouchButton");
								}
								if(!isHover)
								{
									this.SendMessage("OnTouchNormalButton");
								}
							}
						}
					}
				}
			}
		}

//		private GameObject getBuildingParent(GameObject gameObject){
//			if(gameObject.tag.Equals("Building"))
//			   return gameObject;
//			else if(gameObject.name.Equals("PUCMM"))
//				return null;
//			else
//				return getBuildingParent(gameObject.transform.parent.gameObject);
//		}
	}
}

