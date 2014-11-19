using System;
using UnityEngine;

namespace SncPucmm.View
{
	public class UIMovement : UITouch
	{
		private Vector2 previousPosition;		//Posicion antes del Slice
		private Vector2 currentPosition;		//Posicion final del Slice
		
		public float movementConstraint = 1.5f;	//Min Limit 
		
		private float posX = 0.0f;				//distance in X axe
		private float posZ = 0.0f;				//distance in Z axe
		
		public void OnTouchBeganAnyWhere()
		{
			//Getting the first touch's position
			if(Input.touchCount == 1)
			{
				previousPosition = Input.GetTouch(0).position;
			}
		}
		
		//Se hace un Slice por delta de movimiento 
		public void OnTouchMovedAnywhere()
		{
			if (!UITouch.isRotating || !UITouch.isZooming)
			{
				//Be sure that it's a slice, only one touchCount
				if(Input.touchCount == 1)
				{					
					//Getting Actual touch's position
					currentPosition = Input.GetTouch(0).position;
					
					//Getting the substraction for the delta distance
					float touchDelta = currentPosition.magnitude - previousPosition.magnitude;
					
					//Revisando si el delta es superior a un limite inferior de movimiento
					if (Mathf.Abs(touchDelta) > movementConstraint)
					{
						UITouch.isMoving = true;

						//if(this.transform.eulerAngles.y > 6 || this.transform.eulerAngles.y < -6)
						//{
						//    posX -= Input.GetTouch(0).deltaPosition.x * movementSpeed * Time.deltaTime;
						//    posZ += Input.GetTouch(0).deltaPosition.y * movementSpeed * Time.deltaTime;
						//}
						
						if(touchDelta >= 0)
						{
							//Left
							if(currentPosition.x - previousPosition.x > currentPosition.y - previousPosition.y){
								//Getting Meter/Second in X's axe
								posX += touchDelta * Time.deltaTime;
								posZ = 0f;
							} 
							//Top
							else
							{
								//Getting Meter/Second in Z's axe
								posX = 0f;
								posZ += touchDelta * Time.deltaTime;
							}
						} 
						else {
							//Bottom
							if(currentPosition.x - previousPosition.x > currentPosition.y - previousPosition.y)
							{
								//Getting Meter/Second in Z's axe
								posX = 0f;
								posZ += touchDelta * Time.deltaTime;
							} 
							//Right
							else 
							{
								//Getting Meter/Second in X's axe
								posX += touchDelta * Time.deltaTime;
								posZ = 0f;
							}
						}
						
						//Limitando los Metros/Segundo
						posX = Mathf.Clamp(posX, -1.5f, 1.5f);
						posZ = Mathf.Clamp(posZ, -1.5f, 1.5f);

						//Preparacion para trasladar la camara, se quitan las rotaciones para que se traslade
						//solo en X y en Z sin las inclinaciones que la camara tenga
						this.transform.eulerAngles = new Vector3(0f, this.transform.eulerAngles.y, 0f);

						UICamaraControl.lastPosition = this.transform.position;
						
						//Se Traslada la camara mediante la inversion de la direccion (Modo natural del touch)
						this.transform.Translate(
							new Vector3(-posX, 0f, -posZ)
						);

						if(this.transform.position.x < -420f || this.transform.position.x > 420f ||
						   this.transform.position.z < -650f || this.transform.position.z > 400f)
						{
							this.transform.position = UICamaraControl.lastPosition;
						}

						//Se inclina la camara
						this.transform.eulerAngles = new Vector3(45f, this.transform.eulerAngles.y, 0f);
					} 
					else 
					{
						UITouch.isMoving = false;
					}
				}
			}
		}
	}
}
