using UnityEditor;
using UnityEngine;

public class CameraStopper : MonoBehaviour
{

    private new CameraController camera;

    void Start()
    {
        camera = GameObject.FindFirstObjectByType<CameraController>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && camera != null) camera.StopCameraMove();
    }
}
