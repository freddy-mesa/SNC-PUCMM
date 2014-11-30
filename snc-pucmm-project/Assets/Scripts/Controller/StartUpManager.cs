using SncPucmm.Controller.Navigation;
using SncPucmm.Controller;
using SncPucmm.Controller.GUI;
using SncPucmm.Database;
using SncPucmm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using SncPucmm.View;
using SncPucmm.Controller.Tours;
using SncPucmm.Controller.Facebook;

namespace SncPucmm.Controller
{
    class StartUpManager : MonoBehaviour
    {
        void Start()
        {
            MenuManager.GetInstance();
            using (var service = new SQLiteService()) { }
            ModelPoolManager.GetInstance();

            GUIInitializer();
            BuildingInitializer();
            ModelPoolInit();

            UIUtils.ActivateCameraLabels(true);
            State.ChangeState(eState.Exploring);
        }

        private void GUIInitializer()
        {
            MenuManager.GetInstance().AddMenu(new MenuMain("MenuMain"));
            UIUtils.ActivateCameraLabels(true);
        }

        private void BuildingInitializer()
        {

            //Seleccionando desde la Base de datos los localidades
            var sql = "SELECT UBI.abreviacion, UBI.idUbicacion, NOD.edificio, NOD.idNodo, NOD.nombre, UBI.cantidadPlantas "+
                    "FROM Ubicacion UBI, Nodo NOD " +
                    "WHERE UBI.idUbicacion = NOD.edificio";

            List<object> list = new List<object>();
            using (var service = new SQLiteService())
            {
                using (var reader = service.SelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        list.Add(new
                        {
                            abreviacion = Convert.ToString(reader["abreviacion"]),
                            nombre = Convert.ToString(reader["nombre"]),
                            idNodo = Convert.ToInt32(reader["idNodo"]),
                            idUbicacion = Convert.ToInt32(reader["idUbicacion"]),
                            edificio = Convert.ToInt32(reader["edificio"]),
                            cantidadPlantas = Convert.ToInt32(reader["cantidadPlantas"])
                        });
                    }
                }
            }

            sql = "SELECT UBI.abreviacion, COUNT(NOD.idNodo) as insideNodesCount "+
                    "FROM Ubicacion UBI, Nodo NOD " +
                    "WHERE UBI.idUbicacion = NOD.idUbicacion "+ 
                    "GROUP BY UBI.abreviacion";

            Dictionary<string, int> buildingInsideNodeslist = new Dictionary<string,int>();
            using (var service = new SQLiteService())
            {
                using (var reader = service.SelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var abreviacion = Convert.ToString(reader["abreviacion"]);
                        var insideNodosCount = Convert.ToInt32(reader["insideNodesCount"]);

                        buildingInsideNodeslist.Add(abreviacion, insideNodosCount);
                    }
                }
            }

            //Encontrando todos los edificios con el Tag Building
            var model = GameObject.FindGameObjectsWithTag("Building");

            foreach (GameObject child in model)
            {
                var textHeader = child.transform.FindChild("Text").GetComponent<TextMesh>();

                foreach (object item in list)
                {
                    string abreviacion = Convert.ToString(item.GetType().GetProperty("abreviacion").GetValue(item, null));

                    if (child.name.Equals(abreviacion))
                    {
                        var localizacion = child.GetComponent<ModelObject>();

                        string nombre = Convert.ToString(item.GetType().GetProperty("nombre").GetValue(item, null));
                        int idUbicacion = Convert.ToInt32(item.GetType().GetProperty("idUbicacion").GetValue(item, null));
                        int idNodo = Convert.ToInt32(item.GetType().GetProperty("idNodo").GetValue(item, null));
                        int edificio = Convert.ToInt32(item.GetType().GetProperty("edificio").GetValue(item, null));
                        int cantidadPlantas = Convert.ToInt32(item.GetType().GetProperty("cantidadPlantas").GetValue(item, null));

                        var modelNode = new ModelNode() 
                        { 
                            idNodo = idNodo, 
                            name = nombre, 
                            idUbicacion = idUbicacion, 
                            abreviacion = abreviacion,
                            cantidadPlantas = cantidadPlantas
                        };

                        modelNode.isBuilding = (edificio == idUbicacion ? true : false);
                        localizacion.ObjectTag = modelNode;
                        localizacion.Id = idNodo;

                        bool containsInsideNodes = false;
                        foreach(var building in buildingInsideNodeslist)
                        {
                            if (abreviacion == building.Key && building.Value > 1) 
                            {
                                containsInsideNodes = true;
                                break;
                            }
                        }

                        localizacion.ContainsInsideNodes = containsInsideNodes;

                        if (nombre.Length < 20)
                        {
                            if (nombre.Split(' ').Length == 1)
                            {
                                textHeader.transform.localScale = new Vector3(1, 3f, 0);
                            }
                            else
                            {
                                textHeader.transform.localScale = new Vector3(1, 2f, 0);
                            }

                            textHeader.text = UIUtils.FormatStringLabel(nombre, ' ', 12);
                        }
                        else
                        {
                            textHeader.transform.localScale = new Vector3(1, 2.15f, 0);
                            textHeader.text = UIUtils.FormatStringLabel(nombre, ' ', 20);
                        }
                    }
                }
            }
        }


        private void ModelPoolInit()
        {
            NavigationController navigationCtrl = new NavigationController();
            ModelPoolManager.GetInstance().Add("navigationCtrl", navigationCtrl);
        }
    }
}
