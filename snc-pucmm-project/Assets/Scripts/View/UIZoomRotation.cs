using UnityEngine;

namespace SncPucmm.View
{
	public class UIZoomRotation : UITouch
	{
		private Vector2 currentTouch1 = Vector2.zero;
		private Vector2 lastTouch1 = Vector2.zero;
		private Vector2 currentTouch2 = Vector2.zero;
		private Vector2 lastTouch2 = Vector2.zero;
		
		//Zoom
		public float zoomSpeed = 100f;
		private float currentDistance = 0.0f;
		private float lastDistance = 0.0f;
		private float angleOffSet = 0.0f;
		
		//Rotation
		public float rotationSpeed = 2f;		//Rotation Speed Factor
		
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
			switch (UITouch.currentTouch)
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
			if (UITouch.currentTouch >= 1)
			{
				//Rotating, los touch se esta moviendo
				if(
					(Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) 
					&& !UITouch.isMoving
				)
				{
					Vector2 currentDistance = currentTouch2 - currentTouch1;
					Vector2 previousDistance = lastTouch2 - lastTouch1;
					
					angleOffSet = Vector2.Angle(previousDistance, currentDistance);

					if(angleOffSet > 0.5f)
					{
						UITouch.isRotating = true;
						if(Vector3.Cross(previousDistance, currentDistance).z > 0)
						{
							//Rotation in Clockwise
							this.transform.Rotate(0f, angleOffSet * rotationSpeed * -1, 0f);
							this.transform.eulerAngles = new Vector3(45f, this.transform.eulerAngles.y, 0f);
						} 
						else if(Vector3.Cross(previousDistance, currentDistance).z < 0)
						{
							//Rotation in Counter Clockwise
							this.transform.Rotate(0f, angleOffSet * rotationSpeed, 0f);
							this.transform.eulerAngles = new Vector3(45f, this.transform.eulerAngles.y, 0f);
						}
					} 
					else 
					{
						UITouch.isRotating = false;
					}
				}
				else 
				{
					UITouch.isRotating = false;
				}

				//Zooming Condition
				if ((!UITouch.isRotating || !UITouch.isMoving) && angleOffSet < 1.5f)
				{
					UITouch.isZooming = true;

					//Obtain distance magnitude when is Zooming  
					currentDistance = Vector2.Distance(currentTouch2, currentTouch1);
					lastDistance = Vector2.Distance(lastTouch2, lastTouch1);

					var zoomFactor = Mathf.Clamp(lastDistance - currentDistance, -30.0f, 30.0f);

					if (!(zoomFactor < 7.5f && zoomFactor > -7.5f))
					{
						var zoomVectorDistance = Vector3.forward * zoomFactor * zoomSpeed * Time.deltaTime * -1;
						var previosPosition = this.transform.position;

						this.transform.Translate(zoomVectorDistance);

						if (
							this.transform.position.x < -420f || this.transform.position.x > 420f ||
							this.transform.position.y < 30f || this.transform.position.y > 75f ||
							this.transform.position.z < -650f || this.transform.position.z > 400f
						)
						{
							this.transform.position = previosPosition;
						}
					}
					else
					{
						UITouch.isZooming = false;
					}
				}
				else
				{
					UITouch.isZooming = false;
				}
			}
		}
	}	
}

