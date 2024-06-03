using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneConsole : MonoBehaviour
{
	public static SceneConsole instance;
	public bool isLoading;
		
	public GameObject loadingUI;
	public Scrollbar loadingBar;
	public UnityEvent<string> loadingInfo;
	public float timeThreshold = 2f;
	
	public string loadingHint;
	public string doneLoadingHint;
	
	public UnityEvent loadingEvent;
	
	void Awake()
	{
		instance = this;
	}
		
	public void LoadScene(string sceneName, string loadingHint, string doneLoadingHint)
	{
		if(isLoading)
		{
			return;
		}
		else
		{			
			isLoading = true;
		}
		
		PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
		
		this.loadingHint = loadingHint;
		this.doneLoadingHint = doneLoadingHint;
		StartCoroutine( Loading(sceneName) );
	}
		
	public void LoadScene(string sceneName, string loadingHint, string doneLoadingHint, float timeThreshold)
	{
		if(isLoading)
		{
			return;
		}
		else
		{			
			isLoading = true;
		}
		
		PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
		
		this.loadingHint = loadingHint;
		this.doneLoadingHint = doneLoadingHint;
		this.timeThreshold = timeThreshold;
		StartCoroutine( Loading(sceneName) );
	}
		
	public void LoadScene(string sceneName)
	{
		if(isLoading)
		{
			return;
		}
		else
		{			
			isLoading = true;
		}
		
		PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
		
		StartCoroutine( Loading(sceneName) );
	}
	
	public void LoadAddressableScene(string sceneName, string loadingHint, string doneLoadingHint, float timeThreshold)
	{
		if(isLoading)
		{
			return;
		}
		else
		{			
			isLoading = true;
		}
		
		PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
		
		this.loadingHint = loadingHint;
		this.doneLoadingHint = doneLoadingHint;
		this.timeThreshold = timeThreshold;
		StartCoroutine( LoadingAddressable( System.String.Format("AddressableScene/{0}", sceneName) ) );
	}
	
	public void LoadAddressableScene(string sceneName)
	{
		if(isLoading)
		{
			return;
		}
		else
		{			
			isLoading = true;
		}
		
		PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
		
		StartCoroutine( LoadingAddressable( System.String.Format("AddressableScene/{0}", sceneName) ) );
	}
		
	public void LoadLastScene()
	{
		string sceneName = PlayerPrefs.GetString("LastScene", "");
		
		if(isLoading)
		{
			return;
		}
		else
		{			
			isLoading = true;
		}
		
		PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
		
		StartCoroutine( Loading(sceneName) );
	}
	
	public void SetLoadingHint(string val)
	{
		loadingHint = val;
	}
	
	public void SetDoneLoadingHint(string val)
	{
		doneLoadingHint = val;
	}

    IEnumerator Loading(string val)
    {
		loadingEvent.Invoke();
		
		loadingInfo.Invoke(loadingHint);
		loadingUI.SetActive(true);
		
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync( val );
		
		
		if(timeThreshold > 0f)
		{
			asyncOperation.allowSceneActivation = false;
		}
		
		float startTime = Time.time;
		bool isDone = false;

		float timeProgress = 1f;
		float slowerProgress;

        // Wait until the asynchronous scene fully loads
        while (!isDone)
        {
			if(timeThreshold > 0f)
			{
				timeProgress = (Time.time - startTime)/timeThreshold;
			}
			slowerProgress = asyncOperation.progress < timeProgress ? asyncOperation.progress/0.9f : timeProgress;
			loadingBar.size = Mathf.Clamp01(slowerProgress);
			
			if(asyncOperation.progress >= 0.9f && timeProgress >= 1f)
			{
				isDone = true;
				loadingBar.size = 1f;
				loadingInfo.Invoke(doneLoadingHint);
			}
			
            yield return null;
        }
			
		if( !asyncOperation.allowSceneActivation)
		{
			asyncOperation.allowSceneActivation = true;
		}
    }

    IEnumerator LoadingAddressable(string address)
    {
		loadingEvent.Invoke();
		
		loadingInfo.Invoke(loadingHint);
		loadingUI.SetActive(true);
		
        AsyncOperationHandle<SceneInstance> asyncOperation = Addressables.LoadSceneAsync( address );
		
		// asyncOperation.allowSceneActivation = false;
		
		float startTime = Time.time;
		bool isDone = false;

		float timeProgress = 1f;
		float slowerProgress;

        // Wait until the asynchronous scene fully loads
        while (!isDone)
        {
			if(timeThreshold > 0f)
			{
				timeProgress = (Time.time - startTime)/timeThreshold;
			}
			slowerProgress = asyncOperation.PercentComplete < timeProgress ? asyncOperation.PercentComplete/0.9f : timeProgress;
			loadingBar.size = Mathf.Clamp01(slowerProgress);
			
			if(asyncOperation.PercentComplete >= 0.9f && timeProgress >= 1f)
			{
				isDone = true;
				loadingBar.size = 1f;
				loadingInfo.Invoke(doneLoadingHint);
			}
			
            yield return null;
        }
			
		// yield return waitForSecond;
		// asyncOperation.allowSceneActivation = true;
    }
}
