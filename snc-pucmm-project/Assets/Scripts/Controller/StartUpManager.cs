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

namespace SncPucmm.Controller
{
    class StartUpManager : MonoBehaviour
    {
        void Start()
        {
            MenuManager.GetInstance();
            ModelPoolManager.GetInstance();
            SQLiteService.GetInstance();
            State.ChangeState(eState.Navigation);

            GUIInitializer();
            BuildingInitializer();
            ModelPoolInit();

            UIUtils.ActivateCameraLabels(true);
        }

        private void GUIInitializer()
        {
            GUIMenuMain menuMain = new GUIMenuMain("GUIMenuMain");
            MenuManager.GetInstance().AddMenu(menuMain);
        }

        private void BuildingInitializer()
        {
            IDataReader reader;
            reader = SQLiteService.GetInstance().Query(true,
                "SELECT UBI.abreviacion, LOC.idLocalizacion, UBI.idUbicacion, LOC.nombre FROM Ubicacion UBI, Localizacion LOC " +
                "WHERE UBI.idUbicacion = LOC.idLocalizacion"
            );

            List<object> list = new List<object>();
            while (reader.Read())
            {
                list.Add(new
                {
                    Abreviacion = Convert.ToString(reader["abreviacion"]),
                    Nombre = Convert.ToString(reader["nombre"]),
                    Localizacion = Convert.ToInt32(reader["idLocalizacion"]),
                    Ubicacion = Convert.ToInt32(reader["idUbicacion"])
                });
            }

            var model = GameObject.FindGameObjectsWithTag("Building");

            foreach (GameObject child in model)
            {
                foreach (object item in list)
                {
                    string abreviacion = Convert.ToString(item.GetType().GetProperty("Abreviacion").GetValue(item, null));

                    if (child.name.Equals(abreviacion))
                    {
                        var localizacion = child.GetComponent<ModelObject>();

                        string nombre = Convert.ToString(item.GetType().GetProperty("Nombre").GetValue(item, null));
                        int idUbicacion = Convert.ToInt32(item.GetType().GetProperty("Ubicacion").GetValue(item, null));
                        int idLocalizacion = Convert.ToInt32(item.GetType().GetProperty("Localizacion").GetValue(item, null));

                        localizacion.ObjectTag = new Localizacion(idLocalizacion, idUbicacion, nombre);
                        localizacion.Id = idLocalizacion;
                    }
                }
            }
        }


        private void ModelPoolInit()
        {
            NavigationController navigationCrtl = new NavigationController();
            ModelPoolManager.GetInstance().Add("navigation", navigationCrtl);
        }
    }
}
