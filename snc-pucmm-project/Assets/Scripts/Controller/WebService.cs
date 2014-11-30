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

        private bool IsEnterToUpdateUserFriends { get; set; }
        private bool IsEnterToUpdateModel { get; set; }
        private bool IsEnterToUpdateTours { get; set; }
        private bool IsEnterConnectWithFacebook { get; set; }
        private bool IsUserLogIn
        {
            get
            {
                return ModelPoolManager.GetInstance().Contains("Usuario");
            }
        }
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

        public static Coroutine POST(string url, string json, System.Action<bool, string> callback)
        {
            return Instance.StartCoroutine(PostRequest(url, json, callback));
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

        private static IEnumerator PostRequest(string url, string json, System.Action<bool, string> callback)
        {
            var form = new WWWForm();
            form.AddField("json", json);

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
            IsEnterToUpdateUserFriends = true;
            IsEnterConnectWithFacebook = true;
        }

        void Update()
        {
            //Mientras no haya conexion a internet
            //if (IsEnterToUpdateModel)
            //{
            //    IsEnterToUpdateModel = false;
            //    StartCoroutine(UpdateModelService());
            //}

            //if (IsUserLogIn && IsEnterToUpdateTours)
            //{
            //    IsEnterToUpdateTours = false;
            //    StartCoroutine(UpdateToursService());
            //}

            if (IsEnterConnectWithFacebook)
            {
                IsEnterConnectWithFacebook = false;
                StartCoroutine(ConnectWithFacebook());
            }
        }

        private IEnumerator UpdateModelService()
        {
            bool IsConnectionAvailable = false;
            string responseJson = string.Empty;

            //Obtener la ultima actualizacion del servidor
            yield return WebService.GET(
                "http://localhost:8080/", 
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

            //Salir de la corotutina
            yield return null;
        }

        private IEnumerator UpdateToursService()
        {
            bool isConnectionAvailable = false, accessControlWebService = false;
            string responseJson = string.Empty;
            //JSONObject json = SQLiteService.GetInstance().DataSynchronization();

            //Obtener la ultima actualizacion del servidor
            yield return WebService.GET(
                "http://localhost:8080/SNCWeb/tour/list",
                (status, response) =>
                {
                    if (status) { responseJson = response; }
                    isConnectionAvailable = status;
                    accessControlWebService = true;
                }
            );

            //Esperar que se refresque el valores
            yield return new WaitForSeconds(0.5f);

            if (accessControlWebService)
            {
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

            }

            if (!IsEnterToUpdateTours)
            {
                StartCoroutine(CountDown(600, value => IsEnterToUpdateTours = value));
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
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
                json = new JSONObject();
                json.AddField("id", FB.UserId);

                var IsConnectionAvailableServer = false;
                responseJson = string.Empty;
                yield return WebService.POST("http://localhost:8080/SNCWeb/usuario/following",json.ToString(),
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
                    Transform parent = UIUtils.FindGUI("MenuUsuarioFollowing/ScrollView").transform;

                    UIUtils.DestroyChilds("MenuUsuarioFollowing/ScrollView", true);

                    int k = 0;
                    foreach(var usuario in usuarioFriendsFacebook)
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
                        menu.GetCheckBoxList().Add(checkBox);                        

                        friendItem.GetComponent<BoxCollider>().enabled = true;

                        k++;
                    }
                }
            }
        }

        private IEnumerator SendUserFacebookId(Usuario user)
        {
            yield return WebService.POST("http://localhost:8080/SNCWeb/usuario/crear", user.ToJson().ToString(), (status, response) => { });
        }

        public void GetUsuario()
        {
            StartCoroutine(GetUsuarioFacebook());
        }

        private IEnumerator GetUsuarioFacebook()
        {
            bool IsConnectionAvailable = false;
            string responseJson = string.Empty;

            yield return WebService.GET("https://graph.facebook.com/me?access_token=" + FB.AccessToken,
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
                }

               //StartCoroutine(SendUserFacebookId(user));
                yield return StartCoroutine(GetUserInfo(75, 75));
            }
        }

        public void GetUserInfoRequest(int width, int height)
        {
            StartCoroutine(GetUserInfo(width, height));
        }

        private IEnumerator GetUserInfo(int width, int height)
        {
            yield return new WaitForSeconds(0.25f);

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

        private IEnumerator CountDown(float seconds, System.Action<bool> callback)
        {
            //Esperar 10 min
            yield return new WaitForSeconds(seconds);
            callback(true);
        }
        
        #endregion
    }
}
