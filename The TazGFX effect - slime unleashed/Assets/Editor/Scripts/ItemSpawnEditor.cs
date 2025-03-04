using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSpawn))]
public class ItemSpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool makeDirty = false;
        List<string> _choices = new List<string>();
        ItemSpawn Spawn = target as ItemSpawn;


        for (int i = 0; i < Spawn.ItemsList.Items.Count; i++)
        {
            _choices.Add(Spawn.ItemsList.Items[i].name);
        }

        if (Spawn.choice != _choices[Spawn._choiceIndex])
        {
            makeDirty = true;
        }

        Spawn.choice = _choices[Spawn._choiceIndex];

        Spawn.GetComponent<SpriteRenderer>().sprite = Spawn.ItemsList.Items[Spawn._choiceIndex].GetComponent<Item>().sr.sprite;

        GUILayout.BeginHorizontal();

        float newZLayer = EditorGUILayout.FloatField("Z Layer", Spawn.ZLayer);

        if (newZLayer != Spawn.ZLayer)
            makeDirty = true;

        Spawn.ZLayer = newZLayer;

        Spawn._choiceIndex = EditorGUILayout.Popup(Spawn._choiceIndex, _choices.ToArray());
        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemsList"), new GUIContent("ItemsList"));

        var texture = AssetPreview.GetAssetPreview(Spawn.ItemsList.Items[Spawn._choiceIndex].GetComponent<Item>().sr.sprite);

        GUILayout.Label("", GUILayout.Height(300), GUILayout.Width(300));

        Rect Pos = new Rect((Screen.width / 2) - (250 / 2), (GUILayoutUtility.GetLastRect().y + 250 / 2) - (250 / 2), 250, 250);
        Rect ShadowPos = new Rect((Screen.width / 2) - (250 / 2 / 1.65f), (GUILayoutUtility.GetLastRect().y + 250 / 1.4f) - (250 / 2 / 1.65f), 250 / 1.65f, 250 / 1.65f);

        var textureShad = AssetPreview.GetAssetPreview(Spawn.ItemsList.Items[Spawn._choiceIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);

        GUI.DrawTexture(ShadowPos, textureShad, ScaleMode.ScaleToFit, true, 1, new Color(0, 0, 0, 0.5f), 1000, 1000);

        GUI.DrawTexture(Pos, texture);

        if (makeDirty)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
