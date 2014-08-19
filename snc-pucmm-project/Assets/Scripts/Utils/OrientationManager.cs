using System;
using UnityEngine;

namespace SncPucmm.Utils
{
	public class OrientationManager : MonoBehaviour
	{
		public static bool isPortrait;

		void Start(){
			isPortrait = true;
		}

		void Update(){
			if(Input.deviceOrientation.Equals(DeviceOrientation.Portrait)){
				isPortrait = true;
			}
			else if (Input.deviceOrientation.Equals(DeviceOrientation.LandscapeLeft)){
				isPortrait = false;
			}
		}
	}
}

