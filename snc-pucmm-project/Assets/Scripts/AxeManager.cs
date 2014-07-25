using UnityEngine;
using System.Collections;

public class AxeManager : MonoBehaviour {

	public static float positionAxeX;
	public static float positionAxeY;
	public static float positionAxeZ;

	// Use this for initialization
	void Start () {
		positionAxeX = this.transform.rotation.x;
		positionAxeY = this.transform.rotation.y;
		positionAxeZ = this.transform.rotation.z;
	}
	
	// Update is called once per frame
	void Update () {
		positionAxeX = this.transform.rotation.x;
		positionAxeY = this.transform.rotation.y;
		positionAxeZ = this.transform.rotation.z;
	}
}
