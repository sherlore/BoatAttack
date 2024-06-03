using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeConfig", menuName = "ScriptableObjects/ThemeConfig", order = 1)]
public class ThemeConfig : ScriptableObject
{	
	private static ThemeConfig m_instance;
	public  static ThemeConfig instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = GetOrCreateInstance();
				m_instance.Init();
			}
			return m_instance;
		}
	}
	
	public enum ThemeMode
	{
		Dark,
		Light
	}
	
	public ThemeMode themeMode;
    	
	public Theme currentTheme
	{
		get
		{
			if(themeMode == ThemeMode.Dark)
			{
				return themeFile.darkTheme;
			}
			else
			{
				return themeFile.lightTheme;
			}
		}
	}
	
	public bool useDynamicTheme;
	
	public ThemeFile themeFile;
	
	public const string PATH                    = "Assets/MaterialTheme/Resources/ThemeConfig.asset";
	public const string PATH_FOR_RESOURCES_LOAD = "ThemeConfig";
	
	//Color
	public Dictionary<string, Color> colorDic = new Dictionary<string, Color>();
	
	[System.NonSerialized]
	private List<BaseColorBinder> colorBinderList = new List<BaseColorBinder>();
	
	#if UNITY_EDITOR
	public List<string> colorOptions;
	#endif
	
	//Typography	
	public MaterialTypographyAttr defaultTypographyAttr;	
	public Dictionary<string, MaterialTypographyAttr> typographyDic = new Dictionary<string, MaterialTypographyAttr>();
	
	#if UNITY_EDITOR
	[System.NonSerialized]
	private List<BaseTypographyBinder> typographyBinderList = new List<BaseTypographyBinder>();
	
	public List<string> typographyOptions;	
	#endif	
	
	//ThemeMode
	[System.NonSerialized]
	private List<ThemeModeBinder> themeModeBinderList = new List<ThemeModeBinder>();
		
	public static ThemeConfig GetOrCreateInstance()
	{
		var config = Resources.Load<ThemeConfig>(PATH_FOR_RESOURCES_LOAD);
		/*if (config == null)
		{
#if UNITY_EDITOR
			Debug.Log($"<color=orange><b>Creating Unity-Theme database file</b> at <i>{PATH}</i></color>");
			config = ScriptableObject.CreateInstance<ThemeConfig>();
			
			var directory = System.IO.Path.GetDirectoryName(PATH);
			if (!System.IO.Directory.Exists(directory))
			{
				System.IO.Directory.CreateDirectory(directory);
			}

			UnityEditor.AssetDatabase.CreateAsset(config, PATH);
			UnityEditor.AssetDatabase.SaveAssets();

			return config;
#else
			Debug.LogError($"Can't find <b>Unity-Theme database file</b> at <i>{PATH}</i>");
#endif
		}*/
		return config;
	}
	
	public void Init()
	{		
		colorBinderList.Clear();
		UpdateColor();
		UpdateTypography();
		
		#if UNITY_EDITOR
		typographyBinderList.Clear();
		#endif
		
		themeModeBinderList.Clear();
	}
	
	public void SwitchTheme()
	{
		if(themeMode == ThemeMode.Dark)
		{
			themeMode = ThemeMode.Light;
		}
		else
		{
			themeMode = ThemeMode.Dark;
		}
		
		UpdateColor();
		
		foreach(ThemeModeBinder themeModeBinder in themeModeBinderList)
		{
			themeModeBinder.UpdateThemeMode();
		}
	}
	
	public virtual void OnValidate()
	{
		UpdateColor();
		UpdateTypography();
	}
	
	public void UpdateColor()
	{
		colorDic.Clear();
		
		#if UNITY_EDITOR
		colorOptions.Clear();		
		colorOptions.Add("Default");
		#endif
		
		foreach(MaterialColor materialColor in currentTheme.standardColorPair)
		{
			#if UNITY_EDITOR
			colorOptions.Add(materialColor.key);
			#endif
			
			colorDic[materialColor.key] = materialColor.color;
		}
		
		foreach(MaterialColor materialColor in currentTheme.customColorColorPair)
		{
			#if UNITY_EDITOR
			colorOptions.Add(materialColor.key);
			#endif
			
			colorDic[materialColor.key] = materialColor.color;
		}
		
		foreach(BaseColorBinder colorBinder in colorBinderList)
		{
			colorBinder.UpdateColor();
		}
	}
	
	public bool IsInColorBinder(BaseColorBinder colorBinder)
	{
		return colorBinderList.Contains(colorBinder);
	}
	
	public void AddColorBinder(BaseColorBinder colorBinder)
	{
		colorBinderList.Add(colorBinder);
	}
	
	public void RemoveColorBinder(BaseColorBinder colorBinder)
	{
		colorBinderList.Remove(colorBinder);
	}
	
	public Color GetColor(string key)
	{
		if(colorDic.ContainsKey(key))
		{
			return colorDic[key];
		}
		else
		{
			Debug.LogWarning("Can not found color in theme");
			return Color.white;
		}
	}
	
	public void UpdateTypography()
	{
		typographyDic.Clear();
		
		#if UNITY_EDITOR
		typographyOptions.Clear();		
		typographyOptions.Add("Default");
		#endif
		
		if(themeFile.typographyPair.Length > 0)
		{
			foreach(MaterialTypography materialTypography in themeFile.typographyPair)
			{
				#if UNITY_EDITOR
				typographyOptions.Add(materialTypography.key);
				#endif
				
				typographyDic[materialTypography.key] = materialTypography.typographyAttr;
			}
		}

		#if UNITY_EDITOR
        foreach (BaseTypographyBinder typographyBinder in typographyBinderList)
		{
			typographyBinder.UpdateTypography();
        }
		#endif
    }

#if UNITY_EDITOR
    public bool IsInTypographyBinder(BaseTypographyBinder typographyBinder)
	{
		return typographyBinderList.Contains(typographyBinder);
	}
	
	public void AddTypographyBinder(BaseTypographyBinder typographyBinder)
	{
		typographyBinderList.Add(typographyBinder);
	}
	
	public void RemoveTypographyBinder(BaseTypographyBinder typographyBinder)
	{
		typographyBinderList.Remove(typographyBinder);
	}
#endif

    public MaterialTypographyAttr GetTypographyAttr(string key)
	{
		if(typographyDic.ContainsKey(key))
		{
			return typographyDic[key];
		}
		else
		{
			Debug.LogWarning("Can not found typography in theme");
			return ThemeConfig.instance.defaultTypographyAttr;
		}
	}
	
	public bool IsInThemeModeBinder(ThemeModeBinder themeModeBinder)
	{
		return themeModeBinderList.Contains(themeModeBinder);
	}
	
	public void AddThemeModeBinder(ThemeModeBinder themeModeBinder)
	{
		themeModeBinderList.Add(themeModeBinder);
	}
	
	public void RemoveThemeModeBinder(ThemeModeBinder themeModeBinder)
	{
		themeModeBinderList.Remove(themeModeBinder);
	}
}
