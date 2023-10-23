using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRabboniConsole : MonoBehaviour
{
	// public Dropdown accDropdown;
	// public Dropdown gyroDropdown;
	// public Dropdown dataRateDropdown;
	
	public Color connectedColor;
	public Color runningColor;
	public Color disconnectedColor;
	public Color warningColor;
	
   /* public void CreateDropdownOptions(Dropdown dropdown, Type enumType, string removeStr)
    {
		List<string> dropOptions = new List<string>();
		
        foreach (string val in Enum.GetNames(enumType))
		{
			dropOptions.Add(val.Replace(removeStr, String.Empty));
		}
		
		dropdown.ClearOptions();
        dropdown.AddOptions(dropOptions);
    }*/
}
