using System;
using System.Collections.Generic;

using UnityEngine;
using SncPucmm.Database;
using SncPucmm.Controller.Control;
using SncPucmm.Controller;

namespace SncPucmm.View
{
    class UITreeView : UITouch
    {
        #region Atributos

        public Transform itemTreeViewTemplate;
        public GUIText textSearch;
        private TreeView treeView;
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
            if (IsWriting() && !UITextBox.isEqualToPreviousText)
            {
                ShowTreeViewList(textSearch.text);
                previousText = textSearch.text;
            }

            base.Update();
        }

        void OnGUI() 
        {
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
        }

        /// <summary>
        /// Show the Tree View List from 
        /// </summary>
        /// <param name="text">Text to search in Database</param>
        void ShowTreeViewList(string text) 
        {
            var obj = new { 
                text = text,
                parent = this.transform,
                template = itemTreeViewTemplate
            };

            treeView = (MenuManager.GetInstance().GetCurrentMenu() as ITreeView).GetTreeView();
            treeView.OnChange(obj);
        }

        /// <summary>
        /// Verifica si hay algo algo escrito en el Texto del SearchBox
        /// </summary>
        /// <returns>true si hay algo escrito, de lo contrario, falso</returns>
        bool IsWriting() 
        {
            return (this.textSearch.text == String.Empty ? false : true);
        }

        #endregion
    }
}
