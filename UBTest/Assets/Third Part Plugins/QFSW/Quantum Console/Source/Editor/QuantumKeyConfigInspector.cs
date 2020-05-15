using UnityEditor;
using UnityEngine;

namespace QFSW.QC.Editor
{
    [CustomEditor(typeof(QuantumKeyConfig))]
    public class QuantumKeyConfigInspector : QCInspectorBase
    {
        private QuantumKeyConfig _keyConfigInstance;

        public SerializedProperty _submitCommandKeyProperty;
        public SerializedProperty _hideConsoleKeyProperty;
        public SerializedProperty _showConsoleKeyProperty;
        public SerializedProperty _toggleConsoleVisibilityKeyProperty;

        public SerializedProperty _suggestNextCommandKeyProperty;
        public SerializedProperty _suggestPreviousCommandKeyProperty;

        public SerializedProperty _nextCommandKeyProperty;
        public SerializedProperty _previousCommandKeyProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            _keyConfigInstance = (QuantumKeyConfig)target;

            _submitCommandKeyProperty = serializedObject.FindProperty("SubmitCommandKey");
            _hideConsoleKeyProperty = serializedObject.FindProperty("HideConsoleKey");
            _showConsoleKeyProperty = serializedObject.FindProperty("ShowConsoleKey");
            _toggleConsoleVisibilityKeyProperty = serializedObject.FindProperty("ToggleConsoleVisibilityKey");

            _suggestNextCommandKeyProperty = serializedObject.FindProperty("SuggestNextCommandKey");
            _suggestPreviousCommandKeyProperty = serializedObject.FindProperty("SuggestPreviousCommandKey");

            _nextCommandKeyProperty = serializedObject.FindProperty("NextCommandKey");
            _previousCommandKeyProperty = serializedObject.FindProperty("PreviousCommandKey");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorHelpers.DrawBanner(Banner);

            EditorGUILayout.LabelField(new GUIContent("General"), EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_submitCommandKeyProperty, new GUIContent("Submit Command", "The key to submit and invoke the current console input."));
            EditorGUILayout.PropertyField(_showConsoleKeyProperty, new GUIContent("Show Console", "The key used to show and activate the console."));
            EditorGUILayout.PropertyField(_hideConsoleKeyProperty, new GUIContent("Hide Console", "The key used to hide and deactivate the console."));
            EditorGUILayout.PropertyField(_toggleConsoleVisibilityKeyProperty, new GUIContent("Toggle Console", "The key used to toggle the active and visibility state of the console."));
            EditorGUILayout.Space();

            EditorGUILayout.LabelField(new GUIContent("Command Suggestions"), EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_suggestNextCommandKeyProperty, new GUIContent("Show Suggested Commands", "The key to show and suggest commands based on the current console input."));
            EditorGUILayout.PropertyField(_suggestPreviousCommandKeyProperty, new GUIContent("Show Previous Suggestion", "The key to show the previous suggestion."));
            EditorGUILayout.Space();

            EditorGUILayout.LabelField(new GUIContent("Command History"), EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_nextCommandKeyProperty, new GUIContent("Select Next Command", "The key to be used to select the next command from the console history."));
            EditorGUILayout.PropertyField(_previousCommandKeyProperty, new GUIContent("Select Previous Command", "The key to be used to select the previous command from the console history."));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
