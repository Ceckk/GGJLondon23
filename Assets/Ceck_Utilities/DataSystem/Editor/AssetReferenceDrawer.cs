using UnityEditor;
using UnityEngine;

namespace DataSystem
{
    [CustomPropertyDrawer(typeof(AssetReferenceAttribute))]
    public class AssetReferenceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight) * 2 + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                var assetReference = attribute as AssetReferenceAttribute;
                var offset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var idRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                var assetRect = new Rect(position.x, idRect.y + offset, position.width, EditorGUIUtility.singleLineHeight);

                Asset asset = null;
                if (!string.IsNullOrEmpty(property.stringValue))
                {
                    asset = DataBase.GetAssetById(property.stringValue);
                }
                asset = EditorGUI.ObjectField(assetRect, "Asset", asset, assetReference.type, false) as Asset;
                property.stringValue = asset != null ? asset.Id : "";
                EditorGUI.PropertyField(idRect, property);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use AssetReference with string representing an ID");
            }

            EditorGUI.EndProperty();
        }
    }
}
