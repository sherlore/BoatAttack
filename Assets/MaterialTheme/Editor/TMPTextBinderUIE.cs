using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;

[CustomEditor(typeof(TMPTextBinder)), CanEditMultipleObjects]
public class TMPTextBinderUIE : Editor
{
    public VisualTreeAsset m_UXML;
    private FloatField fontSizeField;
    private ObjectField fontAssetField;

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        // Create property container element.
        /*m_UXML.CloneTree(root);

        var typographyBinder = target as BaseTypographyBinder;

        int index = ThemeConfig.instance.typographyOptions.IndexOf(typographyBinder.key);

        if (index < 0)
        {
            index = 0;

            var csharpLabel = new Label(System.String.Format("Key({0}) doesn't exist in current theme. Use Default", typographyBinder.key));
            root.Add(csharpLabel);
        }

        var dropdownField = root.Q<DropdownField>("keyDropdown");
        dropdownField.choices = ThemeConfig.instance.typographyOptions;
        dropdownField.value = ThemeConfig.instance.typographyOptions[index];

        fontSizeField = root.Q<FloatField>("keyFontSize");
        fontSizeField.value = typographyBinder.GetTypographyAttr().fontSize;
        fontSizeField.SetEnabled(false);

        fontAssetField = root.Q<ObjectField>("keyFontAsset");
        fontAssetField.value = typographyBinder.GetTypographyAttr().fontAsset;
        fontAssetField.SetEnabled(false);

        dropdownField.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            Undo.RecordObject(typographyBinder, "Change Typography key");

            typographyBinder.key = evt.newValue;
            fontSizeField.value = typographyBinder.GetTypographyAttr().fontSize;
            fontAssetField.value = typographyBinder.GetTypographyAttr().fontAsset;
        });*/

        return root;
    }
}
