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
        //StartCoroutine(StartGameFlow());
    }
	
	public void ReachGoal()
	{
		Debug.Log("ReachGoal");
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

        //Start Game
        startEvent.Invoke();
    }
}
