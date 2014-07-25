using UnityEngine;
using System.Collections;

public class TapTouch : TouchLogic
{
	public UILabel label;

	void OnClick()
	{
		Debug.Log("Edificio: " + this.name);
		label.text = this.name;
	}
}