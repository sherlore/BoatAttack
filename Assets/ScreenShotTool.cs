using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotTool : MonoBehaviour
{
	public KeyCode screenShotKey;
	
	#if UNITY_EDITOR
	
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
	
    // Update is called once per frame
    void Update()
    {
		
        if(Input.GetKeyDown(screenShotKey) )
		{
			Debug.Log("ScreenShot");
			DateTime thisDay = DateTime.Now;
			ScreenCapture.CaptureScreenshot( System.String.Format("ScreenShot/ScreenShot_{0}x{1}_{2}.png", Screen.width, Screen.height, thisDay.ToString("yyyyMMdd_HH_mm_ss") ) );
		}
		
		
    }
	#endif
}
