using UnityEngine;

public class CameraZoomManager : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float minZoom = 5;
    public float maxZoom = 25f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize  - scroll * zoomSpeed, minZoom, maxZoom);
    }
}