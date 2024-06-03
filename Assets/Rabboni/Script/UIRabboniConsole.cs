using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static RabboniConsole;

public class UIRabboniConsole : MonoBehaviour
{
    public Dropdown accDropdown;
    public Dropdown gyroDropdown;
    public Dropdown dataRateDropdown;

    public void Start()
    {
        if (accDropdown != null)
        {
            CreateDropdownOptions(accDropdown, typeof(AccScale), "acc");
            RabboniConsole.instance.accScale = (AccScale)PlayerPrefs.GetInt("accScale", 0);
            accDropdown.SetValueWithoutNotify((int)RabboniConsole.instance.accScale);
        }

        if (gyroDropdown != null)
        {
            CreateDropdownOptions(gyroDropdown, typeof(GyroScale), "gyro");
            RabboniConsole.instance.gyroScale = (GyroScale)PlayerPrefs.GetInt("gyroScale", 2);
            gyroDropdown.SetValueWithoutNotify((int)RabboniConsole.instance.gyroScale);
        }

        if (dataRateDropdown != null)
        {
            CreateDropdownOptions(dataRateDropdown, typeof(DataRate), "sample");
            RabboniConsole.instance.dataRate = (DataRate)PlayerPrefs.GetInt("dataRate", 2);
            dataRateDropdown.SetValueWithoutNotify((int)RabboniConsole.instance.dataRate);
        }
    }

    public void CreateDropdownOptions(Dropdown dropdown, Type enumType, string removeStr)
    {
        List<string> dropOptions = new List<string>();

        foreach (string val in Enum.GetNames(enumType))
        {
            dropOptions.Add(val.Replace(removeStr, String.Empty));
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(dropOptions);
    }
}
