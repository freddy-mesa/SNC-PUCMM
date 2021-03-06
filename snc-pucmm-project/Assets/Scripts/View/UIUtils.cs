﻿using SncPucmm.Database;
using SncPucmm.Model;
using SncPucmm.Model.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.View
{
    public class UIUtils
    {

        const float ORIGEN_LAT = 19.448918f;
        const float ORIGEN_LON = -70.687180f;
        const float totalMetrosAncho = (950 / 2);
        const float totalMetrosLargo = (1150 / 2);

        public static int notificationPendingFollowCount;

        static UIUtils()
        {
            notificationPendingFollowCount = 0;
        }

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
            labels.SetActive(activate);
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
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                return longitude;
            }
            else
            {
                var componentAxeX = Distance(ORIGEN_LAT, ORIGEN_LON, ORIGEN_LAT, longitude);

                if (componentAxeX >= totalMetrosAncho)
                    componentAxeX -= totalMetrosAncho;
                else
                    componentAxeX = -(totalMetrosAncho - componentAxeX);

                return (float)componentAxeX;
            }
        }

        public static float getZDistance(float latitude)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                return latitude;
            }
            else
            {
                var componentAxeZ = Distance(ORIGEN_LAT, ORIGEN_LON, latitude, ORIGEN_LON);

                if (componentAxeZ >= totalMetrosLargo)
                    componentAxeZ = -(componentAxeZ - totalMetrosLargo);
                else
                    componentAxeZ = totalMetrosLargo - componentAxeZ;

                return (float)componentAxeZ;
            }
        }

        public static float getDirectDistance(float x1, float y1, float x2, float y2)
        {
            return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        }

        public static void ConvertNodeAltitudeInMeter(string buildingName, int insideBuildingFloor, out float altitud1, out float altitud2)
        {
            float result1 = 0, result2 = 0;

            if (buildingName == "Aulas 3")
            {
                if (insideBuildingFloor == 1)
                {
                    result1 = 4.5f;
                    result2 = result1 - 3.5f;
                }
                else if (insideBuildingFloor == 2)
                {
                    result1 = 8.5f;
                    result2 = result1 - 3.5f;
                }
                else if (insideBuildingFloor == 3)
                {
                    result1 = 12.5f;
                    result2 = result1 - 3f;
                }
            }

            altitud1 = result1;
            altitud2 = result2;
        }

        public static void EnableInsideFloorCaminosBuilding(List<PathDataDijkstra> nodeList)
        {
            foreach(var path in nodeList)
            {
                if (path.StartNode.IsInsideBuilding)
                {
                    string strPath = "/PUCMM/Model3D/" + path.StartNode.BuildingName + "/Caminos";
                    var planta = Find(strPath).transform.FindChild("Planta" + path.StartNode.PlantaBuilding);
                    planta.gameObject.SetActive(true);
                }
            }
        }

        public static void DisableInsideFloorCaminosBuilding(List<PathDataDijkstra> nodeList, PathDataDijkstra selectPath)
        {
            //Buscar todos los edificios
            List<string> buildingsName = new List<string>();
            foreach (var path in nodeList)
            {
                if(path.EndNode.IsInsideBuilding)
                {
                    if (!buildingsName.Contains(path.EndNode.BuildingName))
                    {
                        buildingsName.Add(path.EndNode.BuildingName);
                    }
                }
            }

            if(selectPath.StartNode.IsInsideBuilding)
            {
                //De los edificios encontrados buscar cual de estos hace match con el path actual
                foreach (var building in buildingsName)
                {
                    if (!(selectPath.StartNode.BuildingName == building))
                    {
                        ShowEntireBuilding(building);
                    }
                }
            }
            else 
            {
                foreach (var building in buildingsName)
                {
                    ShowEntireBuilding(building);
                }
            }
        }

        public static void ShowInsidePlaneBuilding(string buildingAbbreviation, string floorName)
        {
            //Haciendo un reset del edificio
            var edificio = ShowEntireBuilding(buildingAbbreviation);

            edificio.FindChild("Otros").gameObject.SetActive(false);
            edificio.FindChild("Columnas").gameObject.SetActive(false);
            edificio.transform.FindChild("Text").gameObject.SetActive(false);

            var planos = edificio.FindChild("Planos");
            foreach (Transform plano in planos)
            {
                if (plano.name == floorName)
                {
                    plano.gameObject.SetActive(true);
                }
            }

            var caminos = edificio.FindChild("Caminos").transform;
            foreach (Transform planta in caminos)
            {
                if (planta.name == floorName)
                {
                    planta.gameObject.SetActive(true);
                }
            }

            var plantas = edificio.FindChild("Plantas").transform;

            if (floorName == "Planta1")
            {
                plantas.FindChild("Planta1").gameObject.SetActive(false);
                plantas.FindChild("Planta2").gameObject.SetActive(false);
                plantas.FindChild("Planta3").gameObject.SetActive(false);
            }
            else if (floorName == "Planta2")
            {
                plantas.FindChild("Planta2").gameObject.SetActive(false);
                plantas.FindChild("Planta3").gameObject.SetActive(false);
            }
            else
            {
                plantas.FindChild("Planta3").gameObject.SetActive(false);
            }
        }

        public static Transform ShowEntireBuilding(string buildingAbbreviation)
        {
            var edificio = UIUtils.Find("/PUCMM/Model3D/" + buildingAbbreviation).transform;

            var plantas = edificio.FindChild("Plantas");
            if (plantas != null)
            {
                foreach (Transform planta in plantas) { planta.gameObject.SetActive(true); }
            }

            var planos = edificio.FindChild("Planos");
            if (planos != null)
            {
                foreach (Transform plano in planos) { plano.gameObject.SetActive(false); }
            }

            var caminos = edificio.FindChild("Caminos");
            if (caminos != null)
            {
                foreach (Transform plano in caminos) { plano.gameObject.SetActive(false); }
            }

            var otros = edificio.FindChild("Otros");
            if (otros != null)
            {
                otros.gameObject.SetActive(true);
            }

            var columnas = edificio.FindChild("Columnas");
            if (columnas != null) 
            { 
                columnas.gameObject.SetActive(true); 
            }

            edificio.transform.FindChild("Text").gameObject.SetActive(true);

            return edificio.transform;
        }

        public static void ShowAllBuildingExterior(List<PathDataDijkstra> nodeList)
        {
            List<string> buildingsName = new List<string>();
            foreach (var path in nodeList)
            {
                if (path.EndNode.IsInsideBuilding)
                {
                    if (!buildingsName.Contains(path.EndNode.BuildingName))
                    {
                        buildingsName.Add(path.EndNode.BuildingName);
                    }
                }
            }

            foreach (var building in buildingsName)
            {
                ShowEntireBuilding(building);
            }
        }

        public static void MoveCameraToUser()
        {
            float userPosX = UIUtils.getXDistance(UIGPS.Longitude) - 40f;
            float userPosZ = UIUtils.getZDistance(UIGPS.Latitude);

            Transform camera = UIUtils.Find("/Vista3erPersona").camera.transform;
            camera.eulerAngles = new Vector3(camera.eulerAngles.x, 90, 0f);
            camera.position = new Vector3(camera.position.x, 30f, camera.position.z);

            UICamaraControl.targetTransitionPosition = new Vector3(userPosX, camera.position.y, userPosZ);
            UICamaraControl.isTransitionAnimated = true;
        }

        public static void PushFollowingNotification(bool active)
        {
            var gui = UIUtils.Find("/GUI").transform;
            var notificationButtonMain = gui.FindChild("MenuMain").FindChild("Bar").FindChild("ButtonMain").FindChild("Notification");
            var notificationButtonUsuario = gui.FindChild("MenuMain").FindChild("Sidebar").FindChild("ButtonUsuario").FindChild("Logeado").FindChild("Notification");
            var notificationButtonPendingFollowing = gui.FindChild("MenuUsuarioSettings").FindChild("ButtonPendingFollowingRequest").FindChild("Notification");

            if (active)
            {
                using (var sqlite = new SQLiteService())
                {
                    int notificationCount = 0;

                    var query = "SELECT COUNT(id) as count FROM UserFollowingNotification";
                    using (var reader = sqlite.SelectQuery(query))
                    {
                        while (reader.Read())
                        {
                            notificationCount = Convert.ToInt32(reader["count"]);
                        }
                    }

                    if (notificationCount == 0)
                    {
                        active = false;
                    }
                    else
                    {
                        notificationPendingFollowCount = notificationCount;
                        //Main Button
                        notificationButtonMain.gameObject.SetActive(true);
                        notificationButtonMain.FindChild("Label").GetComponent<UILabel>().text = notificationCount.ToString();

                        //SideBar
                        notificationButtonUsuario.FindChild("Label").GetComponent<UILabel>().text = notificationCount.ToString();
                        notificationButtonUsuario.gameObject.SetActive(true);


                        //MenuUsuario
                        notificationButtonPendingFollowing.FindChild("Label").GetComponent<UILabel>().text = notificationCount.ToString();
                        notificationButtonPendingFollowing.gameObject.SetActive(true);
                    }
                }
            }

            if (!active)
            {
                //Main Button
                notificationButtonMain.FindChild("Label").GetComponent<UILabel>().text = "";
                notificationButtonMain.gameObject.SetActive(false);

                //SideBar
                notificationButtonUsuario.FindChild("Label").GetComponent<UILabel>().text = "";
                notificationButtonUsuario.gameObject.SetActive(false);


                //MenuUsuario
                notificationButtonPendingFollowing.FindChild("Label").GetComponent<UILabel>().text = "";
                notificationButtonPendingFollowing.gameObject.SetActive(false);
            }
        }

        public static void PushSharedLocationNotification(bool active)
        {
        //    var gui = UIUtils.Find("/GUI").transform;
        //    var notificationButtonMain = gui.FindChild("MenuMain").FindChild("Bar").FindChild("ButtonMain").FindChild("Notification");
        //    var notificationButtonUsuario = gui.FindChild("MenuMain").FindChild("Sidebar").FindChild("ButtonUsuario").FindChild("Logeado").FindChild("Notification");
        //    var notificationButtonPendingFollowing = gui.FindChild("MenuUsuarioSettings").FindChild("ButtonPendingFollowingRequest").FindChild("Notification");

        //    if (active)
        //    {
        //        using (var sqlite = new SQLiteService())
        //        {
        //            int notificationCount = 0;

        //            var query = "SELECT COUNT(id) as count FROM UserFollowingNotification";
        //            using (var reader = sqlite.SelectQuery(query))
        //            {
        //                while (reader.Read())
        //                {
        //                    notificationCount = Convert.ToInt32(reader["count"]);
        //                }
        //            }

        //            if (notificationCount == 0)
        //            {
        //                active = false;
        //            }
        //            else
        //            {
        //                notificationPendingFollowCount = notificationCount;
        //                //Main Button
        //                notificationButtonMain.gameObject.SetActive(true);
        //                notificationButtonMain.FindChild("Label").GetComponent<UILabel>().text = notificationCount.ToString();

        //                //SideBar
        //                notificationButtonUsuario.FindChild("Label").GetComponent<UILabel>().text = notificationCount.ToString();
        //                notificationButtonUsuario.gameObject.SetActive(true);


        //                //MenuUsuario
        //                notificationButtonPendingFollowing.FindChild("Label").GetComponent<UILabel>().text = notificationCount.ToString();
        //                notificationButtonPendingFollowing.gameObject.SetActive(true);
        //            }
        //        }
        //    }

        //    if (!active)
        //    {
        //        //Main Button
        //        notificationButtonMain.FindChild("Label").GetComponent<UILabel>().text = "";
        //        notificationButtonMain.gameObject.SetActive(false);

        //        //SideBar
        //        notificationButtonUsuario.FindChild("Label").GetComponent<UILabel>().text = "";
        //        notificationButtonUsuario.gameObject.SetActive(false);


        //        //MenuUsuario
        //        notificationButtonPendingFollowing.FindChild("Label").GetComponent<UILabel>().text = "";
        //        notificationButtonPendingFollowing.gameObject.SetActive(false);
        //    }
        }
    }
}
