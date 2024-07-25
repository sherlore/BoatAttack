using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InputFieldPrefsInt : MonoBehaviour
{
	public TMP_InputField inputField;
    public bool initWithPrefs = true;
    public string prefsKey;
    public int defaultValue;

    public UnityEvent<int> intEvent;
    public UnityEvent wrongInputEvent;

    void Start()
    {
        if (initWithPrefs) 
        {
            int prefVal = PlayerPrefs.GetInt(prefsKey, defaultValue);

            intEvent.Invoke(prefVal);
            inputField.SetTextWithoutNotify(prefVal.ToString());
        }
    }

    public void OnReceuveInput(string val) 
    {
        int number;

        bool success = int.TryParse(val, out number);
        if (success)
        {
            intEvent.Invoke(number);

            PlayerPrefs.SetInt(prefsKey, number);
        }
        else
        {
            wrongInputEvent.Invoke();
        }
    }
}
