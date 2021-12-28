using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(ObtainedFromAttribute))]
public class ObtainedFromDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ObtainedFromAttribute obtainedFrom = (ObtainedFromAttribute)attribute;
        if (property.serializedObject.targetObject is Item item)
        {
            if (item.GetObtainedFrom() != obtainedFrom.GetObtainedFrom()) return 0;
        }
        else if (property.serializedObject.targetObject is Spaceship spaceship)
        {
            if (spaceship.GetObtainedFrom() != obtainedFrom.GetObtainedFrom()) return 0;
        }
        return EditorGUI.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ObtainedFromAttribute obtainedFrom = (ObtainedFromAttribute)attribute;
        if (property.serializedObject.targetObject is Item item)
        {
            if (item.GetObtainedFrom() == obtainedFrom.GetObtainedFrom()) EditorGUI.PropertyField(position, property, label);
        }
        else if(property.serializedObject.targetObject is Spaceship spaceship)
        {
            if (spaceship.GetObtainedFrom() == obtainedFrom.GetObtainedFrom()) EditorGUI.PropertyField(position, property, label);
        }
    }
}
