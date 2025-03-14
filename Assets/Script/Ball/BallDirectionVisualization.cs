using UnityEngine;

public class BallDirectionVisualization : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnDrawGizmos()
    {
        if (rb == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rb.linearVelocity);
    }
}
