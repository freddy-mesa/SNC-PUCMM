using System;
using UnityEngine;

public class CameraMovement : TouchManager
{
	private Vector2 previousPosition;		//Posicion antes del Slice
	private Vector2 currentPosition;		//Posicion final del Slice

	public float movementFactor = 1f;	//Min Limit 
	public float movementSpeed = 0.01f;		//Movement Speed

	private bool isMoving = false;

	private float posX = 0.0f;				//distance in X axe
	private float posZ = 0.0f;				//distance in Z axe

	void OnTouchBeganAnyWhere()
	{
		//Getting the first touch's position
		if(Input.touchCount == 1){
			previousPosition = Input.GetTouch(0).position;
		}
	}

	//Se hace un Slice por delta de movimiento 
	void OnTouchMovedAnywhere()
	{
		//Be sure that it's a slice, only one touchCount
		if(Input.touchCount == 1){

			this.isMoving = true;

			//Getting Actual touch's position
			currentPosition = Input.GetTouch(0).position;

			//Getting the substraction for the delta distance
			float touchDelta = currentPosition.magnitude - previousPosition.magnitude;

			//Revisando si el delta es superior a un limite inferior de movimiento
			if(Mathf.Abs(touchDelta) > movementFactor){
				//Orientacion Vectical
				if(this.transform.eulerAngles.y > 6 || this.transform.eulerAngles.y < -6){
					posX -= Input.GetTouch(0).deltaPosition.x * movementSpeed * Time.deltaTime;
					posZ += Input.GetTouch(0).deltaPosition.y * movementSpeed * Time.deltaTime;
				}
				else if(touchDelta >= 0){
					//Left
					if(currentPosition.x - previousPosition.x > currentPosition.y - previousPosition.y){
						//Getting Meter/Second in X's axe
						posX += touchDelta * movementSpeed * Time.deltaTime;
						posZ = 0f;
					} 
					//Top
					else {
						//Getting Meter/Second in Z's axe
						posX = 0f;
						posZ += touchDelta * movementSpeed * Time.deltaTime;
					}
				} 
				else {
					//Bottom
					if(currentPosition.x - previousPosition.x > currentPosition.y - previousPosition.y){
						//Getting Meter/Second in Z's axe
						posX = 0f;
						posZ += touchDelta * movementSpeed * Time.deltaTime;
					} 
					//Right
					else {
						//Getting Meter/Second in X's axe
						posX += touchDelta * movementSpeed * Time.deltaTime;
						posZ = 0f;
					}
				}

				//Limitando los Metros/Segundo
				posX = Mathf.Clamp(posX, -1f, 1f);
				posZ = Mathf.Clamp(posZ, -1f, 1f);

				//Preparacion para trasladar la camara, se quitan las rotaciones para que se traslade
				//solo en X y en Z sin las inclinaciones que la camara tenga
				this.transform.eulerAngles = new Vector3(0f, this.transform.eulerAngles.y, 0f);

				//Se Traslada la camara mediante la inversion de la direccion (Modo natural del touch)
				this.transform.Translate(
					new Vector3(-posX, 0f, -posZ)
				);

				//Se inclina la camara
				this.transform.eulerAngles = new Vector3(15f, this.transform.eulerAngles.y, 0f);

			}
		}
	}

	void OnTouchEndedAnyWhere(){
		if(this.isMoving){
			this.transform.position = new Vector3(-posX, 0f, -posZ);
			this.isMoving = false;
		}
	}
}