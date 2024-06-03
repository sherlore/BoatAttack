using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, ExecuteInEditMode]
public abstract class BaseTypographyBinder : MonoBehaviour
{
    [TypographyKey]
	public string m_key = "Default";
	public string key
	{
		get
		{
			return m_key;
		}
		set
		{
			m_key = value;
			
			UpdateTypography();
		}
	}
	
    protected virtual void Awake()
	{
		Debug.Log("BaseTypographyBinder/Awake");
		UpdateTypography();
		
		#if UNITY_EDITOR
		if(!ThemeConfig.instance.IsInTypographyBinder(this))
		{
			ThemeConfig.instance.AddTypographyBinder(this);
		}
		#endif
		
	}
	
	protected virtual void OnDestroy()
	{
		#if UNITY_EDITOR		
		if(ThemeConfig.instance.IsInTypographyBinder(this))
		{
			ThemeConfig.instance.RemoveTypographyBinder(this);
		}
		#endif		
	}
	
	public virtual void UpdateTypography()
	{
		SetTypographyAttr( GetTypographyAttr() );
	}
	
	public MaterialTypographyAttr GetTypographyAttr()
	{
		if(ThemeConfig.instance.typographyDic.ContainsKey(key))
		{
			return ThemeConfig.instance.typographyDic[key];
		}
		else
		{
			Debug.LogWarning( System.String.Format("Object({0}) with key({1}) cannot found typography in theme", this.name, key) );
			return ThemeConfig.instance.defaultTypographyAttr;
		}
	}
	
	public abstract void SetTypographyAttr(MaterialTypographyAttr typographyAttr);
	
	public virtual void OnValidate()
	{
		UpdateTypography();
		
		#if UNITY_EDITOR		
		if(!ThemeConfig.instance.IsInTypographyBinder(this))
		{
			ThemeConfig.instance.AddTypographyBinder(this);
		}
		#endif
		
	}
}
