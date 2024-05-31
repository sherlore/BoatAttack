using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(StageObjectCreator))]
public class StageObjectCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StageObjectCreator myScript = (StageObjectCreator)target;

        if (GUILayout.Button("CreateObjects"))
        {
            myScript.CreateObjects();
            //EditorSceneManager.MarkSceneDirty(myScript.sphere.gameObject.scene);
        }
    }
}
