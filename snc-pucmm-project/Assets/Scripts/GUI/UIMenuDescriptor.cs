using UnityEngine;
using System.Collections;

namespace UserInterface
{
	public class UIMenuDescriptor : UIManager
	{
		public GUIStyle guiStyleBackground;
		public GUIStyle guiStyleLabelBuildingName;
		public GUIStyle guiStyleButtonNavigation;
		public GUIStyle guiStyleButtonOthers;

		public GUITexture Background;
		public GUITexture LabelBuildingName;
		public GUITexture ButtonNavigation;
		public GUITexture ButtonOther;

		public override void OnGUI()
		{
			if(this.isGUIVisible){
				GUI.BeginGroup(new Rect(25, 50, Screen.width - 50, Screen.height - 100));
				//GUI.Box(new Rect(0, 0, Screen.width - 50, Screen.height - 100), texture, guiStyleBackground);
				GUI.Box(new Rect(0, 0, Screen.width - 50, Screen.height - 100), "", guiStyleBackground);

				GUI.BeginGroup(new Rect(45, 50, Screen.width - 50, Screen.height - 100));
				GUI.Label(new Rect(0,0,200,100), this.BuildingName, guiStyleLabelBuildingName);
				if(GUI.Button(new Rect(0, 150, 200,50 ), "Ir a este lugar", guiStyleButtonNavigation)){
					Debug.Log("Click me!");
				}


				GUI.Box(new Rect(0, 250, 200,50 ), "Descripcion",  	guiStyleButtonOthers);
				GUI.Box(new Rect(0, 298, 200,50 ), "Fotos", guiStyleButtonOthers);
				GUI.Box(new Rect(0, 346, 200,50 ), "Informacion Adicional", guiStyleButtonOthers);

				GUI.EndGroup();

				GUI.EndGroup();
			}
		}
	}
}