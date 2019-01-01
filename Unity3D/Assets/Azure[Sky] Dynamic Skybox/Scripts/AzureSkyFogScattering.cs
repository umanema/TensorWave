//Based on Unity's GlobalFog.
namespace UnityEngine.AzureSky
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Azure[Sky]/Fog Scattering")]
    public class AzureSkyFogScattering : MonoBehaviour
    {
        public Material fogBaseMaterial;
        public Material fogScatteringMaterial;
        public bool applyToTransparent = true;
        public LayerMask excludeLayers = 0;
        public CameraClearFlags clearFlags = CameraClearFlags.Nothing;
        private GameObject m_tmpCam;
        private Camera m_camera;

        [ImageEffectOpaque] // Apply the fog scattering effect after opaque geometry but before transparent geometry.
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
            if (fogScatteringMaterial == null || fogBaseMaterial == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            m_camera = GetComponent<Camera>();
            Transform cameraTransform = m_camera.transform;

            Vector3[] frustumCorners = new Vector3[4];
            m_camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), m_camera.farClipPlane, m_camera.stereoActiveEye, frustumCorners);
            var bottomLeft = cameraTransform.TransformVector(frustumCorners[0]);
            var topLeft = cameraTransform.TransformVector(frustumCorners[1]);
            var topRight = cameraTransform.TransformVector(frustumCorners[2]);
            var bottomRight = cameraTransform.TransformVector(frustumCorners[3]);

            Matrix4x4 frustumCornersArray = Matrix4x4.identity;
            frustumCornersArray.SetRow(0, bottomLeft);
            frustumCornersArray.SetRow(1, bottomRight);
            frustumCornersArray.SetRow(2, topLeft);
            frustumCornersArray.SetRow(3, topRight);

            // Apply fog to transparent objects.
            if (applyToTransparent)
            {
                // Gets the fog scattering data and stores it in a texture.
                RenderTexture fogScatteringTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
                fogBaseMaterial.SetMatrix("_FrustumCorners", frustumCornersArray);
                Graphics.Blit(null, fogScatteringTexture, fogBaseMaterial, 0);

                // Sends the data to all shaders that contain the scattering texture.
                Shader.SetGlobalTexture("_Azure_ScatteringTexture", fogScatteringTexture);

                RenderTexture.ReleaseTemporary(fogScatteringTexture);
            }

            // Apply the fog scattering effect.
            fogScatteringMaterial.SetMatrix("_FrustumCorners", frustumCornersArray);
            Graphics.Blit(source, destination, fogScatteringMaterial, 0);

            //Exclude layers.
            if (Application.isPlaying)
            {
                //m_camera = null;
                if (excludeLayers.value != 0)
                {
                    m_camera = GetTmpCam();
                }

                if (m_camera && excludeLayers.value != 0)
                {
                    //cam.targetTexture = destination;
                    m_camera.cullingMask = excludeLayers;
                    //cam.Render();
                }
            }
        }

        //From: https://github.com/UnityCommunity/UnityLibrary/blob/master/Assets/Shaders/ImageEffects/GrayscaleLayers.cs
        Camera GetTmpCam()
        {
            if (m_tmpCam == null)
            {
                if (m_camera == null) m_camera = GetComponent<Camera>();

                string name = "Exclude Fog Scattering";
                GameObject go = GameObject.Find(name);

                if (go == null)//Couldn't find, recreate.
                {
                    m_tmpCam = new GameObject(name, typeof(Camera));
                }
                else
                {
                    m_tmpCam = go;
                }
            }

            m_tmpCam.transform.position = transform.position;
            m_tmpCam.transform.rotation = transform.rotation;
            m_tmpCam.transform.localScale = transform.localScale;
            m_tmpCam.transform.parent = this.transform;
            m_tmpCam.GetComponent<Camera>().CopyFrom(m_camera);

            //tmpCam.GetComponent<Camera>().enabled = false;
            m_tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
            m_tmpCam.GetComponent<Camera>().clearFlags = clearFlags;
            m_tmpCam.GetComponent<Camera>().depth = m_camera.depth + 1;

            return m_tmpCam.GetComponent<Camera>();
        }
    }
}