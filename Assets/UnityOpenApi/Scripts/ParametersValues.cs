using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityOpenApi
{
    [Serializable]
    public class ParameterValue
    {
        [HideInInspector]
        public OAParameter parameter;
        public string value;
        public bool HasValue
        {
            get
            {
                return string.IsNullOrEmpty(value) == false;
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ParameterValue))]
    public class ParameterValueEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //OAParameter parameter = property.FindPropertyRelative("parameter");
            var parNameRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            var parValueRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - (EditorGUIUtility.labelWidth), position.height);
            EditorGUI.LabelField(parNameRect, property.FindPropertyRelative("parameter.Name").stringValue);
            EditorGUI.PropertyField(parValueRect, property.FindPropertyRelative("value"), GUIContent.none);
        }
    }
#endif
}
