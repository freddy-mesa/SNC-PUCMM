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
        Transform searchBox;
        UIInput textSeach;

        private ScrollView treeView;
        private string previousText;
        private bool isSearchBoxActive;

        #endregion

        #region Propiedades

        #endregion

        #region Metodos

        void Start()
        {
            itemTreeView = Resources.Load("GUI/TreeViewScrollItem") as GameObject;
        }

        new void Update()
        {
            if (searchBox != null && searchBox.gameObject.activeInHierarchy)
            {
                if(!isSearchBoxActive)
                {
                    if (State.GetCurrentState().Equals(eState.Exploring))
                    {
                        isSearchBoxActive = true;
                        ShowTreeViewList("");
                        State.ChangeState(eState.MenuMain);
                    }
                    else
                    {
                        isSearchBoxActive = true;
                    }
                    
                }

                if (textSeach == null)
                {
                    textSeach = searchBox.GetComponent<UIInput>();
                }

                if (textSeach.value != previousText)
                {
                    ShowTreeViewList(textSeach.value);
                    previousText = textSeach.value;
                }
            }

            if (isSearchBoxActive)
            {
                if (!searchBox.gameObject.activeInHierarchy)
                {
                    textSeach = null;
                    isSearchBoxActive = false;
                }
            }

            base.Update();
        }

        /// <summary>
        /// Show the Tree View List from 
        /// </summary>
        /// <param name="text">Text to search in Database</param>
        public void ShowTreeViewList(string text)
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


        public void SetTextSearch(Transform searchBox)
        {
            this.searchBox = searchBox;
            this.textSeach = searchBox.GetComponent<UIInput>();
            isSearchBoxActive = false;
            previousText = string.Empty;
        }

        #endregion
    }
}
