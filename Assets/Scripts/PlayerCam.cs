using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float XSens;
    public float YSens;

    public Transform orientation;

    float xrotate;
    float yrotate;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * XSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * YSens * Time.deltaTime;

        yrotate += mouseX;
        xrotate -= mouseY;

        xrotate = Mathf.Clamp(xrotate, -90f, 90f);

        transform.rotation = Quaternion.Euler(xrotate, yrotate, 0);
        orientation.rotation = Quaternion.Euler(0, yrotate, 0);
    }
}

