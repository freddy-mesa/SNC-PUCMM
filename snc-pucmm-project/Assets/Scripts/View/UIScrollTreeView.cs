using System;
using System.Collections.Generic;

using UnityEngine;
using SncPucmm.Database;
using SncPucmm.Controller.Control;
using SncPucmm.Controller;

namespace SncPucmm.View
{
    class UIScrollTreeView : UITouch
    {
        #region Atributos

        GameObject itemTreeViewTemplate;
        GUIText textSearch;
        GameObject guiTreeView;

        private ScrollTreeView treeView;
        private string previousText;

        #endregion

        #region Propiedades

        public Vector2 ScrollPosition { get; set; }
        public static bool isScrolling { get; set; }
        public bool IsEqualToPreviousText { get { return previousText == textSearch.text; } }

        #endregion

        #region Metodos

        void Start()
        {
            itemTreeViewTemplate = Resources.Load("GUI/GUITreeViewItem") as GameObject;
            textSearch = UIUtils.FindGUI("GUIMenuMain/HorizontalBar/SearchBox/SearchText").guiText;
            guiTreeView = UIUtils.FindGUI("GUIMenuMain/TreeView/ScrollTreeView");

            previousText = String.Empty;
            Initialize();
        }

        new void Update()
        {
            base.Update();

            if (isScrolling)
            {
                //Creating vector of new position
                var newPosition = new Vector3(0f, ScrollPosition.y, 0f);

                //Translating vector in local position
                this.transform.Translate(newPosition, Space.Self);

                float upperLimit, downerLimit;

                if (guiTreeView.transform.childCount > 7)
                {
                    upperLimit = 0.16f;
                    downerLimit = 0.43f + 0.15f * (guiTreeView.transform.childCount - 8f);
                }
                else
                {
                    upperLimit = 0.16f;
                    downerLimit = 0.2f;
                }

                //Limitings of the local position
                this.transform.localPosition = new Vector3(
                    this.transform.localPosition.x,
                    Mathf.Clamp(this.transform.localPosition.y, upperLimit, downerLimit),
                    this.transform.localPosition.z
                );
            }

            if (IsWriting())
            {
                if (State.GetCurrentState().Equals(eState.Navigation))
                {
                    State.ChangeState(eState.GUIMenuMain);
                }

                if (!IsEqualToPreviousText)
                {
                    Initialize();
                    ShowTreeViewList(textSearch.text);
                    previousText = textSearch.text;
                }
            }
        }

        private void Initialize()
        {
            this.transform.localPosition = new Vector3(0f, 0.16f, 2f);
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
                template = itemTreeViewTemplate
            };

            treeView = (MenuManager.GetInstance().GetCurrentMenu() as IScrollTreeView).GetScrollTreeView();
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
