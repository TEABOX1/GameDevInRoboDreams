#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace MainGame
{
    public class MinimapRenderer : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private RenderTexture _renderTexture;

        [ContextMenu("Bake Minimap")]
        private void BakeMinimap()
        {
            #if UNITY_EDITOR
            int width = _renderTexture.width;
            int height = _renderTexture.height;
            Texture2D texture = new Texture2D(width, height);
            _camera.Render();
            RenderTexture.active = _renderTexture;
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();

            string path = EditorUtility.SaveFilePanel("Save minimap", Application.dataPath, "Minimap", "png");
            File.WriteAllBytes(path, texture.EncodeToPNG());
            DestroyImmediate(texture);
            AssetDatabase.Refresh();
            #endif
        }
    }
}
