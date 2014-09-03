using System;
using UnityEngine;

namespace SncPucmm.View
{
	public class UIResolution : MonoBehaviour
	{
        public float originWidth = 800;
        public float originHeight = 1280;

        void Start()
		{
            float scalex = (float)(Screen.width) / originWidth; //your scale x
            float scaley = (float)(Screen.height) / originHeight; //your scale y

            var allGUIText = GetComponentsInChildren(typeof(GUIText), true); ; //all GUIText

            foreach (GUIText item in allGUIText)
            {
                Vector2 pixOff = item.pixelOffset; //your pixel offset on screen
                int origSizeText = item.fontSize;
                item.pixelOffset = new Vector2(pixOff.x * scalex, pixOff.y * scaley);
                item.fontSize = (int) Mathf.Round(origSizeText * scalex);
            }
		}
	}
}

