using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatProgress : MonoBehaviour
{
	public RectTransform pinTransform;
	public Vector2 startPos;
	public Vector2 endPos;
	
    public void UpdateProgress(float progress)
    {
        pinTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
    }
}
