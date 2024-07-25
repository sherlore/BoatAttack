using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SliderPlugin : MonoBehaviour
{
    public Slider slider;
    public bool initWithPrefs = true;
    public string prefsKey;
    public float defaultValue;

    public UnityEvent<float> floatEvent;

    void Start()
    {
        if (initWithPrefs) 
        {
            float prefVal = PlayerPrefs.GetFloat(prefsKey, defaultValue);

            floatEvent.Invoke(prefVal);
            slider.SetValueWithoutNotify(prefVal);
        }
    }

    public void OnReceuveInput(float val) 
    {
        floatEvent.Invoke(val);
        PlayerPrefs.SetFloat(prefsKey, val);
    }
}
