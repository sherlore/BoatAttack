using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InputFieldPrefsFloat : MonoBehaviour
{
    public TMP_InputField inputField;
    public bool initWithPrefs = true;
    public string prefsKey;
    public float defaultValue;

    public UnityEvent<float> floatEvent;
    public UnityEvent wrongInputEvent;

    void Start()
    {
        if (initWithPrefs) 
        {
            float prefVal = PlayerPrefs.GetFloat(prefsKey, defaultValue);

            floatEvent.Invoke(prefVal);
            inputField.SetTextWithoutNotify(prefVal.ToString());
        }
    }

    public void OnReceuveInput(string val) 
    {
        float number;

        bool success = float.TryParse(val, out number);
        if (success)
        {
            floatEvent.Invoke(number);

            PlayerPrefs.SetFloat(prefsKey, number);
        }
        else
        {
            wrongInputEvent.Invoke();
        }
    }
}
