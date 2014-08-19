using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.Utils
{
	public static class AnimationManager
	{
		public static void Move(GameObject _gameObject, Dictionary<string,object> _animation){
			iTween.MoveBy(_gameObject, new Hashtable(_animation));
		}
	}
}

