using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;

    [Header("Zoom")]
    [SerializeField] float zoomSpeed = 12f;
    [SerializeField] float maxZoom = 16;
    [SerializeField] float minZoom = 4;

    private Camera cameraComp;

    void Start()
    {
        this.cameraComp = this.GetComponent<Camera>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            var direction = (new Vector3(h, v, 0)).normalized;
            this.transform.position = this.transform.position + (direction * this.moveSpeed * Time.deltaTime);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            float zoomDelta = (Input.mouseScrollDelta.y * (-1)) * this.zoomSpeed * Time.deltaTime;
            this.SetZoom(this.cameraComp.orthographicSize + zoomDelta);
        }
    }

    public void SetZoom(float nextZoom)
    {
        this.cameraComp.orthographicSize = Mathf.Clamp(nextZoom, this.minZoom, this.maxZoom);
    }
}