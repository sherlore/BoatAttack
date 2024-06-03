using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(ThemeBuilder))]
public class ThemeBuilderUIE : Editor
{
	private ThemeBuilder themeBuilder;
	
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
		InspectorElement.FillDefaultInspector(root, serializedObject, this);
		
		themeBuilder = target as ThemeBuilder;
		
		var themeButton = new Button(() =>
		{
			UpdateTheme(themeBuilder.darkColorPaletteJson, themeBuilder.themeFile.darkTheme);
			UpdateTheme(themeBuilder.lightColorPaletteJson, themeBuilder.themeFile.lightTheme);
			
            EditorUtility.SetDirty(themeBuilder.themeFile);
		});
		themeButton.text = "Update Theme";
		root.Add(themeButton);
				
        return root;
	}
	
	public void UpdateTheme(string val, Theme theme)
	{
		// "primary": "#68548E","surfaceTint": "#68548E",
		
		string[] colorPairs = val.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
		
		StringBuilder sb = new StringBuilder();
		List<MaterialColor> colorList = new List<MaterialColor>();
		
		foreach(string colorPair in colorPairs)
		{
			//"primary": "#68548E"
			
			int subStringIndex = colorPair.IndexOf(':');
			
			string key = colorPair.Remove(subStringIndex).Replace("\"", System.String.Empty).Trim();
			string colorHex = colorPair.Substring(subStringIndex+1).Replace("\"", System.String.Empty).Trim();
			
			sb.AppendFormat("Key: {0}    Hex: {1} \n", key, colorHex);	
			
			Color newCol;
			if (ColorUtility.TryParseHtmlString(colorHex, out newCol))
			{
				if (key == "scrim")
                {
                    //Scrims use the scrim color role at an opacity of 32%.
                    newCol.a = 0.32f;
                }
                   
				colorList.Add(new MaterialColor(key, newCol));
			}
			else
			{
				Debug.LogWarning( String.Format("ColorKey({0}) failed to parse from hex string.", key) );
				colorList.Add( new MaterialColor(key) );
			}	
		}
		
		Debug.Log(sb.ToString());
		
		theme.standardColorPair = colorList.ToArray();
	}
}
