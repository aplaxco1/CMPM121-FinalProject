using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Camera sens
    public float sensX;
    public float sensY;

    //Camera objects
    public Transform followTarget;
    public Transform rotator;

    //Camera input/rotation
    float mouseX, mouseY;
    float multiplier = 0.01f;
    float xRotation, yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        MouseInput();

        followTarget.transform.localRotation = Quaternion.Euler(followTarget.transform.localEulerAngles - Vector3.right * xRotation);
        rotator.localRotation = Quaternion.Euler(rotator.localEulerAngles + Vector3.up * yRotation);
    }

    private void MouseInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation = mouseX * sensX * multiplier;
        xRotation = mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
