using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractibleObject))]
public class InteractableObjectEditor : Editor
{
    public SerializedProperty TypeProperty;
    public SerializedProperty DamageValueProperty;

    void OnEnable()
    {
        TypeProperty = serializedObject.FindProperty("type");
        DamageValueProperty = serializedObject.FindProperty("DamageValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(TypeProperty);
        InteractibleObject.InteractType type = (InteractibleObject.InteractType)TypeProperty.enumValueIndex;
        switch (type)
        {

            case InteractibleObject.InteractType.Damage:
                EditorGUILayout.PropertyField(DamageValueProperty, new GUIContent("DamageValue"));
                break;

            case InteractibleObject.InteractType.DangerZone:
                EditorGUILayout.PropertyField(DamageValueProperty, new GUIContent("DamageValue"));
                break;
                
        }

        serializedObject.ApplyModifiedProperties();
    }
}
