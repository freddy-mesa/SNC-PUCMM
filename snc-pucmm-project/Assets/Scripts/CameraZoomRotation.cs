using UnityEngine;
using System.Collections;

public class CameraZoomRotation : TouchLogic 
{
	private Vector2 currentTouch1 = Vector2.zero;
	private Vector2 lastTouch1 = Vector2.zero;
	private Vector2 currentTouch2 = Vector2.zero;
	private Vector2 lastTouch2 = Vector2.zero;

	//private static float currentCameraPosY = 0.0f;

	//Zoom
	public float zoomSpeed = 0.5f;
	private float zoomFactor = 0.0f;
	private float currentDistance = 0.0f;
	private float lastDistance = 0.0f;

	//Rotation
	public float rotationSpeed = 5f;	//Rotation Speed Factor
	public float movementSpeed = 10f;		//Rotation Speed Factor

//	private float posX = 0.0f;			//distance in X axe
//	private float posZ = 0.0f;			//distance in Z axe
//	private bool inverted = true;		//Inverted Movements in TouchScreen (Natural Way)
	
	void Start(){
		//currentCameraPosY = this.transform.position.y;
	}

	void OnTouchMovedAnywhere()
	{
		TouchHandler();
	}

	void OnTouchStayedAnywhere()
	{
		TouchHandler();
	}

	void TouchHandler()
	{
		switch(TouchLogic.currentTouch)
		{
			case 0:
				currentTouch1 = Input.GetTouch(0).position;
				lastTouch1 = currentTouch1 - Input.GetTouch(0).deltaPosition;
				break;
			case 1:
				currentTouch2 = Input.GetTouch(1).position;
				lastTouch2 = currentTouch2 - Input.GetTouch(1).deltaPosition;
				break;
		}

		//Two fingers in screen
		if(TouchLogic.currentTouch >= 1)
		{
			//Rotating
			if(Input.GetTouch(0).phase == TouchPhase.Moved && 
			   Input.GetTouch(1).phase == TouchPhase.Moved)
			{
				Vector2 currentDistance = currentTouch2 - currentTouch1;
				Vector2 previousDistance = lastTouch2 - lastTouch1;

				float angleOffSet = Vector2.Angle(previousDistance, currentDistance);

				if(angleOffSet > 0.1f){
					if(Vector3.Cross(previousDistance, currentDistance).z > 0){
						//Rotation Clockwise
						this.transform.Rotate(0f, angleOffSet * rotationSpeed, 0f);
						this.transform.eulerAngles = new Vector3(15f, this.transform.eulerAngles.y, 0f);
					} else if(Vector3.Cross(previousDistance, currentDistance).z < 0){
						//Rotation Counter Clockwise
						this.transform.Rotate(0f, angleOffSet * rotationSpeed * -1, 0f);
						this.transform.eulerAngles = new Vector3(15f, this.transform.eulerAngles.y, 0f);
					}
				}
			//Zooming
			} else {
				//Obtain distance magnitude when is Zooming  
				currentDistance = Vector2.Distance(currentTouch2,currentTouch1);
				lastDistance = Vector2.Distance(lastTouch2,lastTouch1);

				zoomFactor = Mathf.Clamp(lastDistance - currentDistance, -30.0f, 30.0f);
				this.transform.Translate(Vector3.forward * zoomFactor * zoomSpeed * Time.deltaTime * -1);
			}
			
			//Obtain the current position of the camera
			//currentCameraPosY = this.transform.position.y;
		}
	}
}