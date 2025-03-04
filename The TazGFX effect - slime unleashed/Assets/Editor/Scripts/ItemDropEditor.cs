using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CustomEditor(typeof(ItemDrop))]
public class ItemDropEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemDrop itemDrop = (ItemDrop)target;

        List<string> _choices = new List<string>();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemList"));

        for (int i = 0; i < itemDrop.itemList.Items.Count; i++)
        {
            _choices.Add(itemDrop.itemList.Items[i].name);
        }

        itemDrop.Items = EditorGUILayout.IntField("Items", itemDrop.Items);

        if (itemDrop.Items < 0)
        {
            itemDrop.Items = 0;
        }

        if (itemDrop.Items != itemDrop.DropChances.Length)
        {
            itemDrop.DropChances = new int[itemDrop.Items];
            itemDrop.items = new string[itemDrop.Items];
            itemDrop.SelectedItems = new int[itemDrop.Items];
        }

        SerializedProperty arrayProp = serializedObject.FindProperty("DropChances");

        arrayProp.isExpanded = EditorGUILayout.Foldout(arrayProp.isExpanded, "Items List");
        if (arrayProp.isExpanded)
        {
            for (int i = 0; i < arrayProp.arraySize; ++i)
            {
                GUILayout.BeginHorizontal();

                SerializedProperty transformProp = arrayProp.GetArrayElementAtIndex(i);

                if (itemDrop.SelectedItems.Length != i) itemDrop.SelectedItems[i] = EditorGUILayout.Popup(itemDrop.SelectedItems[i], _choices.ToArray());

                for (int b = 0; b < _choices.Count; b++)
                {
                    if (itemDrop.SelectedItems.Length != i) itemDrop.items[i] = _choices[itemDrop.SelectedItems[i]];
                }

                int element = transformProp.intValue;

                if (element > 100)
                {
                    element = 100;
                }
                if (element < 0)
                {
                    element = 0;
                }

                if (itemDrop.DropChances.Length != i) itemDrop.DropChances[i] = EditorGUILayout.IntField(new GUIContent("DropChance: "), element);

                GUILayout.Label("%");

                GUILayout.EndHorizontal();

                if (i != arrayProp.arraySize - 1) GUILayout.Space(10);
            }
        }
    }
}
