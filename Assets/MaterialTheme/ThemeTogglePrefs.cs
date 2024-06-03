using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThemeTogglePrefs : MonoBehaviour
{
    public UnityEvent<bool> themeInitEvent;

    // Start is called before the first frame update
    void Start()
    {
        themeInitEvent.Invoke(ThemeConfig.instance.themeMode == ThemeConfig.ThemeMode.Light);
    }
}
