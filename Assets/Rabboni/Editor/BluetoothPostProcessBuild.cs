﻿#if UNITY_IOS
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

public class BluetoothPostProcessBuild
{
	[PostProcessBuild]
	public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
	{
		if (buildTarget == BuildTarget.iOS)
		{
			// Get plist
			string plistPath = pathToBuiltProject + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));

			// Get root
			PlistElementDict rootDict = plist.root;

			rootDict.SetString("NSBluetoothPeripheralUsageDescription", "使用藍芽連接體感感測器");
			rootDict.SetString("NSBluetoothAlwaysUsageDescription", "使用藍芽連接體感感測器");

			// Write to file
			File.WriteAllText(plistPath, plist.WriteToString());
		}
	}
}
#endif