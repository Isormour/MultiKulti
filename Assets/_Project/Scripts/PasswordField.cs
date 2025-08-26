using UnityEditor;
using UnityEngine;

//
// 1) Atrybut do oznaczania p�l jako "password"
//
public class PasswordAttribute : PropertyAttribute { }





//
// 3) Drawer, kt�ry rysuje pole jako "password" w Inspectorze
//
[CustomPropertyDrawer(typeof(PasswordAttribute))]
public class PasswordDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "U�yj [Password] tylko ze string.");
            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        // Rysowanie maskowanego pola has�a
        string newValue = EditorGUI.PasswordField(position, label, property.stringValue);

        if (newValue != property.stringValue)
            property.stringValue = newValue;

        EditorGUI.EndProperty();
    }
}