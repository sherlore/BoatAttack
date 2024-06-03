using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(ColorEventBinder)), CanEditMultipleObjects]
public class ColorEventBinderUIE : Editor
{
    public VisualTreeAsset m_UXML;
    private ColorField colorField;

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        // Create property container element.
        /*m_UXML.CloneTree(root);

        var colorBinder = target as BaseColorBinder;

        int index = ThemeConfig.instance.colorOptions.IndexOf(colorBinder.key);

        if (index < 0)
        {
            index = 0;

            var csharpLabel = new Label(System.String.Format("Key({0}) doesn't exist in current theme. Use Default", colorBinder.key));
            root.Add(csharpLabel);
        }

        var dropdownField = root.Q<DropdownField>("keyDropdown");
        dropdownField.choices = ThemeConfig.instance.colorOptions;
        dropdownField.value = ThemeConfig.instance.colorOptions[index];

        colorField = root.Q<ColorField>("keyColor");
        colorField.value = colorBinder.GetColor();
        colorField.SetEnabled(false);

        dropdownField.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            Undo.RecordObject(colorBinder, "Change color key");

            colorBinder.key = evt.newValue;
            colorField.value = colorBinder.GetColor();
        });*/

        return root;
    }
}

