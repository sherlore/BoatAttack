using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, ExecuteInEditMode]
public abstract class BaseColorBinder : MonoBehaviour
{
	//[HideInInspector]
	[ColorKeyAttribute]
	public string m_key = "primary";
	public string key
	{
		get
		{
			return m_key;
		}
		set
		{
			m_key = value;
			
			UpdateColor();
		}
	}
	
    protected virtual void Awake()
	{
		Debug.Log("BaseColorBinder/Awake");
		UpdateColor();
		
		#if !UNITY_EDITOR
		if(!ThemeConfig.instance.useDynamicTheme)
		{
			return;
		}
		#endif
		
		if(!ThemeConfig.instance.IsInColorBinder(this))
		{
			ThemeConfig.instance.AddColorBinder(this);
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
		
		if(ThemeConfig.instance.IsInColorBinder(this))
		{
			ThemeConfig.instance.RemoveColorBinder(this);
		}
	}
	
	public virtual void UpdateColor()
	{
		SetColor( GetColor() );
	}
	
	public Color GetColor()
	{
		if(ThemeConfig.instance.colorDic.ContainsKey(key))
		{
			return ThemeConfig.instance.colorDic[key];
		}
		else
		{
			Debug.LogWarning( System.String.Format("Object({0}) with key({1}) cannot found color in theme", this.name, key) );
			return Color.white;
		}
	}
	
	public abstract void SetColor(Color color);
	
	public virtual void OnValidate()
	{
		UpdateColor();
		
		#if !UNITY_EDITOR
		if(!ThemeConfig.instance.useDynamicTheme)
		{
			return;
		}
		#endif
		
		if(!ThemeConfig.instance.IsInColorBinder(this))
		{
			ThemeConfig.instance.AddColorBinder(this);
		}
	}
}
