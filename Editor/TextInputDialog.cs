using UnityEditor;
using UnityEngine;

namespace GGTools.BuildTools
{

    public class TextInputDialog : EditorWindow
    {
        string text = "";
        System.Action<string> onConfirm;

        /// <summary>
        /// Opens a modal editor window that allows the user to input multi-line text.
        /// The provided callback is invoked when the user confirms the input.
        /// </summary>
        /// <param name="title">Title displayed on the dialog window.</param>
        /// <param name="defaultText">Initial text shown in the input field.</param>
        /// <param name="callback">Callback executed when the user presses OK, returning the entered text.</param>
        public static void Show(string title, string defaultText, System.Action<string> callback)
        {
            var window = CreateInstance<TextInputDialog>();
            window.titleContent = new GUIContent(title);
            window.text = defaultText;
            window.onConfirm = callback;
            window.position = new Rect(0, 0, Screen.width, Screen.height);
            window.ShowModal();
        }

        void OnGUI()
        {
            GUILayout.Label("Enter value:", EditorStyles.label);
            float halfHeight = 3 * position.height / 4f;

            text = EditorGUILayout.TextArea(text, GUILayout.Height(halfHeight));

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Cancel"))
            {
                Close();
            }

            GUI.enabled = text.Length >= 100;

            if (GUILayout.Button("OK"))
            {
                onConfirm?.Invoke(text);
                Close();
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();
        }
    }
}