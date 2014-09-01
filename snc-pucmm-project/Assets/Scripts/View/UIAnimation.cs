using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.View
{
	public static class UIAnimation
	{
		public static void MoveBy(GameObject _gameObject, Dictionary<string,object> _animation)
        {
			iTween.MoveBy(_gameObject, new Hashtable(_animation));
		}

        public static void ScaleBy(GameObject _gameObject, Dictionary<string, object> _animation)
        {
            iTween.ScaleBy(_gameObject, new Hashtable(_animation));
        }
	}
}

