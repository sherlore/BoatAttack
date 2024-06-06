using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(DragonBoatTeam))]
public class DragonBoatTeamEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DragonBoatTeam myScript = (DragonBoatTeam)target;

        if (GUILayout.Button("Set Team 1"))
        {
            myScript.SetTeam(true);
			SetTeamDirty(myScript);	
        }

        if (GUILayout.Button("Set Team 2"))
        {
            myScript.SetTeam(false);
			SetTeamDirty(myScript);
        }
    }
	
	public void SetTeamDirty(DragonBoatTeam team)
	{
		foreach(Renderer boy in team.boys)
		{
			EditorUtility.SetDirty(boy);
		}
		foreach(Renderer girl in team.girls)
		{
			EditorUtility.SetDirty(girl);
		}
	}
}
