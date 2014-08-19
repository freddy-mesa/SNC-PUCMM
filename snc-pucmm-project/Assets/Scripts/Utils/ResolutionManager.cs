using System;
using UnityEngine;

namespace SncPucmm.Utils
{
	public class ResolutionManager : MonoBehaviour
	{
		void Start()
		{
			IterateAllChildrens(this.transform);
		}

		private void IterateAllChildrens(Transform parent){

			if(parent.guiText != null){
				var _ratio = 19f;
				var _fontSize = Mathf.Min(Screen.width, Screen.height) / _ratio;
				parent.guiText.fontSize = Convert.ToInt32(_fontSize);
			}

			foreach (Transform child in parent){
				IterateAllChildrens(child);
			}
		}
	}
}

