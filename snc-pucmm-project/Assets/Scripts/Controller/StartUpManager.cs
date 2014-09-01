using SncPucmm.Controller;
using SncPucmm.Controller.GUI;
using SncPucmm.Database;
using SncPucmm.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller
{
    class StartUpManager : MonoBehaviour
    {
        void Start() 
        {
            MenuManager.GetInstance();
            ModelPoolManager.GetInstance();
            SQLiteService.GetInstance();
            State.ChangeState(eState.Exploring);

            GUIInitializer();
            BuildingInitializer();
        }

        private void GUIInitializer()
        {
            GUIMenuMain menuMain = new GUIMenuMain("GUIMenuMain");
            MenuManager.GetInstance().AddMenu(menuMain);
        }

        private void BuildingInitializer()
        {
            var reader = SQLiteService.GetInstance().Query(true, "SELECT * FROM Localizacion");

            List<Localizacion> list = new List<Localizacion>();
            while (reader.Read())
            {
                list.Add(new Localizacion(
                    Convert.ToInt32(reader["idLocalizacion"]),
                    Convert.ToInt32(reader["idUbicacion"]),
                    Convert.ToString(reader["nombre"])
                ));
            }

            var model = GameObject.FindGameObjectsWithTag("Building");

            foreach (GameObject child in model)
            {
                var localizacion = child.GetComponent<ModelObject>();
                foreach (var item in list)
                {
                    if (item.IdLocalizacion == localizacion.Id)
                    {
                        localizacion.ObjectTag = item;
                    }
                }
            }
        }
    }
}
