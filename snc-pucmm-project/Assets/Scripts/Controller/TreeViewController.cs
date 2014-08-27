using SncPucmm.Controller.Utils;
using SncPucmm.Database;
using SncPucmm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller
{
    class TreeViewController : TouchManager
    {
        #region Atributos

        public GameObject itemTreeViewTemplate;
        public GUIText textSearch;

        private string previousText;

        #endregion

        #region Propiedades

        public Vector2 ScrollPosition { get; set; }

        #endregion

        #region Metodos

        void Start() 
        {
            previousText = String.Empty;
            ScrollPosition = this.transform.localPosition;
        }

        new void Update() 
        {
            if (IsWriting() && !IsEqualToPreviousText())
            {
                ShowTreeViewList(textSearch.text);
                previousText = textSearch.text;
            }

            base.Update();
        }

        //void OnGUI() 
        //{
        //    if (IsWriting())
        //    {
        //        scrollPosition = GUI.BeginScrollView(
        //            treeViewList.guiTexture.GetScreenRect(Camera.main),
        //            scrollPosition,
        //            treeViewList.guiTexture.GetScreenRect(Camera.main),
        //            GUIStyle.none,
        //            GUIStyle.none
        //        );

        //        GUI.EndScrollView();
        //    }
        //}

        /// <summary>
        /// Show the Tree View List from 
        /// </summary>
        /// <param name="text">Text to search in Database</param>
        void ShowTreeViewList(string text) 
        {
            //Obteniendo de la Base de datos
            var sqliteService = SQLiteService.GetInstance();
            var reader = sqliteService.Query( 
                true,
                "SELECT nombre,idUbicacion,idLocalizacion " + 
                    "FROM Localizacion " + 
                    "WHERE nombre LIKE '%"+text+"%' "+
                    "ORDER BY idUbicacion, idLocalizacion"
            );

            //Guardando los datos en memoria
            var textList = new List<object>();
            while (reader.Read())
            {
                var lugar = new {
                    nombre = reader["nombre"],
                    ubicacion = reader["idUbicacion"], 
                    localizacion = reader["idLocalizacion"],
                };

                textList.Add(lugar);
            }
            reader = null;

            //Eliminando los hijos del Tree View List
            UIUtils.DestroyChilds("GUIMainMenu/HorizontalBar/TreeViewList", true);

            //Agregando los hijos al Tree View List
            for(int i=0; i < textList.Count; i++)
            {
                GameObject item;

                //Creando el item del Tree View con world coordinates
                item = Instantiate(itemTreeViewTemplate) as GameObject;

                //Agregando relacion de padre (Tree View List) - hijo (item del Tree View List)
                item.transform.parent = this.transform;

                //Agregando la posicion relativa del hijo con relacion al padre
                item.transform.localPosition = new Vector3(
                    itemTreeViewTemplate.transform.localPosition.x,
                    itemTreeViewTemplate.transform.localPosition.y - 0.15f * i,
                    itemTreeViewTemplate.transform.localPosition.z
                );

                //Agregando la escala relativa del hijo con relacion al padre
                item.transform.localScale = itemTreeViewTemplate.transform.localScale;
               
                //Encontrando texto del un item (su hijo)
                var itemText = item.transform.FindChild("text");

                var nombre = Convert.ToString(textList[i].GetType().GetProperty("nombre").GetValue(textList[i], null));
                var ubicacion = Convert.ToInt32(textList[i].GetType().GetProperty("ubicacion").GetValue(textList[i], null));
                var localizacion = Convert.ToInt32(textList[i].GetType().GetProperty("localizacion").GetValue(textList[i], null));

                //Si son iguales la localizacion es un nombre de un edificio
                if (ubicacion == localizacion)
                {
                    itemText.guiText.text = UIUtils.FormatStringLabel(nombre, ' ', 36);
                }
                //De lo contrario esta dentro del edificio
                else
                {
                    //Se agrega un padding de 5 espacios
                    itemText.guiText.text = UIUtils.FormatStringLabel(nombre.PadLeft(5,' '), ' ', 36);
                }
            }            
        }

        /// <summary>
        /// Verifica si hay algo algo escrito en el Texto del SearchBox
        /// </summary>
        /// <returns>true si hay algo escrito, de lo contrario, falso</returns>
        bool IsWriting() 
        {
            return (this.textSearch.text == String.Empty ? false : true);
        }

        /// <summary>
        /// Verifica si en este frame, el texto del SearchText es igual su ultima actualizacion
        /// </summary>
        /// <returns></returns>
        bool IsEqualToPreviousText() 
        {
            return this.textSearch.text.Equals(previousText);
        }

        #endregion
    }
}
