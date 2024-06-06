using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPrefs : MonoBehaviour
{
	public void SetVolume(float coef)
	{
		GameBGM.instance.SetVolume(coef);
	}
}
