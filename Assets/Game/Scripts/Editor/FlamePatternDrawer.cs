using Bosses.Sun_Boss;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(FlamePattern))]
    public class FlamePatternDrawer : PropertyDrawer
    {
        private SerializedProperty _flame0,
            _flame1,
            _flame2,
            _flame3,
            _flame4,
            _flame5,
            _flame6,
            _flame7,
            _flame8,
            _flame9,
            _flame10,
            _flame11,
            _flame12,
            _flame13,
            _flame14,
            _flame15;

        private string _name;

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            _name = property.displayName;
            Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(_name));

            if (position.height > 16f)
            {
                position.height = 16f;
                EditorGUI.indentLevel += 1;
                contentPosition = EditorGUI.IndentedRect(position);
                contentPosition.y += 18f;
            }

            float sixteenth = contentPosition.width / 16;
            contentPosition.width = sixteenth;
            GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);

            // label stuff?
            EditorGUIUtility.labelWidth = 14f;
            contentPosition.width *= 0.5f;
            EditorGUI.indentLevel = 0;

            for (int i = 0; i < 16; i++)
            {
                property.Next(true);
                SerializedProperty prop = property.Copy();
                DrawProperty(prop, label, contentPosition);
                contentPosition.x += sixteenth;
            }

            // while (property.Next(true))
            // {
            //     if (property.type)
            //     
            // }
        }

        private void DrawProperty(SerializedProperty prop, GUIContent label, Rect contentPosition)
        {
            EditorGUI.BeginProperty(contentPosition, label, prop);
            {
                EditorGUI.BeginChangeCheck();
                bool newVal = EditorGUI.Toggle(contentPosition, new GUIContent(), prop.boolValue);
                if (EditorGUI.EndChangeCheck())
                    prop.boolValue = newVal;
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Screen.width < 333 ? (16f + 18f) : 16f;
        }
    }
}