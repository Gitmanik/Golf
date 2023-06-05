using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private Camera mc;
    private Vector3 startingPosition;

    public bool EnableCameraRotation => !BallController.Instance.IsAiming;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mc = Camera.main;
    }

    void LateUpdate()
    {
        transform.position = BallController.Instance.transform.position;

        if (EnableCameraRotation)
            RotateCamera();
    }

    private void RotateCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startingPosition = mc.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 newPos = mc.ScreenToViewportPoint(Input.mousePosition);
            Vector3 dir = startingPosition - newPos;

            transform.Rotate(Vector3.up, -dir.x * 180);

            startingPosition = newPos;
        }
    }
}
 