using System;
using UnityEngine;

namespace SncPucmm.View
{
    public class UIUtils
    {
        /// <summary>
        /// Formats string of a Label for Menu Descriptor.
        /// </summary>
        /// <param name="stringToFormat">Label's string to format</param>
        /// <param name="delimitor">Delimitador</param>
        /// <param name="lengthToSplit">Length to split words</param>
        /// <returns></returns>
        public static String FormatStringLabel(String stringToFormat, char delimitor, int lengthToSplit) 
        {
            string[] words = stringToFormat.Split(delimitor); //Split the string into seperate words
            string result = String.Empty;
            int runningLength = 0;

            foreach (string word in words)
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
            return result.Remove(0, 1); //Remove the first space
        }
         
        public static void ActivateCameraLabels(bool activate) 
        {
            var camera = Camera.main;
            foreach (Transform label in camera.transform)
            {
                label.gameObject.SetActive(activate);
            }
        }

        public static GameObject FindGUI(string URI) 
        {
            var guiObject = GameObject.Find("GUI");
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
    }
}
