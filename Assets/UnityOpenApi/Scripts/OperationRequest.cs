using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityOpenApi
{

    public class OperationRequest
    {
        public UploadHandler UploadHandler { get; set; }
        public DownloadHandler DownloadHandler { get; set; }
        internal Operation Operation { get; set; }
        
        public string Text
        {
            get
            {
                return string.Empty;
            }
        }

        public Promise Promise = new Promise();


    }
}