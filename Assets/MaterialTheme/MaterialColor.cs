using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaterialColor
{
    public string key;
	public Color color;
	
	public MaterialColor(){}
	
	public MaterialColor(string key)
	{
		this.key = key;
	}
	
	public MaterialColor(string key, Color color)
	{
		this.key = key;
		this.color = color;
	}
}
