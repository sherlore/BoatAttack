using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "ScriptableObjects/ThemeFile", order = 1)]
public class ThemeFile : ScriptableObject
{
    public Theme lightTheme;
    public Theme darkTheme;
		
    public MaterialTypography[] typographyPair;
}