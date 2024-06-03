using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.AssetImporters;

[CustomEditor(typeof(GraphicColorBinder)), CanEditMultipleObjects]
public class GraphicColorBinderUIE : Editor
{
    public VisualTreeAsset m_UXML;
	private ColorField colorField;
	
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
		InspectorElement.FillDefaultInspector(root, serializedObject, this);

        //root.Add(new PropertyField(serializedObject.FindProperty("graphic")));
        //root.Add(new PropertyField(serializedObject.FindProperty("m_key")));

        return root;
    }
}
