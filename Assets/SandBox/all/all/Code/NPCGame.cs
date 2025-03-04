using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCGame : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    public TMP_Text textNpc;
    private bool isPlayer = false;
    private bool isIntecracting = false;

    [SerializeField] private Transform panelLookAtPoint;
    private Transform playerCamera;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    [SerializeField] private GameObject exclamationMark;
    public float floatSpeed = 1f;
    public float floatHeight = 0.2f;
    private Vector3 intialMarkPosition;

    private void Start()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
        if (textNpc != null)
        {
            textNpc.gameObject.SetActive(false);
        }
        playerCamera = Camera.main.transform;
        originalCameraPosition = playerCamera.position;
        originalCameraRotation = playerCamera.localRotation;
        if(exclamationMark != null)
        {
            exclamationMark.gameObject.SetActive(true);
            intialMarkPosition = exclamationMark.transform.position;
        }
    }
    private void Update()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.F))
        {
            isIntecracting = !isIntecracting;
            interactionPanel.SetActive(isIntecracting);
            if(isIntecracting)
            {
                RoatateCameraToPanel();
                HideExclamationMark();

            }
            else
            {
                ResertCamera();
            }
        }
        if(exclamationMark != null && exclamationMark.activeSelf)
        {
            FloatingEffect();
        }
    }
    private void RoatateCameraToPanel()
    {
        if(playerCamera != null && panelLookAtPoint != null)
        {
            playerCamera.LookAt(panelLookAtPoint);
        }
    }
    private void ResertCamera()
    {
        if (playerCamera != null)
        {
            playerCamera.localRotation = originalCameraRotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayer = true;
            textNpc.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayer = false;
            interactionPanel.SetActive(false);
            textNpc.gameObject.SetActive(false);
            isIntecracting = false;
            ResertCamera();
        }
    }
    private void FloatingEffect()
    {
        float newY = intialMarkPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        exclamationMark.transform.position = new Vector3(intialMarkPosition.x, newY, intialMarkPosition.z);
    }
    private void HideExclamationMark()
    {
        if(exclamationMark != null)
        {
            exclamationMark.gameObject.SetActive(false);
        }
    }
}
