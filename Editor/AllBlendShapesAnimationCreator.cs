using UnityEditor;
using UnityEngine;

namespace ekka.toolBox.editor
{
    public class AllBlendShapesAnimationCreator
    {
        [MenuItem("GameObject/EKKA Tool Box/Create All Blend Shapes Animation", false, 0)]
        public static void CreateAllBlendShapesAnimation()
        {
            CreateAllBlendShapesAnimation(Selection.activeGameObject);
        }

        private static void CreateAllBlendShapesAnimation(GameObject rootObject)
        {
            if (rootObject is null)
            {
                return;
            }

            var skinnedMeshComponents = rootObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            if (skinnedMeshComponents is null || skinnedMeshComponents.Length <= 0)
            {
                EditorUtility.DisplayDialog("Error", "No Skinned Mesh Renderer.", "OK");
                return;
            }

            var animationClip = new AnimationClip();
            var curve = new AnimationCurve(new Keyframe(0f, 0f));

            foreach (var meshComponent in skinnedMeshComponents)
            {
                var objectRelativePath = AnimationUtility.CalculateTransformPath(meshComponent.transform, rootObject.transform);
                var blendShapeCount = meshComponent.sharedMesh.blendShapeCount;

                for (var index = 0; index < blendShapeCount; index++)
                {
                    var propertyName = "blendShape." + meshComponent.sharedMesh.GetBlendShapeName(index);
                    var curveBinding = EditorCurveBinding.FloatCurve(objectRelativePath, typeof(SkinnedMeshRenderer), propertyName);

                    AnimationUtility.SetEditorCurve(animationClip, curveBinding, curve);
                }
            }

            if (animationClip.empty)
            {
                EditorUtility.DisplayDialog("Error", "No Blend Shapes.", "OK");
            }
            else
            {
                var filePath = EditorUtility.SaveFilePanelInProject("Save", rootObject.name + "_AllBlendShapes", "anim", "");
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }
                else
                { 
                    AssetDatabase.CreateAsset(animationClip, filePath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }

    }

}
