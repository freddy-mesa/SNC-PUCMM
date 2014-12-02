using SncPucmm.Controller.Control;
using SncPucmm.Controller.Facebook;
using SncPucmm.Controller.GUI;
using SncPucmm.Controller.Navigation;
using SncPucmm.Database;
using SncPucmm.Model.Domain;
using SncPucmm.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller
{
    class WebService : MonoBehaviour
    {
        #region Atributos

        private static WebService instance = null;
        
        #endregion

        #region Propiedades

        private bool IsEnterToUpdateModel { get; set; }
        private bool IsEnterToUpdateTours { get; set; }
        private bool IsEnterConnectWithFacebook { get; set; }
        private bool IsEnterNotificationFollowingFriend { get; set; }
        public static WebService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<WebService>();
                    if (instance == null)
                    {
                        instance = new GameObject("WebService").AddComponent<WebService>();
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Constructor

        private WebService()
        {

        }

        #endregion

        #region Metodos
        
        public static Coroutine GET(string url, System.Action<bool, string> callback)
        {
            return Instance.StartCoroutine(GetRequest(url, callback));
        }

        public static Coroutine POST(string url, WWWForm form, System.Action<bool, string> callback)
        {
            return Instance.StartCoroutine(PostRequest(url, form, callback));
        }

        private static IEnumerator GetRequest(string url, System.Action<bool, string> callback)
        {
            var webService = new HTTP.Request("GET", url);
            yield return webService.Send();
            if (webService.exception != null)
            {
                Debug.Log(webService.exception);
                if (callback != null)
                {
                    callback(false, webService.exception.Message);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(true, webService.response.Text);
                }
            }
        }

        private static IEnumerator PostRequest(string url, WWWForm form, System.Action<bool, string> callback)
        {
            var webService = new HTTP.Request(url, form);
            yield return webService.Send();
            if (webService.exception != null)
            {
                Debug.Log(webService.exception);
                if (callback != null)
                {
                    callback(false, webService.exception.Message);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(true, webService.response.Text);
                }
            }
        }

        void Start()
        {
            IsEnterToUpdateModel = true;
            IsEnterToUpdateTours = true;
            IsEnterNotificationFollowingFriend = true;
            IsEnterConnectWithFacebook = true;
        }

        void Update()
        {
            if (IsEnterConnectWithFacebook)
            {
                IsEnterConnectWithFacebook = false;
                StartCoroutine(ConnectWithFacebook());
            }

            //if (IsEnterToUpdateModel)
            //{
            //    IsEnterToUpdateModel = false;
            //    StartCoroutine(UpdateModelService());
            //}

            //if (FB.IsInitialized && FB.IsLoggedIn && IsEnterToUpdateTours)
            //{
            //    IsEnterToUpdateTours = false;
            //    StartCoroutine(UpdateToursService());
            //}

            if (FB.IsInitialized && FB.IsLoggedIn && IsEnterNotificationFollowingFriend)
            {
                IsEnterNotificationFollowingFriend = false;
                StartCoroutine(ReceiveFollowingRequest());
            }

        }

        private IEnumerator UpdateModelService()
        {
            bool IsConnectionAvailable = false;
            string responseJson = string.Empty;

            //Obtener la ultima actualizacion del servidor
            yield return WebService.GET(
                "http://localhost:8080/model/update", 
                (status, response) => 
                {
                    if (status) responseJson = response;
                    IsConnectionAvailable = status;
                }
            );

            //Esperar que se refresque el valor de responseJson
            yield return new WaitForSeconds(0.5f);

            if (IsConnectionAvailable)
            {
                IsEnterToUpdateModel = false;

                //Enconding de string a Json
                JSONObject json = new JSONObject(responseJson);
                Debug.Log(responseJson);

                //Hacer un update a la base de datos
                using (var sqlService = new SQLiteService())
                {
                    sqlService.UpdateModel(json);
                }

                var navigation = ModelPoolManager.GetInstance().GetValue("navigationCtrl") as NavigationController;

                //Crear el grafo con la base de datos actualizada.
                navigation.CreateGraph();
            }
            else
            {
                if (!IsEnterToUpdateModel)
                {
                    StartCoroutine(CountDown(600f,value => IsEnterToUpdateModel = value));
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        private IEnumerator UpdateToursService()
        {
            bool isConnectionAvailable = false;
            string responseJson = string.Empty;
            //JSONObject json = SQLiteService.GetInstance().DataSynchronization();

            //Obtener la ultima actualizacion del servidor
            yield return WebService.GET(
                "http://localhost:8080/SNCWeb/tour/list",
                (status, response) =>
                {
                    if (status) { responseJson = response; }
                    isConnectionAvailable = status;
                }
            );

            //Esperar que se refresque el valores
            yield return new WaitForSeconds(0.5f);

            if (isConnectionAvailable)
            {
                //Enconding de string a Json
                JSONObject json = new JSONObject(responseJson);

                Debug.Log("Peticion de UpdateTours al WebService: " + json.ToString());

                //Hacer un update a la base de datos
                using (var sqlService = new SQLiteService())
                {
                    sqlService.UpdateTours(json);
                }
            }


            if (!IsEnterToUpdateTours)
            {
                StartCoroutine(CountDown(600, value => IsEnterToUpdateTours = value));
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator ConnectWithFacebook()
        {
            bool isConnectionAvailable = false;

            yield return WebService.GET(
                "http://www.google.com",
                (status, response) =>
                {
                    isConnectionAvailable = status;
                }
            );

            yield return new WaitForSeconds(0.5f);

            if (isConnectionAvailable)
            {
                FacebookController.Init();
            }
            else
            {
                if (!IsEnterConnectWithFacebook)
                {
                    StartCoroutine(CountDown(10f, value => IsEnterConnectWithFacebook = value));
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        public void GetUserFriendsForFollow()
        {
            StartCoroutine(FollowingUserFriends());
        }

        private IEnumerator FollowingUserFriends()
        {
            bool IsConnectionAvailable = false;
            string responseJson = string.Empty;
            yield return WebService.GET("https://graph.facebook.com/me/friends?access_token=" + FB.AccessToken, 
                (status, response) => 
                {
                    if(status)
                    {
                        IsConnectionAvailable = status;
                        responseJson = response;
                    }
                }
            );

            //Esperar que se refresque el valores
            yield return new WaitForSeconds(1f);

            if (IsConnectionAvailable)
            {
                Dictionary<long, string> usuarioFriendsFacebook = new Dictionary<long, string>();
                var json = new JSONObject(responseJson);
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

                //Filtrar usuario que no han sido enviadas
                WWWForm form = new WWWForm();
                form.AddField("id", FB.UserId);

                var IsConnectionAvailableServer = false;
                responseJson = string.Empty;
                yield return WebService.POST("http://localhost:8080/SNCWeb/usuario/following", form,
                    (status, response) =>
                    {
                        if (status)
                        {
                            IsConnectionAvailableServer = status;
                            responseJson = response;
                        }
                    }
                );

                //Esperar que se refresque el valores
                yield return new WaitForSeconds(0.5f);

                if (IsConnectionAvailableServer)
                {
                    var data = new JSONObject(responseJson);
                    for(int i = 0; i < data.list.Count; ++i)
                    {
                        var idFollowing = Convert.ToInt64(data.list[i].GetField("id").n);

                        if (usuarioFriendsFacebook.ContainsKey(idFollowing))
                        {
                            usuarioFriendsFacebook.Remove(idFollowing);
                        }
                    }

                    //Get Item Template
                    Transform itemTemplate = (Resources.Load("GUI/FriendFollowingItem") as GameObject).transform;

                    //Get Parent
                    Transform parent = UIUtils.FindGUI("MenuSendFollowingRequest/ScrollView").transform;

                    int k = 0;
                    foreach(var usuario in usuarioFriendsFacebook)
                    {
                        var id = usuario.Key;

                        yield return StartCoroutine(GetUsuarioFacebook(id));

                        WWW photo = new WWW("https://graph.facebook.com/" + id + "/picture?width=50&height=50"); //?access_token=" + FB.AccessToken);
                        Texture2D textFb2 = new Texture2D(50, 50, TextureFormat.DXT5, false); //TextureFormat must be DXT5

                        yield return photo;

                        photo.LoadImageIntoTexture(textFb2);

                        var friendItem = GameObject.Instantiate(itemTemplate.gameObject) as GameObject;

                        friendItem.name = "ItemFriend" + k;
                        friendItem.transform.parent = parent;
                        friendItem.transform.localScale = itemTemplate.localScale;

                        //Agregando la posicion relativa del hijo con relacion al padre
                        friendItem.transform.localPosition = new Vector3(
                            itemTemplate.localPosition.x,
                            itemTemplate.localPosition.y - 60f * k,
                            itemTemplate.localPosition.z
                        );

                        friendItem.transform.FindChild("Image").GetComponent<UITexture>().mainTexture = textFb2;
                        friendItem.transform.FindChild("Label").GetComponent<UILabel>().text = usuario.Value;

                        var checkBoxGameObject = friendItem.transform.FindChild("CheckBox");
                        checkBoxGameObject.name = checkBoxGameObject.name + friendItem.name;
                        checkBoxGameObject.GetComponent<UIToggle>().value = false;
                        

                        var menu = MenuManager.GetInstance().GetCurrentMenu() as MenuSendFollowingRequest;

                        var checkBox = new CheckBox(checkBoxGameObject.name);
                        checkBox.OnChangeEvent += new OnChangeEventHandler(menu.OnChangeCheckBox);

                        checkBox.ObjectTag = new { idUsuario = id };

                        menu.GetCheckBoxList().Add(checkBox);                        

                        friendItem.GetComponent<BoxCollider>().enabled = true;

                        k++;
                    }
                }
            }
        }

        private IEnumerator SendUserFacebookId(Usuario user)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", user.idUsuarioFacebook.Value.ToString());
            form.AddField("fist_name", user.nombre);
            form.AddField("last_name", user.apellido);
            form.AddField("gender", user.gender);
            form.AddField("email", user.email);

            yield return WebService.POST("http://localhost:8080/SNCWeb/usuario/crear", form, (status, response) => { });
        }

        public void GetUsuario(long UserId)
        {
            StartCoroutine(GetUsuarioFacebook(UserId));
        }

        private IEnumerator GetUsuarioFacebook(long UserId)
        {
            bool IsConnectionAvailable = false;
            string responseJson = string.Empty;

            yield return WebService.GET("https://graph.facebook.com/" + UserId + "?access_token=" + FB.AccessToken,
                (status, response) =>
                {
                    if (status)
                    {
                        IsConnectionAvailable = status;
                        responseJson = response;
                    }
                }
            );

            yield return new WaitForSeconds(0.5f);

            if (IsConnectionAvailable)
            {
                var json = new JSONObject(responseJson);
                var user = new Usuario(json);

                var modelPool = ModelPoolManager.GetInstance();

                if (!modelPool.Contains("Usuario"))
                {
                    user.email = user.email.Replace("\\u0040", "@");
                    ModelPoolManager.GetInstance().Add("Usuario", user);

                    using (var sqliteService = new SQLiteService())
                    {
                        bool isInDB = false;
                        var query = "SELECT * FROM Usuario WHERE idUsuarioFacebook = " + user.idUsuarioFacebook.Value;
                        using (var reader = sqliteService.SelectQuery(query))
                        {
                            if (reader.Read())
                            {
                                isInDB = true;
                            }
                            else
                            {
                                isInDB = false;
                            }
                        }

                        if (!isInDB)
                        {
                            query = "SELECT MAX(id) as id FROM Usuario";

                            int id = 0;
                            using (var reader = sqliteService.SelectQuery(query))
                            {
                                int temp;
                                if(int.TryParse(Convert.ToString(reader["id"]), out temp))
                                {
                                    id = temp;
                                }
                            }

                            id++;
                            sqliteService.TransactionalQuery(
                                "INSERT INTO Usuario VALUES (" + id + "," + user.idUsuarioFacebook + ",'" + user.nombre + "','" + user.apellido + "','" + user.email + "','" + user.gender + "');"
                            );
                        }
                    }

                    yield return StartCoroutine(UserInfoRequest(75, 75));
                }

                yield return StartCoroutine(SendUserFacebookId(user));
            }
        }

        public void GetUserInfoRequest(int width, int height)
        {
            StartCoroutine(UserInfoRequest(width, height));
        }

        private IEnumerator UserInfoRequest(int width, int height)
        {
            //yield return new WaitForSeconds(0.25f);

            WWW photo = new WWW("https://graph.facebook.com/" + FB.UserId + "/picture?width=" + width + "&height=" + height); //?access_token=" + FB.AccessToken);
            Texture2D textFb2 = new Texture2D(width, height, TextureFormat.DXT5, false); //TextureFormat must be DXT5

            yield return photo;

            photo.LoadImageIntoTexture(textFb2);

            var logeado = UIUtils.FindGUI("MenuMain/Sidebar/ButtonUsuario").transform.FindChild("Logeado");
            logeado.GetComponent<UITexture>().mainTexture = textFb2;

            using (var sqliteService = new SQLiteService())
            {
                using(var reader = sqliteService.SelectQuery("SELECT nombre, apellido FROM Usuario WHERE idUsuarioFacebook = " + FB.UserId))
                {
                    if (reader.Read())
                    {
                        logeado.transform.FindChild("Label").GetComponent<UILabel>().text = Convert.ToString(reader["nombre"]) + " " + Convert.ToString(reader["apellido"]);
                    }
                }
            }
        }

        public void SendFollowingRequest(long idFollowed, List<long> userToFollowed)
        {
            JSONObject jsonArray = new JSONObject(JSONObject.Type.ARRAY);
            foreach (var follow in userToFollowed)
            {
                jsonArray.Add(follow.ToString());
            }

            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("id", idFollowed.ToString());
            json.AddField("usuarios", jsonArray);

            WWWForm form = new WWWForm();
            form.AddField("json", json.ToString());

            StartCoroutine(FollowingRequest(form));
        }

        private IEnumerator FollowingRequest(WWWForm form)
        {
            yield return WebService.POST("http://localhost:8080/SNCWeb/usuario/followRequest", form, (status, response) => { });
        }

        public IEnumerator ReceiveFollowingRequest()
        {
            yield return new WaitForSeconds(10f);

            bool isAvailableConnection = false;
            string jsonResponse = string .Empty;
            
            var idUser = "10152587482388668";

            WWWForm form = new WWWForm();
            //form.AddField("id", FB.UserId);
            form.AddField("id", idUser);

            yield return WebService.POST("http://localhost:8080/SNCWeb/usuario/notifyRequest", form, 
                (status, response) => 
                {
                    if (status)
                    {
                        jsonResponse = response;
                    }
                    isAvailableConnection = status;
                }
            );

            yield return new WaitForSeconds(0.5f);

            if (isAvailableConnection)
            {
                var json = new JSONObject(jsonResponse);

                Debug.Log(json.print(true));

                if (json.list.Count > 0)
                {
                    using (var sqlite = new SQLiteService())
                    {
                        Dictionary<long, string> notificationToSave = new Dictionary<long, string>();
                        var query = string.Empty;
                        foreach (var notification in json.list)
                        {
                            Debug.Log(notification.GetField("id").str);
                            var idNotification = Convert.ToInt64(notification.GetField("id").str);
                            Debug.Log("Followed: " + idUser + ", Follower: " + idNotification);
                            bool isInDB = false;

                            //Comparar si hay alguna notificacion que este en la base de datos
                            
                            query = "SELECT * FROM UserFollowingNotification WHERE idUsuarioFacebook = '" + idUser + "' AND idFollower = '" + idNotification + "'";
                            //query = "SELECT * FROM UserFollowingNotification WHERE idUsuarioFacebook = '" + FB.UserId + "' AND idFollower = '" + idNotification + "'";
                            using (var reader = sqlite.SelectQuery(query))
                            {
                                if (reader.HasRows)
                                {
                                    isInDB = true;
                                }
                            }

                            if (!isInDB)
                            {
                                var nombre = notification.GetField("name").str;
                                notificationToSave.Add(idNotification, nombre);
                            }
                        }

                        if (notificationToSave.Count > 0)
                        {
                            query = "SELECT MAX(id) as id FROM UserFollowingNotification";
                            int id = 0;
                            using (var reader = sqlite.SelectQuery(query))
                            {
                                if (reader.Read())
                                {
                                    int temp;
                                    if (int.TryParse(Convert.ToString(reader["id"]), out temp))
                                    {
                                        id = temp;
                                    }
                                }
                            }

                            id++;
                            var queryBuilder = new StringBuilder();
                            foreach (var notification in notificationToSave)
                            {
                                queryBuilder.Append("INSERT INTO UserFollowingNotification VALUES (" + id + ",'" + idUser + "','" + notification.Key + "','" + notification.Value + "');");
                                //queryBuilder.Append("INSERT INTO UserFollowingNotification VALUES (" + id + ",'" + FB.UserId + "','" + notification.Key + "','" + notification.Value + "');");
                                id++;
                            }

                            sqlite.TransactionalQuery(queryBuilder.ToString());
                        }
                    }
                }
            }

            if (!IsEnterNotificationFollowingFriend)
            {
                StartCoroutine(CountDown(1f, value => IsEnterNotificationFollowingFriend = value));
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void GetUserFriendsPendingToFollow(long userId)
        {
            //StartCoroutine(UserFriendsPendingToFollow(userId));
            LocalUserFriendsPendingToFollow(userId);
        }

        public IEnumerator UserFriendsPendingToFollow(long userId)
        {
            Dictionary<long, string> usuarioFriendsFacebook = new Dictionary<long, string>();

            using (var sqlite = new SQLiteService())
            {
                var query = "SELECT idFollower, nombre FROM UserFollowingNotification WHERE idUsuarioFacebook = '" + userId + "'";
                using (var reader = sqlite.SelectQuery(query))
                {
                    while (reader.Read())
                    {
                        var id = Convert.ToInt64(Convert.ToString(reader["idFollower"]));
                        var nombre = Convert.ToString(reader["nombre"]);

                        usuarioFriendsFacebook.Add(id, nombre);
                    }
                }
            }

            Transform itemTemplate = (Resources.Load("GUI/PendingFriendFollowingItem") as GameObject).transform;

            //Get Parent
            Transform parent = UIUtils.FindGUI("MenuReceiveFollowingRequest/ScrollView").transform;

            int k = 0;
            foreach (var usuario in usuarioFriendsFacebook)
            {
                var id = usuario.Key;
                var url = ("https://graph.facebook.com/" + id + "/picture?width=50&height=50&access_token=" + FB.AccessToken);
                WWW photo = new WWW(url);
                //WWW photo = new WWW("https://graph.facebook.com/" + id + "/picture?width=128&height=128"); //?access_token=" + FB.AccessToken);
                Texture2D textFb2 = new Texture2D(50, 50, TextureFormat.DXT5, false); //TextureFormat must be DXT5

                yield return photo;

                photo.LoadImageIntoTexture(textFb2);

                var friendItem = GameObject.Instantiate(itemTemplate.gameObject) as GameObject;

                friendItem.name = "ItemFriend" + k;
                friendItem.transform.parent = parent;
                friendItem.transform.localScale = itemTemplate.localScale;

                //Agregando la posicion relativa del hijo con relacion al padre
                friendItem.transform.localPosition = new Vector3(
                    itemTemplate.localPosition.x,
                    itemTemplate.localPosition.y - 60f * k,
                    itemTemplate.localPosition.z
                );

                friendItem.transform.FindChild("Image").GetComponent<UITexture>().mainTexture = textFb2;
                friendItem.transform.FindChild("Label").GetComponent<UILabel>().text = usuario.Value;

                var menu = MenuManager.GetInstance().GetCurrentMenu() as MenuReceiveFollowingRequest;

                var button = new Button("Button" + friendItem.name);
                button.OnTouchEvent += new OnTouchEventHandler(menu.OnTouchButton);

                
                button.ObjectTag = new { follower = id, name = usuario.Value, texture = textFb2 };

                menu.GetButtonList().Add(button);

                k++;
            }
        }

        public void LocalUserFriendsPendingToFollow(long userId)
        {
            Dictionary<long, string> usuarioFriendsFacebook = new Dictionary<long, string>();

            using (var sqlite = new SQLiteService())
            {
                var query = "SELECT idFollower, nombre FROM UserFollowingNotification WHERE idUsuarioFacebook = '" + userId + "'";
                using (var reader = sqlite.SelectQuery(query))
                {
                    while (reader.Read())
                    {
                        var id = Convert.ToInt64(Convert.ToString(reader["idFollower"]));
                        var nombre = Convert.ToString(reader["nombre"]);

                        usuarioFriendsFacebook.Add(id, nombre);
                    }
                }
            }

            Transform itemTemplate = (Resources.Load("GUI/PendingFriendFollowingItem") as GameObject).transform;

            //Get Parent
            Transform parent = UIUtils.FindGUI("MenuReceiveFollowingRequest/ScrollView").transform;

            int k = 0;
            foreach (var usuario in usuarioFriendsFacebook)
            {
                var id = usuario.Key;

                var friendItem = GameObject.Instantiate(itemTemplate.gameObject) as GameObject;

                friendItem.name = "ItemFriend" + k;
                friendItem.transform.parent = parent;
                friendItem.transform.localScale = itemTemplate.localScale;

                //Agregando la posicion relativa del hijo con relacion al padre
                friendItem.transform.localPosition = new Vector3(
                    itemTemplate.localPosition.x,
                    itemTemplate.localPosition.y - 60f * k,
                    itemTemplate.localPosition.z
                );

                friendItem.transform.FindChild("Label").GetComponent<UILabel>().text = usuario.Value;

                var menu = MenuManager.GetInstance().GetCurrentMenu() as MenuReceiveFollowingRequest;

                var button = new Button(friendItem.name);
                button.OnTouchEvent += new OnTouchEventHandler(menu.OnTouchButton);

                button.ObjectTag = new { follower = id, name = usuario.Value };

                menu.GetButtonList().Add(button);

                k++;
            }
        }

        public void SendFollowingAcceptance(long idFollowed, long idFollower, string status)
        {
            WWWForm form = new WWWForm();
            form.AddField("followed", idFollowed.ToString());
            form.AddField("follower", idFollower.ToString());
            form.AddField("status", status.ToString());

            StartCoroutine(FollowingAcceptance(form));
        }

        public IEnumerator FollowingAcceptance(WWWForm form)
        {
            yield return WebService.POST("http://localhost:8080/SNCWeb/usuario/followResponse", form, (status, response) => { });
        }

        private IEnumerator CountDown(float seconds, System.Action<bool> callback)
        {
            yield return new WaitForSeconds(seconds);
            callback(true);
        }
        
        #endregion
    }
}
