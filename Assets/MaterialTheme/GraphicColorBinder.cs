using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Theme/GraphicColorBinder")]
public class GraphicColorBinder : BaseColorBinder
{
    [SerializeField] Graphic graphic;

	protected override void Awake()
	{
		if (graphic == null) graphic = GetComponent<Graphic>();
		
		base.Awake();
	}
	
	public override void SetColor(Color color)
	{
		if (graphic == null)
		{
			return;
		}
		graphic.color = color;
	}
}
