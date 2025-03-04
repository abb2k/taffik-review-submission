using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

[CustomEditor(typeof(AnimatronicCell))]
public class AnimatronicCellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AnimatronicCell cell = (AnimatronicCell)target;

        if (cell.AnimatronicSettings.Name == string.Empty)
        {
            cell.gameObject.name = "animatronicCell";
        }
        else
        {
            string myname = cell.AnimatronicSettings.Name + " (animatronicCell)";
            cell.gameObject.name = myname;
        }

        cell.myImage.sprite = cell.sprite;

        var texture = AssetPreview.GetAssetPreview(cell.sprite);
        GUILayout.Label(texture);
    }
}
