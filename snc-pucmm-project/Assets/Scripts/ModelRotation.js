#pragma strict

private var f_touch_delta : float;
private var v2_current_distance : Vector2;
private var v2_previous_distance : Vector2;
private var angleOffSet : float;

var Object_to_rotate : GameObject;
var rotate : int = 8;

function Start () {

}

function Update () {
	if(Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved){
		v2_current_distance = Input.GetTouch(0).position - Input.GetTouch(1).position;
		v2_previous_distance = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
		f_touch_delta = v2_current_distance.magnitude - v2_previous_distance.magnitude;
		
		angleOffSet = Vector2.Angle(v2_previous_distance, v2_current_distance);

		if(angleOffSet > 0.1){
			if(Vector3.Cross(v2_previous_distance, v2_current_distance).z > 0){
				//Rotation Clockwise
				Camera.current.transform.Rotate(Vector3.up, angleOffSet*rotate);
			} else if(Vector3.Cross(v2_previous_distance, v2_current_distance).z < 0){
				//Rotation Counter Clockwise
				Camera.current.transform.Rotate(Vector3.up, -1*angleOffSet*rotate);
			}
		}
	}
}