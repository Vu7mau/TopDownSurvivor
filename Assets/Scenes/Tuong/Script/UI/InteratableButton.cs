using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InteracbleButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private float waitTime = 30f;
    [SerializeField] private TextMeshProUGUI waitTimeText;
    private bool isCountingDown = false;
    private float timer;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void OnButtonClicked()
    {
        if (!isCountingDown && button.interactable)
        {
            button.interactable = false;    
            StartCoroutine(Interatacblebutton());
        }
    }
    private IEnumerator Interatacblebutton()
    {
        isCountingDown = true;
        timer = waitTime;
        while(timer > 0)
        {
            waitTimeText.text = Mathf.Max(0, timer).ToString("F0") + "s";
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
        waitTimeText.text = "Resend";
        button.interactable = true;
        isCountingDown = false;
    }
}
