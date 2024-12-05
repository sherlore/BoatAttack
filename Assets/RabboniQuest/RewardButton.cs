using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
	public Button button;
	public Text label;
	public GameObject icon;
	
    public void SetActive(bool val)
    {
        button.gameObject.SetActive(val);
    }
}
