using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float movementTime;
    private Vector3 newPosition;

    private float zoom;
    private float zoomMultiplier=4f;
    private float minZoom=2f;
    private float maxZoom=8f;
    private float velocity=0f;
    private float smoothTime=0.25f;
    [SerializeField] private Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        zoom = cam.orthographicSize;
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom-= scroll*zoomMultiplier;
        zoom = Mathf.Clamp(zoom,minZoom,maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
    }
    private void MovementInput()
    {
        if(Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow)) {
            newPosition += (transform.up * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.up * -moveSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -moveSpeed);
        }



        transform.position= Vector3.Lerp(transform.position, newPosition, Time.deltaTime* movementTime);
    }
}
