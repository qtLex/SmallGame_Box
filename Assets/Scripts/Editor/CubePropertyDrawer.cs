using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(Cube))]
public class CubePropertyDrawer :  PropertyDrawer {

	const float texWidth = 50;
	const float objWidth = 300;
	const float guidWidth = 100;
	const float spacing = 5;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		return texWidth;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){

		EditorGUI.BeginProperty (position, label, property);

		// Don't make child fields be indented
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		Rect keyRect = new Rect (position.x, position.y, guidWidth, position.height);
		Rect objRect = new Rect (position.x + guidWidth + spacing , position.y, position.width - texWidth - 2 * spacing - guidWidth - 20, position.height);
		Rect TexRect = new Rect (position.width - texWidth, position.y, texWidth, position.height);

		SerializedProperty obj = property.FindPropertyRelative("Object");
		SerializedProperty key = property.FindPropertyRelative("Key");

		if (key != null){
			EditorGUI.PropertyField(keyRect, key , GUIContent.none);
		}

		if (obj != null){
			obj.objectReferenceValue = EditorGUI.ObjectField(objRect, obj.objectReferenceValue, typeof(GameObject), false);

			if (obj.objectReferenceValue != null){
				Texture2D tex = AssetPreview.GetAssetPreview(obj.objectReferenceValue);
				EditorGUI.DrawPreviewTexture(TexRect, tex);
			}

		}

		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();

	}
}
