using UnityEngine;
public class EmergencyLight : MonoBehaviour
{
    public Light redLight;
    public Light blueLight;
    public float flashSpeed = 5f; 
    public float intensity = 15f;
    void Update()
    {
        float t = Mathf.PingPong(Time.time * flashSpeed, 1f);

        redLight.enabled = t < 0.5f;
        blueLight.enabled = t >= 0.5f;

        redLight.intensity = intensity;
        blueLight.intensity = intensity;
    }
}
