using UnityEngine;
using UnityEngine.UI;
public class AlphaClickFix : MonoBehaviour
{
    private void OnEnable()
    {
       GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
