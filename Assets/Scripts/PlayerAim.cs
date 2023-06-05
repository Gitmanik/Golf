using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private void Update()
    {
        transform.position = BallController.Instance.transform.position;
    }
    private void OnMouseDown()
    {
        if (!BallController.Instance.IsMoving)
            BallController.Instance.StartAiming();
    }
}