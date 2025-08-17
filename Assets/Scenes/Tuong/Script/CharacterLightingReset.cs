    using UnityEngine;
    using UnityEngine.SceneManagement;
    public class CharacterLightingReset : MonoBehaviour
    {
        public GameObject characterModel;

        public Material originalMaterial;

        public bool createTempLight = false;
        public Color lightColor = Color.white;
        public float intensity = 1f;

        private Light tempLight;

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (characterModel != null && originalMaterial != null)
            {
                Renderer rend = characterModel.GetComponent<Renderer>();
                if (rend != null)
                    rend.material = originalMaterial;
            }

            DynamicGI.UpdateEnvironment();

            if (createTempLight && tempLight == null)
            {
                GameObject lightObj = new GameObject("TempDirectionalLight");
                tempLight = lightObj.AddComponent<Light>();
                tempLight.type = LightType.Directional;
                tempLight.color = lightColor;
                tempLight.intensity = intensity;
                tempLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            }
        }
    }
