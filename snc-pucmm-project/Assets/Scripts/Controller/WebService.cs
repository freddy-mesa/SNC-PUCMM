using SncPucmm.Controller.Control;
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
        //private const string SERVER_URL = "http://23.88.105.3:9001";
        private const string SERVER_URL = "http://localhost:8080/SNCWeb/";
        
        #endregion

        #region Propiedades

        private bool IsEnterToUpdateModel { get; set; }
        private bool IsEnterToUpdateTours { get; set; }
        private bool IsEnterConnectWithFacebook { get; set; }
        private bool IsEnterPendingFollowingNotification { get; set; }
        private bool IsEnterSharedLocationNotification { get; set; }
        
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

        #region GET & POST Methods
        
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

        #endregion

        void Start()
        {
            IsEnterToUpdateModel = true;
            IsEnterToUpdateTours = true;
            IsEnterPendingFollowingNotification = true;
            IsEnterConnectWithFacebook = true;
            IsEnterSharedLocationNotification = true;
        }

        void Update()
        {
            if (!FB.IsInitialized && IsEnterConnectWithFacebook)
            {
                IsEnterConnectWithFacebook = false;
                StartCoroutine(ConnectWithFacebook());
            }

            //if (IsEnterToUpdateModel)
            //{
            //    IsEnterToUpdateModel = false;
            //    StartCoroutine(UpdateModelService());
            //}

            if (FB.IsInitialized && FB.IsLoggedIn && IsEnterToUpdateTours)
            {
                IsEnterToUpdateTours = false;
                StartCoroutine(UpdateToursService());
            }

            if (FB.IsInitialized && FB.IsLoggedIn && IsEnterSharedLocationNotification)
            {
                IsEnterSharedLocationNotification = false;
                StartCoroutine(ReceiveSharedFriendLocatioNotification());
            }

            if (FB.IsInitialized && FB.IsLoggedIn && IsEnterPendingFollowingNotification)
            {
                IsEnterPendingFollowingNotification = false;
                StartCoroutine(ReceiveFollowingRequest());
            }
        }

        #region Update Model & Tour
        
        private IEnumerator UpdateModelService()
        {
            bool IsConnectionAvailable = false;
            string responseJson = string.Empty;

            //Obtener la ultima actualizacion del servidor
            yield return WebService.GET(
                SERVER_URL + "/nodo/actualizar", 
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
            yield return new WaitForSeconds(1f);

            JSONObject json;

            using (var sqlite = new SQLiteService())
            {
                json = sqlite.DataSynchronization();
            }

            WWWForm form = new WWWForm();
            form.AddField("id", FB.UserId);
            form.AddField("usuarioTourList", json.GetField("UsuarioTourList").ToString());
            form.AddField("usuarioLocalizacionList", json.GetField("UsuarioLocalizacionList").ToString());

            yield return WebService.POST(
                SERVER_URL + "/tour/updateSubscriber",
                form,
                (status, response) =>
                {
                    if (status) 
                    {
                        Debug.Log("Peticion updateSubscriber -> " + response);

                        json = new JSONObject(response);

                        //Hacer un update a la base de datos
                        //using (var sqlService = new SQLiteService())
                        //{
                        //    sqlService.UpdateTours(json);
                        //} 
                    }
                    
                }
            );

            if (!IsEnterToUpdateTours)
            {
                StartCoroutine(CountDown(600, value => IsEnterToUpdateTours = value));
                yield return new WaitForSeconds(0.5f);
            }
        }

        #endregion

        #region Connect with Facebook API
        
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

        #endregion

        #region Get User Information (Login)

        public void GetUsuarioInfo()
        {
            FB.API("/me", Facebook.HttpMethod.GET, GetUsuarioInfoResult);
        }

        private void GetUsuarioInfoResult(FBResult result)
        {
            if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.LogError(result.Error);
            }
            else
            {
                var json = new JSONObject(result.Text);
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
                                if (int.TryParse(Convert.ToString(reader["id"]), out temp))
                                {
                                    id = temp;
                                }
                            }

                            id++;
                            sqliteService.TransactionalQuery(
                                "INSERT INTO Usuario VALUES (" + id + ",'" + user.idUsuarioFacebook + "','" + user.nombre + "','" + user.apellido + "','" + user.email + "','" + user.gender + "');"
                            );
                        }
                    }

                    FB.API("/me/picture?width=128&height=128", Facebook.HttpMethod.GET, GetUsuarioInfoPhotoResult);
                    StartCoroutine(SendUserFacebookId(user));
                }
            }
        }

        private void GetUsuarioInfoPhotoResult(FBResult result)
        {
            if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.LogError(result.Error);
            }
            else
            {
                var logeado = UIUtils.FindGUI("MenuMain/Sidebar/ButtonUsuario").transform.FindChild("Logeado");
                logeado.GetComponent<UITexture>().mainTexture = result.Texture;

                using (var sqliteService = new SQLiteService())
                {
                    using (var reader = sqliteService.SelectQuery("SELECT nombre, apellido FROM Usuario WHERE idUsuarioFacebook = " + FB.UserId))
                    {
                        if (reader.Read())
                        {
                            logeado.transform.FindChild("Label").GetComponent<UILabel>().text = Convert.ToString(reader["nombre"]) + " " + Convert.ToString(reader["apellido"]);
                        }
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

            Debug.Log(form.ToString());

            yield return WebService.POST(SERVER_URL + "/usuario/crear", form, (status, response) => { });
        }

        #endregion

        #region Follow
        
        #region Get/Send Friend List to Follow

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
                    if (status)
                    {
                        IsConnectionAvailable = status;
                        responseJson = response;
                    }
                }
            );

            //Esperar que se refresque el valores
            yield return new WaitForSeconds(0.5f);

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

                        //if (Application.platform == RuntimePlatform.WindowsEditor)
                        //{
                        //    StartCoroutine(SendUserFacebookId(new Usuario() { idUsuarioFacebook = id, apellido = item.GetField("last_name").str, nombre = item.GetField("fist_name").str }));
                        //}
                    }
                }

                //Filtrar usuario que no han sido enviadas
                WWWForm form = new WWWForm();
                form.AddField("id", FB.UserId);

                yield return WebService.POST(SERVER_URL + "/usuario/following", form,
                    (status, response) =>
                    {
                        if (status)
                        {
                            var data = new JSONObject(response);
                            for (int i = 0; i < data.list.Count; ++i)
                            {
                                var idFollowing = Convert.ToInt64(data.list[i].GetField("id").str);

                                if (usuarioFriendsFacebook.ContainsKey(idFollowing))
                                {
                                    usuarioFriendsFacebook.Remove(idFollowing);
                                }
                            }
                        }
                    }
                );

                yield return new WaitForSeconds(0.5f);

                //Get Item Template
                Transform itemTemplate = (Resources.Load("GUI/FriendFollowingItem") as GameObject).transform;

                //Get Parent
                Transform parent = UIUtils.FindGUI("MenuSendFollowingRequest/ScrollView").transform;

                int k = 0;
                foreach (var usuario in usuarioFriendsFacebook)
                {
                    var id = usuario.Key;

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

        public void SendFollowingRequest(List<long> userToFollowed)
        {
            JSONObject jsonArray = new JSONObject(JSONObject.Type.ARRAY);
            foreach (var follow in userToFollowed)
            {
                jsonArray.Add(follow.ToString());
            }

            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("id", FB.UserId);
            json.AddField("usuarios", jsonArray);

            WWWForm form = new WWWForm();
            form.AddField("json", json.ToString());

            StartCoroutine(FollowingRequest(form));
        }

        private IEnumerator FollowingRequest(WWWForm form)
        {
            yield return WebService.POST(SERVER_URL + "/usuario/followRequest", form, (status, response) => { });
        }

        #endregion

        #region Get/Send Following Notication Request

        public IEnumerator ReceiveFollowingRequest()
        {
            var idUser = "";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                idUser = "10152587482388668";
            }
            else
            {
                idUser = FB.UserId;
            }
            
            WWWForm form = new WWWForm();
            form.AddField("id", idUser);

            yield return WebService.POST(SERVER_URL + "/usuario/notifyFollowingRequest", form, (status, response) => 
            {
                if (status)
                {
                    Debug.Log("notifyFollowingRequest -> " + response);
                    var json = new JSONObject(response);

                    if (json.list.Count > 0)
                    {
                        using (var sqlite = new SQLiteService())
                        {
                            Dictionary<long, string> notificationToSave = new Dictionary<long, string>();
                            foreach (var notification in json.list)
                            {
                                var idNotification = Convert.ToInt64(notification.GetField("id").str);
                                var nombre = notification.GetField("name").str;
                                notificationToSave.Add(idNotification, nombre);

                                Debug.Log("Request for FOLLOW from: " + idNotification);
                            }

                            //Limpiar la tabla de notificaciones
                            sqlite.TransactionalQuery("DELETE FROM UserFollowingNotification");

                            //Insertar en la base de datos
                            int id = 0;
                            var queryBuilder = new StringBuilder();

                            foreach (var notification in notificationToSave)
                            {
                                id++;
                                queryBuilder.Append("INSERT INTO UserFollowingNotification VALUES (" + id + ",'" + idUser + "','" + notification.Key + "','" + notification.Value + "');");
                            }

                            sqlite.TransactionalQuery(queryBuilder.ToString());

                            if (UIUtils.notificationPendingFollowCount == id)
                            {
                                UIUtils.PushFollowingNotification(false);
                            }
                            else
                            {
                                UIUtils.PushFollowingNotification(true);
                            }
                        }
                    }
                }
            });

            if (!IsEnterPendingFollowingNotification)
            {
                StartCoroutine(CountDown(10f, value => IsEnterPendingFollowingNotification = value));
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void GetUserFriendsPendingToFollow()
        {
            StartCoroutine(UserFriendsPendingToFollow());
        }

        public IEnumerator UserFriendsPendingToFollow()
        {
            var idUser = "";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                idUser = "10152587482388668";
            }
            else
            {
                idUser = FB.UserId;
            }

            Dictionary<long, string> usuarioFriendsFacebook = new Dictionary<long, string>();

            using (var sqlite = new SQLiteService())
            {
                var query = "SELECT idFollower, nombre FROM UserFollowingNotification WHERE idUsuarioFacebook = '" + idUser + "'";
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
                var url = ("https://graph.facebook.com/" + id + "/picture?width=128&height=128&access_token=" + FB.AccessToken);
                WWW photo = new WWW(url);
                Texture2D textFb2 = new Texture2D(128, 128, TextureFormat.DXT5, false); //TextureFormat must be DXT5

                yield return photo;

                photo.LoadImageIntoTexture(textFb2);

                var friendItem = GameObject.Instantiate(itemTemplate.gameObject) as GameObject;

                friendItem.name = "ItemFriend" + k;
                friendItem.transform.parent = parent;
                friendItem.transform.localScale = itemTemplate.localScale;

                //Agregando la posicion relativa del hijo con relacion al padre
                friendItem.transform.localPosition = new Vector3(
                    itemTemplate.localPosition.x,
                    itemTemplate.position.y - 60f * k,
                    itemTemplate.localPosition.z
                );

                friendItem.transform.FindChild("Image").GetComponent<UITexture>().mainTexture = textFb2;
                friendItem.transform.FindChild("Label").GetComponent<UILabel>().text = usuario.Value;

                var menu = MenuManager.GetInstance().GetCurrentMenu() as MenuReceiveFollowingRequest;

                var button = new Button(friendItem.name);
                button.OnTouchEvent += new OnTouchEventHandler(menu.OnTouchButton);


                button.ObjectTag = new { follower = id, name = usuario.Value, texture = textFb2, index = k };

                menu.GetButtonList().Add(button);

                k++;
            }
        }
        
        public void SendFollowingAcceptance(long idFollower, string status)
        {
            StartCoroutine(FollowingAcceptance(idFollower,status));
        }

        public IEnumerator FollowingAcceptance(long idFollower, string acceptanceStatus)
        {
            WWWForm form = new WWWForm();
            var idUser = "";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                idUser = "10152587482388668";
            }
            else
            {
                idUser = FB.UserId;
            }

            form.AddField("followed", idUser);
            form.AddField("follower", idFollower.ToString());
            form.AddField("status", acceptanceStatus);

            yield return WebService.POST(SERVER_URL + "/usuario/followResponse", form, 
                (status, response) => 
                {
                    if (status)
                    {
                        using (var sqlite = new SQLiteService())
                        {
                            sqlite.TransactionalQuery("DELETE FROM UserFollowingNotification WHERE idUsuarioFacebook = '" + idUser + "' and idFollower = '" + idFollower + "'");
                        }
                    }
                }
            );
        }

        #endregion

        #endregion

        #region FindFriends (Followed/Follower) Show in Model3D

        public void GetFriends()
        {
            StartCoroutine(Friends());
        }

        private IEnumerator Friends()
        {
            WWWForm form = new WWWForm();
            form.AddField("id", FB.UserId);

            Dictionary<long, object> followedFriends = new Dictionary<long,object>();

            yield return WebService.POST(SERVER_URL + "/usuario/friendFindRequest", form,
                (status, response) =>
                {
                    if (status) 
                    {
                        Debug.Log("friendFindRequest -> " + response);
                        var data = new JSONObject(response);

                        if (data.list.Count > 0)
                        {
                            for (int i = 0; i < data.list.Count; ++i)
                            {
                                var followed = Convert.ToInt64(data.list[i].GetField("id").str);
                                var nombre = Convert.ToString(data.list[i].GetField("nombre").str);
                                var ubicacion = Convert.ToString(data.list[i].GetField("ubicacion").str);
                                var fecha = Convert.ToString(data.list[i].GetField("fecha").str);

                                var obj = new { nombre, ubicacion, fecha };

                                followedFriends.Add(followed, obj);
                            }
                        }
                    }
                }
            );

            yield return new WaitForEndOfFrame();

            Transform itemTemplate = (Resources.Load("GUI/PendingFriendFollowingItem") as GameObject).transform;

            Transform parent = UIUtils.FindGUI("MenuFindFriendSelection/ScrollView").transform;

            int k = 0;
            foreach (var usuario in followedFriends)
            {
                var id = usuario.Key;

                WWW photo = new WWW("https://graph.facebook.com/" + id + "/picture?width=250&height=250"); //?access_token=" + FB.AccessToken);
                Texture2D texture = new Texture2D(250, 250, TextureFormat.DXT5, false); //TextureFormat must be DXT5

                yield return photo;

                photo.LoadImageIntoTexture(texture);

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

                var nombre = Convert.ToString(usuario.Value.GetType().GetProperty("nombre").GetValue(usuario.Value, null));
                var ubicacion = Convert.ToString(usuario.Value.GetType().GetProperty("ubicacion").GetValue(usuario.Value, null));
                var fecha = Convert.ToString(usuario.Value.GetType().GetProperty("fecha").GetValue(usuario.Value, null));

                friendItem.transform.FindChild("Image").GetComponent<UITexture>().mainTexture = texture;
                friendItem.transform.FindChild("Label").GetComponent<UILabel>().text = nombre;

                var menu = MenuManager.GetInstance().GetCurrentMenu() as MenuFindFriendSelection;

                var button = new Button(friendItem.name);
                button.ObjectTag = new { ubicacion, nombre, texture, fecha };
                button.OnTouchEvent += new OnTouchEventHandler(menu.OnTouchButton);
                menu.GetButtonList().Add(button);

                k++;
            }

        }

        #endregion

        #region Share Location
        
        #region Get/Send Friends List To Share Location

        public void GetFriendsToShareLocation()
        {
            StartCoroutine(FriendsToShareLocation());
        }

        private IEnumerator FriendsToShareLocation()
        {
            WWWForm form = new WWWForm();
            form.AddField("id", FB.UserId);

            Dictionary<long, string> friends = new Dictionary<long, string>();

            yield return WebService.POST(SERVER_URL + "/usuario/friendsShareLocationRequest", form,
                (status, response) =>
                {
                    if (status)
                    {
                        var json = new JSONObject(response);
                        if(json.list != null)
                        {
                            foreach (var item in json.list)
                            {
                                var id = Convert.ToInt64(item.GetField("id").str);
                                var nombre = item.GetField("nombre").str;

                                friends.Add(id, nombre);
                            }
                        }
                    }
                }
            );

            //Esperar que se refresque el valores
            yield return new WaitForEndOfFrame();

            //Get Item Template
            Transform itemTemplate = (Resources.Load("GUI/FriendShareLocationItem") as GameObject).transform;

            //Get Parent
            Transform parent = UIUtils.FindGUI("MenuShareLocationFriendSelection/ScrollView").transform;

            int k = 0;
            foreach (var usuario in friends)
            {
                var id = usuario.Key;

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


                var menu = MenuManager.GetInstance().GetCurrentMenu() as MenuShareLocationFriendSelection;

                var checkBox = new CheckBox(checkBoxGameObject.name);
                checkBox.OnChangeEvent += new OnChangeEventHandler(menu.OnChangeCheckBox);

                checkBox.ObjectTag = new { idUsuario = id };

                menu.GetCheckBoxList().Add(checkBox);

                friendItem.GetComponent<BoxCollider>().enabled = true;

                k++;
            }
        }

        public void SendShareLocationRequest(List<long> friendList, string message, int idNodo)
        {
            JSONObject jsonArray = new JSONObject(JSONObject.Type.ARRAY);
            foreach (var follow in friendList)
            {
                jsonArray.Add(follow.ToString());
            }

            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("friends", jsonArray);
            json.AddField("message", message);
            json.AddField("idNodo", idNodo);

            WWWForm form = new WWWForm();
            form.AddField("id", FB.UserId);
            form.AddField("json", json.ToString());

            StartCoroutine(ShareLocationRequest(form));
        }

        private IEnumerator ShareLocationRequest(WWWForm form)
        {
            yield return WebService.POST(SERVER_URL + "/usuario/sharedLocationRequest", form, (status, response) => { });
        }

        #endregion

        #region Get/Send Shared Localization Notification

        private IEnumerator ReceiveSharedFriendLocatioNotification()
        {
            var idUser = "";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                idUser = "10152587482388668";
            }
            else
            {
                idUser = FB.UserId;
            }

            WWWForm form = new WWWForm();
            form.AddField("id", idUser);

            yield return WebService.POST(SERVER_URL + "/usuario/notifySharedLocationRequest", form, (status, response) =>
            {
                if (status)
                {
                    Debug.Log("notifySharedLocationRequest -> " + response);
                    var json = new JSONObject(response);

                    if (json.list.Count > 0)
                    {
                        using (var sqlite = new SQLiteService())
                        {
                            Dictionary<long, object> notificationToSave = new Dictionary<long, object>();
                            foreach (var notification in json.list)
                            {
                                var idUsuario = Convert.ToInt64(notification.GetField("id").str);
                                var nombre = notification.GetField("nombre").str;
                                var nodo = Convert.ToInt32(notification.GetField("nodo").n);
                                var mensaje = notification.GetField("mensaje").str;

                                notificationToSave.Add(idUsuario, new { nombre, nodo, mensaje });

                                Debug.Log("Shared Location Request from: " + idUsuario);
                            }

                            //Limpiar la tabla de notificaciones
                            sqlite.TransactionalQuery("DELETE FROM UserSharedLocationNotification");

                            //Insertar en la base de datos
                            int id = 0;
                            var queryBuilder = new StringBuilder();

                            foreach (var notification in notificationToSave)
                            {
                                id++;

                                var nodo = Convert.ToInt32(notification.Value.GetType().GetProperty("nodo").GetValue(notification.Value, null));
                                var nombre = Convert.ToString(notification.Value.GetType().GetProperty("nombre").GetValue(notification.Value, null));
                                var mensaje = Convert.ToString(notification.Value.GetType().GetProperty("mensaje").GetValue(notification.Value, null));

                                queryBuilder.Append("INSERT INTO UserSharedLocationNotification VALUES (" + id + ",'" + idUser + "','" + notification.Key + "','" + nombre + "'," + nodo + ",'" + mensaje + "');");
                            }

                            sqlite.TransactionalQuery(queryBuilder.ToString());

                            //if (UIUtils.notificationPendingFollowCount == id)
                            //{
                            //    UIUtils.PushSharedLocationNotification(false);
                            //}
                            //else
                            //{
                            //    UIUtils.PushSharedLocationNotification(true);
                            //}
                        }
                    }
                }
            });

            if (!IsEnterSharedLocationNotification)
            {
                StartCoroutine(CountDown(10f, value => IsEnterSharedLocationNotification = value));
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void GetSharedFriendLocationNotification()
        {
            StartCoroutine(SharedFriendLocationNotification());
        }

        public IEnumerator SharedFriendLocationNotification()
        {
            var idUser = "";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                idUser = "10152587482388668";
            }
            else
            {
                idUser = FB.UserId;
            }

            Dictionary<long, object> usuarioFriendsFacebook = new Dictionary<long, object>();
            using (var sqlite = new SQLiteService())
            {
                var query = "SELECT idFriend, nombre, idNodo, mensaje FROM UserSharedLocationNotification WHERE idUsuarioFacebook = '" + idUser + "'";
                using (var reader = sqlite.SelectQuery(query))
                {
                    while (reader.Read())
                    {
                        var id = Convert.ToInt64(Convert.ToString(reader["idFriend"]));
                        var nombre = Convert.ToString(reader["nombre"]);
                        var nodo = Convert.ToInt32(reader["idNodo"]);
                        var mensaje = Convert.ToString(reader["mensaje"]);

                        usuarioFriendsFacebook.Add(id, new { nombre, nodo, mensaje });
                    }
                }
            }

            Transform itemTemplate = (Resources.Load("GUI/PendingFriendFollowingItem") as GameObject).transform;

            //Get Parent
            Transform parent = UIUtils.FindGUI("MenuReceiveShareLocationRequest/ScrollView").transform;

            int k = 0;
            foreach (var usuario in usuarioFriendsFacebook)
            {
                var id = usuario.Key;
                var url = ("https://graph.facebook.com/" + id + "/picture?width=50&height=50&access_token=" + FB.AccessToken);
                WWW photo = new WWW(url);
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

                var nombre = Convert.ToString(usuario.Value.GetType().GetProperty("nombre").GetValue(usuario.Value, null));
                var nodo = Convert.ToInt32(usuario.Value.GetType().GetProperty("nodo").GetValue(usuario.Value, null));
                var mensaje = Convert.ToString(usuario.Value.GetType().GetProperty("mensaje").GetValue(usuario.Value, null));
                

                friendItem.transform.FindChild("Image").GetComponent<UITexture>().mainTexture = textFb2;
                friendItem.transform.FindChild("Label").GetComponent<UILabel>().text = nombre;

                var menu = MenuManager.GetInstance().GetCurrentMenu() as MenuReceiveShareLocationRequest;

                var button = new Button(friendItem.name);
                button.OnTouchEvent += new OnTouchEventHandler(menu.OnTouchButton);

                button.ObjectTag = new { index = k, nodo, mensaje };

                menu.GetButtonList().Add(button);

                k++;
            }
        }

        public void SendSharedFriendsLocationNotification(object message)
        {
            StartCoroutine(SendSharedFriendsLocation(message));
        }

        private IEnumerator SendSharedFriendsLocation(object message)
        {
            WWWForm form = new WWWForm();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                form.AddField("id", "10152587482388668");
            }
            else
            {
                form.AddField("id", FB.UserId);
            }

            var friends = Convert.ToString(message.GetType().GetProperty("friends").GetValue(message, null));
            form.AddField("friends", friends);

            var mensaje = Convert.ToString(message.GetType().GetProperty("message").GetValue(message, null));
            form.AddField("message", mensaje);

            var nodo = Convert.ToInt32(message.GetType().GetProperty("nodo").GetValue(message, null));
            form.AddField("idNodo", nodo);

            yield return WebService.POST(SERVER_URL + "/usuario/shareLocation", form, (status, response) => { });
        }
        
        #endregion

        #endregion

        private IEnumerator CountDown(float seconds, System.Action<bool> callback)
        {
            yield return new WaitForSeconds(seconds);
            callback(true);
        }
        
        #endregion
    }
}