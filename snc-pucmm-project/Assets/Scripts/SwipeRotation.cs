using UnityEngine;
using System.Collections;

public class SwipeRotation : TouchLogic
{  
	public float rotateSpeed = 10f;

	private float pitch = 0.0f;
	private float yaw = 0.0f;

	void OnTouchMovedAnywhere ()
	{
		if(Input.touchCount > 0 || this.name.Equals("Vista_3er_Persona")){
			pitch -= Input.GetTouch(0).deltaPosition.y * rotateSpeed * Time.deltaTime;
			yaw += Input.GetTouch(0).deltaPosition.x * rotateSpeed * Time.deltaTime;

			pitch = Mathf.Clamp(pitch,-10,45);

			//Rotation
			this.transform.eulerAngles = new Vector3(-pitch, -yaw,0.0f);
		}
	}
}

