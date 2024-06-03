using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(MaterialColor))]
public class MaterialColorUIE : PropertyDrawer
{	
	#if true
	
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        // Create property fields
        var keyField = new PropertyField(property.FindPropertyRelative("key"), "");
        var colorField = new PropertyField(property.FindPropertyRelative("color"), "");

        // Add fields to the container.
        container.Add(keyField);
        container.Add(colorField);

        return container;
    }
	
	#endif
}
