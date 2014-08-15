using System;
using UnityEngine;

namespace SncPucmm.Utils
{
	public class TextureResolution : MonoBehaviour
	{		
		void Start()
		{
			IterateAllChildrens(this.transform);
		}

		private void IterateAllChildrens(Transform parent){
			if(parent.guiTexture != null || parent.guiText != null){
				Rect pi = parent.guiTexture.pixelInset;
				parent.guiTexture.pixelInset = new Rect(pi.x*2, pi.y*2, pi.width*2, pi.height*2);
			}

			foreach (Transform child in parent){
				IterateAllChildrens(child);
			}
		}
	}
}

