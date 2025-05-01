using UnityEngine;

namespace Utils
{
    public class FixedResolution : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        
        public int targetWidth = 240;
        public int targetHeight = 320;
        private RenderTexture _renderTexture;

        private void Start()
        {
            _renderTexture = new RenderTexture(targetWidth, targetHeight, 24, RenderTextureFormat.RGB565);
            _renderTexture.filterMode = FilterMode.Point; // Keep sharp pixels
            if (targetCamera == null) targetCamera = Camera.main;
            if (targetCamera != null) targetCamera.targetTexture = _renderTexture;
        }

        void OnGUI()
        {
            if (_renderTexture == null) return;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _renderTexture, ScaleMode.StretchToFill);
        }
    }
}