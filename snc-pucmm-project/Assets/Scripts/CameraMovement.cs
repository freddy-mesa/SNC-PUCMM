using System;
using UnityEngine;

public class CameraMovement : TouchLogic
{
	private Vector2 previousPosition;
	private Vector2 currentPosition;

	public float movementFactor = 0.1f;		//Min Limit 
	public float movementSpeed = 5f;		//Movement Speed

	private float posX = 0.0f;				//distance in X axe
	private float posZ = 0.0f;				//distance in Z axe

	void OnTouchBeganAnyWhere()
	{
		if(Input.touchCount == 1){
			previousPosition = Input.GetTouch(0).position;
		}
	}

	void OnTouchMovedAnywhere()
	{
		if(Input.touchCount == 1){
			currentPosition = Input.GetTouch(0).position;
			float touchDelta = currentPosition.magnitude - previousPosition.magnitude;

			if(Mathf.Abs(touchDelta) > movementFactor){
				if(touchDelta >= 0){
					if(currentPosition.x - previousPosition.x > currentPosition.y - previousPosition.y){
						//Left
						posX += touchDelta * movementSpeed * Time.deltaTime;
						posZ = 0f;
					} else {
						//Top
						posX = 0f;
						posZ += touchDelta * movementSpeed * Time.deltaTime;
					}
				} 
				else {
					if(currentPosition.x - previousPosition.x > currentPosition.y - previousPosition.y){
						//Bottom
						posX = 0f;
						posZ += touchDelta * movementSpeed * Time.deltaTime;
					} else {
						//Right
						posX += touchDelta * movementSpeed * Time.deltaTime;
						posZ = 0f;
					}
				}

				posX = Mathf.Clamp(posX,-500 * SncPucmmUtils.Metros, 500 * SncPucmmUtils.Metros);
				posZ = Mathf.Clamp(posZ,-500 * SncPucmmUtils.Metros, 500 * SncPucmmUtils.Metros);

				this.transform.eulerAngles = new Vector3(0f, 0f, 0f);

				this.transform.Translate(
					new Vector3(-posX * SncPucmmUtils.Metros, 0f, -posZ * SncPucmmUtils.Metros)
				);

				this.transform.eulerAngles = new Vector3(15f, 0f, 0f);
			}
		}
	}
}