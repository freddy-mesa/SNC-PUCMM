using UnityEngine;
using System.Collections;

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
	/// The is GUI visible boolean.
	/// </summary>
	protected bool isGUIVisible = false;

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

				if(!isGUIVisible)
				{
					if(Input.GetTouch(i).phase == TouchPhase.Began && this is UserInterface.UIManager)
					{
						Vector3 objectPosition = new Vector3(
							Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0f
						);

						ray = Camera.main.ScreenPointToRay(objectPosition);

						if(Physics.Raycast(ray, out rayHitInfo)){
							var buildingObject = getBuildingParent(rayHitInfo.transform.gameObject);
							if(buildingObject != null && buildingObject.tag.Equals("Building")){
								this.SendMessage("OnTouchBuilding", buildingObject.name);	
							}
						}
					} 
					else if (this is CameraMovement || this is CameraZoomRotation)
					{
						if(Input.GetTouch(i).phase == TouchPhase.Began && this is CameraMovement)
						{
							this.SendMessage("OnTouchBeganAnyWhere");
						}
						if(Input.GetTouch(i).phase == TouchPhase.Ended && this is CameraMovement)
						{
							this.SendMessage("OnTouchEndedAnywhere");
						}
						if(Input.GetTouch(i).phase == TouchPhase.Moved && (this is CameraZoomRotation || this is CameraMovement))
						{
							this.SendMessage("OnTouchMovedAnywhere");
						}
						if(Input.GetTouch(i).phase == TouchPhase.Stationary && this is CameraZoomRotation)
						{
							this.SendMessage("OnTouchStayedAnywhere");
						}
					}
				} 
				else if (this is UserInterface.UIButtonManager)
				{

					Vector3 buttonPosition = new Vector3(
						Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0f
					);

					if(this.guiTexture != null && this.guiTexture.HitTest(buttonPosition))
					{
						if(Input.GetTouch(i).phase == TouchPhase.Began)
						{
							this.SendMessage("OnTouchButtonHover");
						}
						if(Input.GetTouch(i).phase == TouchPhase.Ended)
						{
							this.SendMessage("OnTouchButton");
						}
					}
				}
			}
		}
	}

	private GameObject getBuildingParent(GameObject gameObject){
		if(gameObject.tag.Equals("Building"))
		   return gameObject;
		else if(gameObject.name.Equals("PUCMM"))
			return null;
		else
			return getBuildingParent(gameObject.transform.parent.gameObject);
	}
}

