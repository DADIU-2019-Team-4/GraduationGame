using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractibleObject))]
public class InteractableObjectEditor : Editor
{
    public SerializedProperty TypeProperty;
    public SerializedProperty DamageValueProperty;
    public SerializedProperty HealValueProperty;
    public SerializedProperty BumpHeightProperty;

    void OnEnable()
    {
        TypeProperty = serializedObject.FindProperty("type");
        DamageValueProperty = serializedObject.FindProperty("DamageValue");
        HealValueProperty = serializedObject.FindProperty("HealValue");
        BumpHeightProperty = serializedObject.FindProperty("BumpHeight");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(TypeProperty);
        InteractibleObject.InteractType type = (InteractibleObject.InteractType)TypeProperty.enumValueIndex;
        switch (type)
        {

            case InteractibleObject.InteractType.Damage:
                EditorGUILayout.PropertyField(DamageValueProperty, new GUIContent("Damage Value"));
                break;

            case InteractibleObject.InteractType.DangerZone:
                EditorGUILayout.PropertyField(DamageValueProperty, new GUIContent("Damage Per Second"));
                break;

            case InteractibleObject.InteractType.Break:
            case InteractibleObject.InteractType.BurnableProp:
                EditorGUILayout.PropertyField(HealValueProperty, new GUIContent("Heal Amount"));
                break;

            case InteractibleObject.InteractType.PickUp:
                EditorGUILayout.PropertyField(HealValueProperty, new GUIContent("Heal Amount"));
                break;
        }

        EditorGUILayout.PropertyField(BumpHeightProperty, new GUIContent("Bump Height"));
        serializedObject.ApplyModifiedProperties();
    }
}
