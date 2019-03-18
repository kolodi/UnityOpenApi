using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityFloatEvent : UnityEvent<float>
{
}

[Serializable]
public class UnityIntEvent : UnityEvent<int>
{
}

[Serializable]
public class UnityStringEvent : UnityEvent<string>
{
}

[Serializable]
public class UnityBoolEvent : UnityEvent<bool>
{
}

[Serializable]
public class UnityGOEvent : UnityEvent<GameObject>
{
}
