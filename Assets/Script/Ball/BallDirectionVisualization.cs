using UnityEngine;

public class BallDirectionVisualization : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private bool visualizeDirection = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnDrawGizmos() //Solo se ve en el editor, no en el juego
    {
        if (rb == null || !visualizeDirection) return;

        Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, rb.linearVelocity);
        Gizmos.DrawLine(transform.position, transform.position + rb.linearVelocity);
    }
}
