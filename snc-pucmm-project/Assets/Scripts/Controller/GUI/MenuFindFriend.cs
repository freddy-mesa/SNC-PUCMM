using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.GUI
{
    class MenuFindFriend : IMenu, IButton
    {
        #region Atributos

        string name;
        List<Button> buttonList;
        string ubicacion;

        #endregion

        #region Constructor

        public MenuFindFriend(string name, object obj)
        {
            this.name = name;
            ubicacion = string.Empty;
            Initializer();
            ProcessFindFriend(obj);
        }

        #endregion

        #region Metodos

        private void Initializer()
        {
            buttonList = new List<Button>();

            var buttonExit = new Button("ButtonExit");
            buttonExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(buttonExit);

            var buttonFindFriend = new Button("ButtonFindFriend");
            buttonFindFriend.OnTouchEvent += new OnTouchEventHandler(OnTouchFindFriendButton);
            buttonList.Add(buttonFindFriend);
        }

        private void OnTouchFindFriendButton(object sender, TouchEventArgs e)
        {
            var edificio = UIUtils.Find("/PUCMM/Model3D/" + this.ubicacion).transform;
            UICamaraControl.targetTransitionPosition = edificio.position;
            UICamaraControl.isTransitionAnimated = true;
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        private void ProcessFindFriend(object obj)
        {
            this.ubicacion = Convert.ToString(obj.GetType().GetProperty("ubicacion").GetValue(obj, null));

            var friendPhoto = (Texture2D) obj.GetType().GetProperty("texture").GetValue(obj, null);
            var friendName = Convert.ToString(obj.GetType().GetProperty("name").GetValue(obj, null));
            var fechaStr = Convert.ToString(obj.GetType().GetProperty("fecha").GetValue(obj, null));
            DateTime fecha;
            DateTime.TryParseExact(fechaStr, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            //Insertar Textura de la foto
            var edificio = UIUtils.Find("/PUCMM/Model3D/" + this.ubicacion).transform;
            var friend = edificio.FindChild("friendPhoto");
            friend.gameObject.SetActive(true);
            friend.renderer.material.mainTexture = friendPhoto;

            //Insertar descripcion en días, horas y minutos
            var resultTime = DateTime.Now - fecha;
            var description = new StringBuilder();

            if (resultTime.TotalDays > 0)
            {
                description.Append(string.Format("{0} día" + (resultTime.TotalDays > 1 ? "s " : " "), resultTime.TotalDays));
            }

            if (resultTime.TotalHours > 0)
            {
                description.Append(string.Format("{0} hora" + (resultTime.TotalHours > 1 ? "s " : " "), resultTime.TotalHours));
            }

            if (resultTime.TotalMinutes > 0)
            {
                description.Append(string.Format("y {0} minuto" + (resultTime.TotalMinutes > 1 ? "s" : ""), resultTime.TotalMinutes));
            }

            UIUtils.FindGUI(name + "/Description").GetComponent<UILabel>().text = "Hace " + description.ToString();

            //Nombre del Amigos
            UIUtils.FindGUI(name + "/Name").GetComponent<UILabel>().text = friendName;
        }

        #region Implementado

        public string GetMenuName()
        {
            return name;
        }

        public void Update()
        {
            
        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        #endregion

        #endregion        
    
    }
}
