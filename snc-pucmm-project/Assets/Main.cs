using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	private string description;

	// Use this for initialization
	void Start () {
		Debug.Log("starting SQLiteLoad app");
		
		// Retrieve next word from database
		description = "something went wrong with the database";
		
		dbAccess db = GetComponent<dbAccess>();
		
		db.OpenDB("English.db");
		
		ArrayList result = db.SingleSelectWhere("petdef", "*", "word", "=", "'bag'");
		
		if(result.Count > 0)
		{
			description = ((string[])result[0])[2];
		}
		
		db.CloseDB();
	}

	// Update is called once per frame
	void Update () {
		
			
	}
	
	void OnGUI () {
		// create the gui text of the description
		GUI.Box (new Rect (5 ,5, Screen.width - 10, Screen.height/3), description);
		if(GUI.skin.customStyles.Length > 0)
        	GUI.skin.customStyles[0].wordWrap = true;
	}
}
