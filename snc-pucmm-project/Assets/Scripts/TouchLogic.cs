using UnityEngine;
using System.Collections;

public class TouchLogic : MonoBehaviour
{
	public static int currentTouch = 0;
	public static bool isZooming = false;
	private Ray ray;
	private RaycastHit rayHitInfo = new RaycastHit();

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

				//executes this code for current touch (i) on screen
				if(this.guiTexture != null && (this.guiTexture.HitTest(Input.GetTouch(i).position)))
				{
					//if current touch hits our guitexture, run this code
					if(Input.GetTouch(i).phase == TouchPhase.Began)
					{
						//need to send message b/c function is not present in this script
						//OnTouchBegan();
						this.SendMessage("OnTouchBegan");
					}
					if(Input.GetTouch(i).phase == TouchPhase.Ended)
					{
						//OnTouchEnded();
						this.SendMessage("OnTouchEnded");
					}
					if(Input.GetTouch(i).phase == TouchPhase.Moved)
					{
						//OnTouchMoved();
						this.SendMessage("OnTouchMoved");
					}
				}

				if(Input.GetTouch(i).phase == TouchPhase.Began)
				{
					this.SendMessage("OnTouchBeganAnyWhere");
				}
				if(Input.GetTouch(i).phase == TouchPhase.Ended)
				{
					this.SendMessage("OnTouchEndedAnywhere");
				}
				if(Input.GetTouch(i).phase == TouchPhase.Moved)
				{
					this.SendMessage("OnTouchMovedAnywhere");
				}
				if(Input.GetTouch(i).phase == TouchPhase.Stationary)
				{
					this.SendMessage("OnTouchStayedAnywhere");
				}

				if(Input.GetTouch(i).phase == TouchPhase.Began)
				{
					ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
					if(Physics.Raycast(ray, out rayHitInfo))
				   	{
						rayHitInfo.transform.gameObject.SendMessage("OnTouchBegan3D");	
					}
				}
			}
		}
	}
}

