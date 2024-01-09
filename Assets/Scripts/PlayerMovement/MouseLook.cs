using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //private PlayerInputs _input;
    public float mouseSensitivityX = 0.5f;
    public float mouseSensitivityY = 0.5f;
    public float mouseSensitivity = 0.5f;
    float mouseX, mouseY;
    public Transform playerBody;
    float xRotation = 0f;
    Vector2 mouseInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //mouse input you are receiving is already a delta value
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        /*mouseInput.x = _input.Player.MouseX.ReadValue<float>();
        mouseInput.y = _input.Player.MouseY.ReadValue<float>();*/

        /*mouseX = mouseInput.x * mouseSensitivityX;
        mouseY = mouseInput.y * mouseSensitivityY;*/

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
