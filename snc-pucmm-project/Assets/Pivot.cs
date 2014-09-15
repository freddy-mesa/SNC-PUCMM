using UnityEngine;
using System.Collections;

public class Pivot : MonoBehaviour {

    public float gizmoSize = 0.75f;
    public Color gizmoColor = Color.yellow;

	// Use this for initialization
	void OnDrawGizmos () {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gizmoSize);
	}
}
