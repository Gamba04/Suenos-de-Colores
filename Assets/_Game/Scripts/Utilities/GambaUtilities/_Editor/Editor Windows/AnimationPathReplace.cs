using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

public class AnimationPathReplace : EditorWindow
{
    [SerializeField]
    private AnimationClip animation;
    [SerializeField]
    private string find;
    [SerializeField]
    private string replace;

    private new const string title = "Animation Path Replace";

    private SerializedObject serializedObject;

    #region Window

    [MenuItem("Window/" + "Gamba/" + title)]
    public static void Open()
    {
        GetWindow<AnimationPathReplace>(title, true);
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);

        minSize = new Vector2(350, minSize.y);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region OnGUI

    private void OnGUI()
    {
        serializedObject.Update();

        DrawProperties();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawProperties()
    {
        EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth * 0.4f;

        DrawProperty(nameof(animation));
        DrawProperty(nameof(find));
        DrawProperty(nameof(replace));

        DrawSwapButton();

        EditorGUILayout.EndVertical();

        DrawButton("Replace", Replace);
    }

    private void DrawProperty(string name)
    {
        SerializedProperty property = serializedObject.FindProperty(name);

        EditorGUILayout.PropertyField(property);
    }

    private void DrawButton(string name, Action onTrigger)
    {
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(name, GUILayout.Height(25), GUILayout.Width(100)))
        {
            onTrigger?.Invoke();
        }

        GUILayout.EndHorizontal();
    }

    private void DrawSwapButton()
    {
        Rect rect = GUILayoutUtility.GetLastRect();

        rect.width = 22;
        rect.x += EditorGUIUtility.labelWidth - rect.width - EditorGUIUtility.standardVerticalSpacing;
        rect.y -= (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) / 2;

        GUIContent content = new GUIContent("⇅", "Swap values");

        GUIStyle style = new GUIStyle(EditorStyles.miniButton)
        {
            fontSize = 15
        };

        if (GUI.Button(rect, content, style)) Swap();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Animation

    private void Replace()
    {
        if (animation == null || find == "") return;

        Dictionary<EditorCurveBinding, AnimationCurve> curves = new Dictionary<EditorCurveBinding, AnimationCurve>();
        List<EditorCurveBinding> bindings = AnimationUtility.GetCurveBindings(animation).ToList();

        bindings.ForEach(ReplaceBinding);

        AnimationUtility.SetEditorCurves(animation, curves.Keys.ToArray(), curves.Values.ToArray());

        Debug.Log($"Replaced {curves.Count / 2} animation properties");

        void ReplaceBinding(EditorCurveBinding binding)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(animation, binding);

            EditorCurveBinding result = binding;
            result.path = binding.path.Replace(find, replace);

            if (result == binding || bindings.Contains(result)) return;

            curves.Add(binding, null);
            curves.Add(result, curve);
        }
    }

    private void Swap()
    {
        string oldFind = find;

        find = replace;
        replace = oldFind;
    }

    #endregion

}

#endif