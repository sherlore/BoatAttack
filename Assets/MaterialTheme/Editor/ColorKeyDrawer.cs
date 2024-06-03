using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(ColorKeyAttribute))]
public class ColorKeyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        //var keyField = new PropertyField(property);
        //container.Add(keyField);

        string colorKey = property.stringValue;

        int index = ThemeConfig.instance.colorOptions.IndexOf(colorKey);

        if (index < 0)
        {
            index = 0;

            var csharpLabel = new Label(System.String.Format("Key({0}) doesn't exist in current theme. Use Default", colorKey));
            container.Add(csharpLabel);
        }

        var dropdownField = new DropdownField("Key", ThemeConfig.instance.colorOptions, ThemeConfig.instance.colorOptions[index]);
        
        dropdownField.BindProperty(property);
        dropdownField.AddToClassList(BaseField<string>.alignedFieldUssClassName);

        var colorField = new ColorField("Color");
        colorField.value = GetColor(colorKey);
        colorField.SetEnabled(false);
        colorField.AddToClassList(BaseField<string>.alignedFieldUssClassName);

        dropdownField.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            colorField.value = GetColor(evt.newValue);
        });

        container.Add(dropdownField);
        container.Add(colorField);

        return container;
    }


    public Color GetColor(string colorKey)
    {
        if (ThemeConfig.instance.colorDic.ContainsKey(colorKey))
        {
            return ThemeConfig.instance.colorDic[colorKey];
        }
        else
        {
            Debug.LogWarning(System.String.Format("key({0}) cannot found color in theme", colorKey));
            return Color.white;
        }
    }
}
