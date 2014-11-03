using System;
using UnityEngine;

namespace SncPucmm.View
{
    public class UIUtils
    {
        const float ORIGEN_LAT = 19.448918f;
        const float ORIGEN_LON = -70.687180f;
        const float totalMetrosAncho = (950 / 2);
        const float totalMetrosLargo = (1150 / 2);

        public static String FormatStringLabel(String stringToFormat, char delimitor, int lengthToSplit) 
        {
            bool isPadding = (stringToFormat[0] == ' ' &&  stringToFormat[1] == ' ') ? true : false;

            string[] words = stringToFormat.Trim().Split(delimitor); //Split the string into seperate words
            string result = String.Empty;
            int runningLength = 0;

            foreach (string word in words)
            {
                if (isPadding)
                {
                    if (runningLength + word.Length + 1 <= lengthToSplit - 25)
                    {
                        result += " " + word;
                        runningLength += word.Length + 1;
                    }
                    else
                    {
                        result += "\n".PadRight(5) + word;
                        runningLength = word.Length;
                    }
                }
                else
                {
                    if (runningLength + word.Length + 1 <= lengthToSplit)
                    {
                        result += " " + word;
                        runningLength += word.Length + 1;
                    }
                    else
                    {
                        result += "\n" + word;
                        runningLength = word.Length;
                    }
                }
            }
            return result.Remove(0, 1); //Remove the first space
        }
         
        public static void ActivateCameraLabels(bool activate) 
        {
            var labels = FindGUI("CameraLabels");
            foreach (Transform label in labels.transform)
            {
                label.gameObject.SetActive(activate);
            }
        }

        public static GameObject FindGUI(string URI) 
        {
            var guiObject = GameObject.Find("/GUI");
            return guiObject.transform.FindChild(URI).gameObject;
        }

        public static GameObject Find(string URI)
        {
            return GameObject.Find(URI);
        }

        public static void DestroyChilds(string URI, bool isGUI) 
        {
            GameObject gameObject;

            if(isGUI)
            {
                gameObject = FindGUI(URI);
            }
            else 
            {
                gameObject = Find(URI);
            }

            foreach (Transform item in gameObject.transform)
            {
                UnityEngine.Object.Destroy(item.gameObject);
            }
        }

        private static double Distance(float lat1, float lon1, float lat2, float lon2)
        {
            var R = 6378.137; // Radius of earth in KM
            var dLat = (lat2 - lat1) * Mathf.PI / 180;
            var dLon = (lon2 - lon1) * Mathf.PI / 180;
            var a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
                    Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
            var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
            var d = R * c;
            return d * 1000f; // meters
        }

        public static float getXDistance(float longitude)
        {
            var componentAxeX = Distance(ORIGEN_LAT, ORIGEN_LON, ORIGEN_LAT, longitude);

            if (componentAxeX >= totalMetrosAncho)
                componentAxeX -= totalMetrosAncho;
            else
                componentAxeX = -(totalMetrosAncho - componentAxeX);
            
            return (float)componentAxeX;
        }

        public static float getZDistance(float latitude)
        {
            var componentAxeZ = Distance(ORIGEN_LAT, ORIGEN_LON, latitude, ORIGEN_LON);

            if (componentAxeZ >= totalMetrosLargo)
                componentAxeZ = -(componentAxeZ - totalMetrosLargo);
            else
                componentAxeZ = totalMetrosLargo - componentAxeZ;
            
            return (float)componentAxeZ;
        }

        public static float getDirectDistance(float x1, float y1, float x2, float y2)
        {
            return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        }
    }
}
