using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public static BallController Instance;

    private Rigidbody rb;

    private LineRenderer lr;
    private Camera mc;

    public float maxNaprezenie;

    [SerializeField] private LayerMask ForceLineLayerMask;
    [SerializeField] private Transform ForceLineCollider;
    [SerializeField] private float ForceLineColliderY = -20f;

    public bool IsAiming { get; private set; }
    public bool IsMoving => rb.velocity.magnitude > 0.025f;

    public float shotpower;
    private Vector3 savedpos;
    private MeshCollider mm;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 1000;
        mc = Camera.main;
        lr.enabled = false;
        Instance = this;
    }

    public void Setup()
    {
        ForceLineCollider.position = new Vector3(0, transform.position.y + ForceLineColliderY, 0);
        ForceLineCollider.gameObject.SetActive(true);
        savedpos = GameObject.Find("SpawnPosition").transform.position;
        mm = LevelManager.Instance.LevelTransform.GetComponent<MeshCollider>();
    }

    private void Update()
    {
        if (!IsMoving)
            StopBall();

        if (IsMoving && IsAiming)
            StopAiming();

        ProcessAim();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform == LevelManager.Instance.LevelTransform)
        {
            mm.material.bounciness = collision.contactCount > 0 ? .2f : 0f;
        }
    }

    private void ProcessAim()
    {
        if (!IsAiming || IsMoving)
            return;

        Vector3? worldPoint = CastMouseClickRay();

        if (!worldPoint.HasValue)
            return;

        Vector3 wp = worldPoint.Value;
        wp.y = transform.position.y;

        Vector3 dir = wp - transform.position;

        wp = dir.magnitude > maxNaprezenie ? (transform.position + dir.normalized * maxNaprezenie) : wp;

        DrawForceLine(wp);

        if (Input.GetMouseButtonUp(0))
            ShootBall(wp);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (IsAiming)
            return;

        if (collision.transform == ForceLineCollider)
        {
            Loader.Instance.RestartLevel();
        }

        if (collision.name == "FinishPosition")
        {
            ForceLineCollider.gameObject.SetActive(false);
            Loader.Instance.OnFinishedLevel();
        }
    }

    public void StartAiming()
    {
        ForceLineCollider.position = transform.position;
        Physics.SyncTransforms();
        IsAiming = true;
    }

    private void StopAiming()
    {
        IsAiming = false;
        lr.enabled = false;
        ForceLineCollider.position = new Vector3(0, transform.position.y + ForceLineColliderY, 0);
    }

    private void ShootBall(Vector3 wp)
    {
        StopAiming();

        Vector3 dir = -(wp - transform.position).normalized;
        float str = Vector3.Distance(transform.position, wp);

        rb.AddForce(shotpower * str * dir, ForceMode.Impulse);
        LevelManager.Instance.OnShot();
    }

    private void StopBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void DrawForceLine(Vector3 worldPoint)
    {
        Vector3[] pos =
        {
            transform.position,
            worldPoint
        };

        lr.SetPositions(pos);
        lr.enabled = true;
    }

    private Vector3? CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mc.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mc.nearClipPlane);

        Vector3 worldMousePosFar = mc.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = mc.ScreenToWorldPoint(screenMousePosNear);

        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out RaycastHit hit, float.PositiveInfinity, ForceLineLayerMask))
            return hit.point;
        else
            return null;
    }
}
