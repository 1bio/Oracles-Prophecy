using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 cameraRotation = mainCamera.transform.eulerAngles;
        transform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
    }
}
