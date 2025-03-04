using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(EnemySpawn))]
public class EnemySpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool makeDirty = false;
        List<string> _choices = new List<string>();
        EnemySpawn Spawn = target as EnemySpawn;


        for (int i = 0; i < Spawn.enemyList.Enemies.Count; i++)
        {
            _choices.Add(Spawn.enemyList.Enemies[i].name);
        }

        if (Spawn.choice != _choices[Spawn._choiceIndex])
        {
            makeDirty = true;
        }

        Spawn.choice = _choices[Spawn._choiceIndex];

        Spawn.GetComponent<SpriteRenderer>().sprite = Spawn.enemyList.Enemies[Spawn._choiceIndex].GetComponent<enemyBase>().sr.sprite;

        GUILayout.BeginHorizontal();

        Spawn.ZLayer = EditorGUILayout.FloatField("Z Layer", Spawn.ZLayer);

        Spawn._choiceIndex = EditorGUILayout.Popup(Spawn._choiceIndex, _choices.ToArray());

        Spawn.Vision = Spawn.enemyList.Enemies[Spawn._choiceIndex].GetComponent<enemyBase>().enemyVision;

        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyList"), new GUIContent("EnemyList"));

        var texture = AssetPreview.GetAssetPreview(Spawn.enemyList.Enemies[Spawn._choiceIndex].GetComponent<enemyBase>().sr.sprite);

        GUILayout.Label("", GUILayout.Height(300), GUILayout.Width(300));

        Rect Pos = new Rect((Screen.width / 2) - (250 / 2), (GUILayoutUtility.GetLastRect().y + 250 / 2) - (250 / 2), 250, 250);
        Rect ShadowPos = new Rect((Screen.width / 2) - (250 / 2 / 1.5f), (GUILayoutUtility.GetLastRect().y + 250 / 1.25f) - (250 / 2 / 1.5f), 250 / 1.5f, 250 / 1.5f);

        var textureShad = AssetPreview.GetAssetPreview(Spawn.enemyList.Enemies[Spawn._choiceIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);

        GUI.DrawTexture(ShadowPos, textureShad, ScaleMode.ScaleToFit, true, 1, new Color(0,0,0,0.5f), 1000, 1000);

        GUI.DrawTexture(Pos, texture);

        if (makeDirty)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
