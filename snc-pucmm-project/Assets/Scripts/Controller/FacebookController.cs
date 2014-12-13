using Facebook;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller
{
    public class FacebookController
    {
        #region Metodos

        #region FB.Init()

        public static void Init()
        {
            FB.Init(OnInitComplete, OnHideUnity);
        }

        private static void OnInitComplete()
        {
            Debug.Log("FB.Init completed");
        }

        private static void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // start the game back up - we're getting focus again
                Time.timeScale = 1;
            }
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
                WebService.Instance.GetUsuarioInfo();
                MenuManager.GetInstance().GetCurrentMenu().Update();
            }

            Debug.Log(lastResponse);
        }

        public static void Logout()
        {
            FB.Logout();
        }

        public static void GetFollowingUserFriends()
        {
            FB.API("/me/friends", Facebook.HttpMethod.GET, FollowingUserFriendsFBResult);
        }

        private static void FollowingUserFriendsFBResult(FBResult result)
        {
            if (string.IsNullOrEmpty(result.Error))
            {
                Debug.Log("Error!");
            }
            else
            {
                Dictionary<long, string> usuarioFriendsFacebook = new Dictionary<long, string>();
                var json = new JSONObject(result.Text);
                if (json.HasField("data"))
                {
                    var data = json.GetField("data");
                    foreach (var item in data.list)
                    {
                        var id = 0L;
                        var name = string.Empty;

                        if (item.HasField("id"))
                        {
                            id = Convert.ToInt64(item.GetField("id").str);
                        }

                        if (item.HasField("name"))
                        {
                            name = item.GetField("name").str;
                        }

                        usuarioFriendsFacebook.Add(id, name);
                    }
                }

                //WebService.Instance.GetUserFriendsForFollow(usuarioFriendsFacebook);
            }
            
        }

        #endregion

        #endregion
    }
}
