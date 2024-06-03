using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Theme/ThemeSpriteBinder")]
public class ThemeSpriteBinder : ThemeModeBinder
{
	[SerializeField] Image img;
	
	public Sprite lightSprite;
	public Sprite darkSprite;

	protected override void Awake()
	{
		if (img == null) img = GetComponent<Image>();
		
		base.Awake();
	}
	
    public override void SetThemeMode(ThemeConfig.ThemeMode themeMode)
	{
		if (themeMode == ThemeConfig.ThemeMode.Light)
		{
			img.sprite = lightSprite;
		}
		else if (themeMode == ThemeConfig.ThemeMode.Dark)
		{
			img.sprite = darkSprite;
		}
	}
}
