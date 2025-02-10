using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class h_rotateCharacter : MonoBehaviour
{
    public Transform character;
    public float rotationSpeed = 5f;
    private bool isRotating = false;
    private Vector3 lastMousePosition;    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;            
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            isRotating = false;
        }
        if (isRotating)
        {            
            Vector3 delta = Input.mousePosition - lastMousePosition;
            character.Rotate(Vector3.up, -delta.x * rotationSpeed * Time.deltaTime);
            lastMousePosition = Input.mousePosition;
        }
    }
    public void clickout()
    {
        isRotating = false;
    }
}
