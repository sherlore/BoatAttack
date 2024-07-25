using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InputFieldPrefsString : MonoBehaviour
{
    public TMP_InputField inputField;
    public bool initWithPrefs = true;
    public string prefsKey;
    public string defaultValue;

    public UnityEvent<string> stringEvent;

    void Start()
    {
        if (initWithPrefs) 
        {
            string prefVal = PlayerPrefs.GetString(prefsKey, defaultValue);

            stringEvent.Invoke(prefVal);
            inputField.SetTextWithoutNotify(prefVal);
        }
    }

    public void OnReceuveInput(string val) 
    {
		stringEvent.Invoke(val);

		PlayerPrefs.SetString(prefsKey, val);
    }
}
