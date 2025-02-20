using UnityEngine;

public class CameraScript : MonoBehaviour {

    private Vector3 screenRight = new Vector3(Screen.width - 30, 0, 0);
    private Vector3 screenLeft = new Vector3(30, 0, 0);
    private Vector3 screenTop = new Vector3(0, Screen.height - 30, 0);
    private Vector3 screenBottom = new Vector3(0, 30, 0);

    public int cameraSpeed = 5;
    public float mouseApproachSpeed;

    void Update()
    {
        MouseMovement();
    }

    void MouseMovement()
    {
        //Horizontal Mouse movement
        if (Input.mousePosition.x >= screenRight.x)
        {
            mouseApproachSpeed = -((Screen.width - 30) - Input.mousePosition.x) / 30;
            transform.Translate(mouseApproachSpeed * (Time.deltaTime * cameraSpeed), 0, 0);
        }
        else if (Input.mousePosition.x <= screenLeft.x)
        {
            mouseApproachSpeed = (30 - Input.mousePosition.x) / 30;
            transform.Translate(-mouseApproachSpeed * (Time.deltaTime * cameraSpeed), 0, 0);
        }

        //Vertical Mouse movement
        if (Input.mousePosition.y >= screenTop.y)
        {
            mouseApproachSpeed = -((Screen.height - 30) - Input.mousePosition.y) / 30;
            transform.Translate(0, 0, mouseApproachSpeed * (Time.deltaTime * cameraSpeed));
        }
        else if (Input.mousePosition.y <= screenBottom.y)
        {
            mouseApproachSpeed = (30 - Input.mousePosition.y) / 30;
            transform.Translate(0, 0, -mouseApproachSpeed * (Time.deltaTime * cameraSpeed));
        }
    }
}
