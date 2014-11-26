using System;
using System.Collections.Generic;

using UnityEngine;
using SncPucmm.Database;
using SncPucmm.Controller.Control;
using SncPucmm.Controller;

namespace SncPucmm.View
{
    class UIScrollViewControl : UITouch
    {
        #region Atributos

        GameObject itemTreeView;
        UILabel textSearch;

        private ScrollView treeView;
        private string previousText;

        #endregion

        #region Propiedades

        public bool IsEqualToPreviousText { get { return previousText == textSearch.text; } }

        #endregion

        #region Metodos

        void Start()
        {
            itemTreeView = Resources.Load("GUI/TreeViewScrollItem") as GameObject;
            textSearch = UIUtils.FindGUI("MenuMain/Bar/SearchBox/Label").GetComponent<UILabel>();

            previousText = String.Empty;
        }

        new void Update()
        {
            base.Update();

            if (IsWriting())
            {
                if (State.GetCurrentState().Equals(eState.Exploring))
                {
                    State.ChangeState(eState.MenuMain);
                }

                if (!IsEqualToPreviousText)
                {
                    ShowTreeViewList(textSearch.text);
                    previousText = textSearch.text;
                }
            }
        }

        /// <summary>
        /// Show the Tree View List from 
        /// </summary>
        /// <param name="text">Text to search in Database</param>
        void ShowTreeViewList(string text)
        {
            var obj = new
            {
                text = text,
                parent = this.transform,
                template = itemTreeView
            };

            treeView = (MenuManager.GetInstance().GetCurrentMenu() as IScrollView).GetScrollView();
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
