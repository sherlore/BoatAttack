using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotTool : MonoBehaviour
{
	#if UNITY_EDITOR
	
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
		
	public void SreenShot()
	{
		DateTime thisDay = DateTime.Now;
		ScreenCapture.CaptureScreenshot( System.String.Format("ScreenShot/ScreenShot_{0}x{1}_{2}.png", Screen.width, Screen.height, thisDay.ToString("yyyyMMdd_HH_mm_ss") ) );
	}
	
	
	#endif
}
