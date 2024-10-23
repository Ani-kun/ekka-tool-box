using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ekka.toolbox.editor
{
    public sealed class AssetFileCreator
    {
        [MenuItem("Assets/Create/Blend Tree [EKKA Toolbox]", priority = 405)]
        public static void CreateNewBlendTree()
        {
            CreateAssetFileInActiveFolder(new BlendTree(), "New Blend Tree.Asset");
        }

        private static void CreateAssetFileInActiveFolder(Object asset, string fileName = "New Asset File.Asset")
        {
            string activeFolderPath = typeof(ProjectWindowUtil).GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[0]).ToString();

            string filePath = AssetDatabase.GenerateUniqueAssetPath(activeFolderPath + "/" + fileName);

            AssetDatabase.CreateAsset(asset, filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
    }

}
