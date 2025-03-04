using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CreateUtility
{
    public static GameObject CreatePrefab(string Path, bool KeepPacked)
    {
        GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load(Path)) as GameObject;
        if (!KeepPacked) PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        Place(newObj);
        return newObj;
    }

    public static void Place(GameObject obj)
    {
        SceneView lastView = SceneView.lastActiveSceneView;
        obj.transform.position = lastView ? lastView.pivot : Vector3.zero;

        StageUtility.PlaceGameObjectInCurrentStage(obj);
        GameObjectUtility.EnsureUniqueNameForSibling(obj);

        Undo.RegisterCreatedObjectUndo(obj, $"Create Object: {obj.name}");
        Selection.activeGameObject = obj;

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
