using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[Serializable]
public class MaterialTypographyAttr
{
    public TMP_FontAsset fontAsset;
    public float fontSize = 20f;
    //public float characterSpacing;
    //public TypographyStyles typographyStyle;
}

[Serializable]
public class MaterialTypography
{
	public string key;


	public MaterialTypographyAttr typographyAttr;

    public MaterialTypography(){}
	
	public MaterialTypography(string key)
	{
		this.key = key;
	}
	
	public MaterialTypography(string key, MaterialTypographyAttr typographyAttr)
	{
		this.key = key;
		this.typographyAttr = typographyAttr;
	}
}
