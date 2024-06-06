using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBGM : MonoBehaviour
{
	public static GameBGM instance;
	public AudioSource bgm;
	
	public Vector2 volumeRange;
	
	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public void SetVolume(float coef)
	{
		bgm.volume = Mathf.Lerp(volumeRange.x, volumeRange.y, coef);
	}
}
