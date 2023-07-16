using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// credit: @mstruzyna
/// pulled from https://gist.github.com/deebrol/02f61b7611fd4eca923776077b92dfc2
/// </summary>
public static class SerializedPropertyExt
{
    public static SerializedProperty GetParent(this SerializedProperty aProperty)
    {
        var path = aProperty.propertyPath;
        int i = path.LastIndexOf('.');
        if (i < 0)
            return null;
        return aProperty.serializedObject.FindProperty(path.Substring(0, i));
    }
    public static SerializedProperty FindSiblingProperty(this SerializedProperty aProperty, string aPath)
    {
        var parent = aProperty.GetParent();
        if (parent == null)
            return aProperty.serializedObject.FindProperty(aPath);
        return parent.FindPropertyRelative(aPath);
    }
}