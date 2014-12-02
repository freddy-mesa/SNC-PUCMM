using Facebook;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.Facebook
{
    public class FacebookController
    {
        #region Metodos

        #region FB.Init()

        public static void Init()
        {
            FB.Init(OnInitComplete);
        }

        private static void OnInitComplete()
        {
            Debug.Log("FB.Init completed");
        }

        #endregion

        #region FB.Login()

        public static void Login()
        {
            FB.Login("email,public_profile,user_friends", LoginCallback);
        }

        static private void LoginCallback(FBResult result)
        {
            var lastResponse = string.Empty;

            if (result.Error != null)
            {
                lastResponse = "Error Response:\n" + result.Error;
                UINotification.StartNotificationNoInternet = true;
            }
            else if (!FB.IsLoggedIn)
            {
                //No se pudo logear
                lastResponse = "Login cancelled by Player";
                UINotification.StartNotificationNoInternet = true;
            }
            else
            {
                //Login correcto
                lastResponse = "Login was successful!";
                WebService.Instance.GetUsuario(Convert.ToInt64(FB.UserId));
                MenuManager.GetInstance().GetCurrentMenu().Update();
            }

            Debug.Log(lastResponse);
        }

        public static void Logout()
        {
            FB.Logout();
        }

        #endregion

        #endregion
    }
}
