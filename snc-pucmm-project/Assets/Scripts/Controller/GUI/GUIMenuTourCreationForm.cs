using SncPucmm.Controller.Control;
using SncPucmm.Model;
using SncPucmm.Model.Domain;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.GUI
{
    class GUIMenuTourCreationForm : IMenu, IButton, ITextBox
    {
        #region Atributos

        string name;

        List<Button> buttonList;
        List<TextBox> textBoxList;

        List<ModelNode> modelNodeList;
        
        #endregion

        #region Constructor

        public GUIMenuTourCreationForm(string name, List<ModelNode> modelNodeList)
        {
            this.name = name;
            this.modelNodeList = modelNodeList;
            Initializer();
        }
        
        #endregion

        #region Metodos

        private void Initializer()
        {
            //Button
            buttonList = new List<Button>();

            Button buttonCreate = new Button("ButtonCreateTour");
            buttonCreate.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonCreateTour);
            buttonList.Add(buttonCreate);

            Button buttonCancel = new Button("ButtonCancelTour");
            buttonCancel.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonCancelTour);
            buttonList.Add(buttonCancel);

            //Textbox
            textBoxList = new List<TextBox>();

            TextBox txtTourName = new TextBox("txtTourName");
            txtTourName.OnChangeEvent += new OnChangeEventHandler(OnChangeText);
            textBoxList.Add(txtTourName);

            TextBox txtTourFechaInicio = new TextBox("txtTourFechaInicio");
            txtTourName.OnChangeEvent += new OnChangeEventHandler(OnChangeText);
            textBoxList.Add(txtTourFechaInicio);

            TextBox txtTourFechaFin = new TextBox("txtTourFechaFin");
            txtTourName.OnChangeEvent += new OnChangeEventHandler(OnChangeText);
            textBoxList.Add(txtTourFechaFin);

        }

        private void OnChangeText(object sender, ChangeEventArgs e)
        {
            var text = e.Mensaje as String;
            var textBox = sender as TextBox;

            textBox.Text = text;
        }

        private void OnTouchButtonCancelTour(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
            State.ChangeState(eState.TourCreation);
        }

        void OnTouchButtonCreateTour(object sender, TouchEventArgs e)
        {
            Tour tour = new Tour();
            //foreach(var textField in textBoxList)
            //{
            //    if(textField.Name == "txtTourName")
            //        tour.nombreTour = textField.Text;
            //    else if(textField.Name == "txtTourFechaInicio")
            //        tour.fechaInicio = Convert.ToDateTime(textField.Text);
            //    else if(textField.Name == "txtTourFechaFin")
            //        tour.fechaFin = Convert.ToDateTime(textField.Text);
            //}

            Usuario user = new Usuario() { nombre = "Freddy", apellido = "Mesa", usuario = "fmesa" };
            tour.usuario = user;

            var tourSystem = UIUtils.Find("TourSystem").GetComponent<UITour>();
            tourSystem.CreateTourRoleCreator(tour, modelNodeList);

            Exit();
        }

        private void Exit()
        {
            //Remove CreationTourForm
            MenuManager.GetInstance().RemoveCurrentMenu();

            //Remove CreationTour
            MenuManager.GetInstance().RemoveCurrentMenu();

            State.ChangeState(eState.Navigation);
        }

        #region Implemented Methods

        public string GetMenuName()
        {
            return name;
        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        public List<TextBox> GetTextBoxList()
        {
            return textBoxList;
        }

        #endregion

        #endregion

        #region Destructor

        ~GUIMenuTourCreationForm() 
        {
            buttonList = null;
            textBoxList = null;
            name = null;
        }

        #endregion

    }
}
