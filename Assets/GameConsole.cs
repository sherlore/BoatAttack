using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameConsole : MonoBehaviour
{
	public UnityEvent startEvent;
	public UnityEvent endEvent;
	
    public TMP_Text countDownText;
	public float countDownLength;
	
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGameFlow());
    }
	
	public void ReachGoal()
	{
		Debug.Log("ReachGoal");
		
		endEvent.Invoke();
	}
	
	public void GamePause()
	{
		Time.timeScale = 0f;
	}
	
	public void GameResume()
	{
		Time.timeScale = 1f;
	}
	
	void OnDestroy()
	{
		if(Time.timeScale != 1f)
		{
			Time.timeScale = 1f;
		}
	}
	
	IEnumerator StartGameFlow()
    {
        //Count Down

        float startMoment = Time.time;

        while (Time.time - startMoment < countDownLength)
        {
            float remainTime = Mathf.Ceil(countDownLength + startMoment - Time.time);
            countDownText.text = remainTime.ToString("F0");

            yield return null;
        }
		
		countDownText.text = "<size=70%>GO!!</size>";

        //Start Game
        startEvent.Invoke();
		
		yield return new WaitForSeconds(1.5f);
		
		countDownText.gameObject.SetActive(false);
    }
}
