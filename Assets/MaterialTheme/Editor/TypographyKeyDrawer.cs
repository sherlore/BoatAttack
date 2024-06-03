using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
using static UnityEditor.Recorder.OutputPath;

[CustomPropertyDrawer(typeof(TypographyKeyAttribute))]
public class TypographyKeyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        //var keyField = new PropertyField(property);
        //container.Add(keyField);

        string typographyKey = property.stringValue;

        int index = ThemeConfig.instance.typographyOptions.IndexOf(typographyKey);

        if (index < 0)
        {
            index = 0;

            var csharpLabel = new Label(System.String.Format("Key({0}) doesn't exist in current theme. Use Default", typographyKey));
            container.Add(csharpLabel);
        }

        var dropdownField = new DropdownField("Key", ThemeConfig.instance.typographyOptions, ThemeConfig.instance.typographyOptions[index]);

        dropdownField.BindProperty(property);
        dropdownField.AddToClassList(BaseField<string>.alignedFieldUssClassName);


        var fontSizeField = new FloatField("keyFontSize");
        fontSizeField.value = GetTypographyAttr(typographyKey).fontSize;
        fontSizeField.SetEnabled(false);
        fontSizeField.AddToClassList(BaseField<string>.alignedFieldUssClassName);


        var fontAssetField = new ObjectField("keyFontAsset");
        fontAssetField.value = GetTypographyAttr(typographyKey).fontAsset;
        fontAssetField.SetEnabled(false);
        fontAssetField.AddToClassList(BaseField<string>.alignedFieldUssClassName);

        dropdownField.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            //property.stringValue = evt.newValue;
            fontSizeField.value = GetTypographyAttr(evt.newValue).fontSize;
            fontAssetField.value = GetTypographyAttr(evt.newValue).fontAsset;
        });

        container.Add(dropdownField);
        container.Add(fontSizeField);
        container.Add(fontAssetField);

        return container;
    }
    public MaterialTypographyAttr GetTypographyAttr(string key)
    {
        if (ThemeConfig.instance.typographyDic.ContainsKey(key))
        {
            return ThemeConfig.instance.typographyDic[key];
        }
        else
        {
            Debug.LogWarning(System.String.Format("key({0}) cannot found typography in theme", key));
            return ThemeConfig.instance.defaultTypographyAttr;
        }
    }
}
