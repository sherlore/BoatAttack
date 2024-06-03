using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeBuilder", menuName = "ScriptableObjects/ThemeBuilder", order = 1)]
public class ThemeBuilder : ScriptableObject
{
    public ThemeFile themeFile;
		
	[Multiline, Header("Light Theme")]
	public string lightColorPaletteJson;
	
	[Multiline, Header("Dark Theme")]
	public string darkColorPaletteJson;
	
}
