using UnityEngine;
using System.Collections;

public class PinchZoom : TouchLogic 
{
	private Vector2 currentTouch1 = Vector2.zero;
	private Vector2 lastTouch1 = Vector2.zero;
	private Vector2 currentTouch2 = Vector2.zero;
	private Vector2 lastTouch2 = Vector2.zero;
	
	public float zoomSpeed = 0.5f;
	public float zoomFactor = 0.0f;
	public float currentDistance = 0.0f;
	public float lastDistance = 0.0f;

	void OnTouchMovedAnywhere()
	{
		Zoom();
	}

	void OnTouchStayedAnywhere()
	{
		Zoom();
	}

	void Zoom()
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

		//be Sure to have two fingers in screen
		if(TouchLogic.currentTouch >= 1)
		{
			TouchLogic.isZooming = true;
			currentDistance = Vector2.Distance(currentTouch2,currentTouch1);
			lastDistance = Vector2.Distance(lastTouch2,lastTouch1);
		}
		else
		{
			TouchLogic.isZooming = false;
			currentDistance = 0.0f;
			lastDistance = 0.0f;
		}

		zoomFactor = Mathf.Clamp(lastDistance - currentDistance, -30.0f, 30.0f);

		this.transform.Translate(Vector3.forward * zoomFactor * zoomSpeed * Time.deltaTime * -1);
	}
}
