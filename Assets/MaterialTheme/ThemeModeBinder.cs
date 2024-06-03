using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, ExecuteInEditMode]
public abstract class ThemeModeBinder : MonoBehaviour
{
    protected virtual void Awake()
	{
		UpdateThemeMode();
		
		#if !UNITY_EDITOR
		if(!ThemeConfig.instance.useDynamicTheme)
		{
			return;
		}
		#endif
		
		if(!ThemeConfig.instance.IsInThemeModeBinder(this))
		{
			ThemeConfig.instance.AddThemeModeBinder(this);
		}
	}
	
	protected virtual void OnDestroy()
	{
		#if !UNITY_EDITOR
		if(!ThemeConfig.instance.useDynamicTheme)
		{
			return;
		}
		#endif
		
		if(ThemeConfig.instance.IsInThemeModeBinder(this))
		{
			ThemeConfig.instance.RemoveThemeModeBinder(this);
		}
	}
	
	public virtual void UpdateThemeMode()
	{
		SetThemeMode( ThemeConfig.instance.themeMode );
	}
	
	public abstract void SetThemeMode(ThemeConfig.ThemeMode themeMode);
	
	public virtual void OnValidate()
	{
		UpdateThemeMode();
		
		#if !UNITY_EDITOR
		if(!ThemeConfig.instance.useDynamicTheme)
		{
			return;
		}
		#endif
		
		if(!ThemeConfig.instance.IsInThemeModeBinder(this))
		{
			ThemeConfig.instance.AddThemeModeBinder(this);
		}
	}
}
