using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CraftingRecipe))]
public class CraftingRecipeEditor : Editor {

    public override void OnInspectorGUI() {

        serializedObject.Update();

        CraftingRecipe craftingRecipe = (CraftingRecipe)target;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Output", new GUIStyle { fontStyle = FontStyle.Bold });
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical();

        Texture texture = null;

        if(craftingRecipe.output != null) {
            texture = craftingRecipe.output.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("output"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.Label("Recipe", new GUIStyle { fontStyle = FontStyle.Bold });
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        texture = null;
        if(craftingRecipe.input0 != null) {
            texture = craftingRecipe.input0.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input0"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        texture = null;
        if(craftingRecipe.input1 != null) {
            texture = craftingRecipe.input1.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input1"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        texture = null;
        if(craftingRecipe.input2 != null) {
            texture = craftingRecipe.input2.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input2"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        texture = null;
        if(craftingRecipe.input3 != null) {
            texture = craftingRecipe.input3.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input3"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        texture = null;
        if(craftingRecipe.input4 != null) {
            texture = craftingRecipe.input4.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input4"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        texture = null;
        if(craftingRecipe.input5 != null) {
            texture = craftingRecipe.input5.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input5"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        texture = null;
        if(craftingRecipe.input6 != null) {
            texture = craftingRecipe.input6.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input6"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        texture = null;
        if(craftingRecipe.input7 != null) {
            texture = craftingRecipe.input7.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input7"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        texture = null;
        if(craftingRecipe.input8 != null) {
            texture = craftingRecipe.input8.firstIcon.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input8"), GUIContent.none, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
