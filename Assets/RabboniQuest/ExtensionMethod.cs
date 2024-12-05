using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    public static bool ToBool(this object obj)
    {
        // return (obj is bool)? (bool)obj : bool.Parse(obj.ToString());
		
		if(obj is bool)
		{
			return (bool)obj;
		}
		else
		{
			bool val;
			
			bool success = bool.TryParse(obj.ToString(), out val);
			if (success)
			{
				return val;
			}
			else
			{
				Debug.LogWarning("ExtensionMethod.ToBool not match");
				return false;
			}
		}
    }
    public static int ToInt(this object obj)
    {
        // return (obj is int)? (int)obj : int.Parse(obj.ToString());
		
		if(obj is int)
		{
			return (int)obj;
		}
		else
		{
			int val;
			
			bool success = int.TryParse(obj.ToString(), out val);
			if (success)
			{
				return val;
			}
			else
			{
				Debug.LogWarning("ExtensionMethod.ToInt not match");
				return 0;
			}
		}
    }
	
    public static long ToLong(this object obj)
    {
        // return (obj is long)? (long)obj : long.Parse(obj.ToString());
		
		if(obj is long)
		{
			return (long)obj;
		}
		else
		{
			long val;
			
			bool success = long.TryParse(obj.ToString(), out val);
			if (success)
			{
				return val;
			}
			else
			{
				Debug.LogWarning("ExtensionMethod.ToLong not match");
				return 0L;
			}
		}
    }
	
    public static float ToFloat(this object obj)
    {
        // return (obj is float)? (float)obj : float.Parse(obj.ToString());
		
		if(obj is float)
		{
			return (float)obj;
		}
		else
		{
			float val;
			
			bool success = float.TryParse(obj.ToString(), out val);
			if (success)
			{
				return val;
			}
			else
			{
				Debug.LogWarning("ExtensionMethod.ToFloat not match");
				return 0f;
			}
		}
    }
	
    public static double ToDouble(this object obj)
    {
        // return (obj is double)? (double)obj : double.Parse(obj.ToString());
		
		if(obj is double)
		{
			return (double)obj;
		}
		else
		{
			double val;
			
			bool success = double.TryParse(obj.ToString(), out val);
			if (success)
			{
				return val;
			}
			else
			{
				Debug.LogWarning("ExtensionMethod.ToDouble not match");
				return 0;
			}
		}
    }
	
    public static string ToText(this object obj)
    {
        return (obj is string)? (string)obj : obj.ToString();
    }
	
	public static int FloorToInt(this float val)
	{
		return Mathf.FloorToInt(val);
	}
}
