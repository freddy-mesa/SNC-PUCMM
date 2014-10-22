using UnityEngine;
using System.Collections;
using System.IO;

namespace HTTP
{
    public class WebCache
    {
        public string root;

        public WebCache() {
            root = Application.persistentDataPath;
        }

        public void Delete(string filename) {
            var path = Path.Combine(root, filename);
            File.Delete(path);
        }

        public void Download(string filename, string url) {
            var req = new HTTP.Request("HEAD", url);
            req.Send(delegate(Request headreq) {
                if(headreq.exception == null) {
                    if(headreq.response.status == 200) {
                        Debug.Log(headreq.response.headers.Get("Content-Length"));
                    }
                } else {
                    Debug.Log(headreq.exception);
                }
            });
        }

        void RecvHead(Request req) {

            //if(req.response.status == 
            /*var path = Path.Combine(root, filename);
            Directory.CreateDirectory(path);
            using (var fs = System.IO.File.Create(path)) {

            }*/
        }




    }

}
