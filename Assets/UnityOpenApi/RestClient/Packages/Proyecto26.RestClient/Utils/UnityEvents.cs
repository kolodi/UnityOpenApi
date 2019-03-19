using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Proyecto26
{
    [Serializable]
    public class UnityResponseHelperEvent : UnityEvent<ResponseHelper>
    {
    }

    [Serializable]
    public class UnityRequestExceptionEvent : UnityEvent<RequestException>
    {
    }

    [Serializable]
    public class UnityRequestHelperEvent : UnityEvent<RequestHelper>
    {
    }
}