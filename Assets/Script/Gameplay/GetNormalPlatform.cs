using UnityEngine;

public class GetNormalPlatform : MonoBehaviour
{
    public Vector3 normalPlatform;

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        // Obtiene la normal de la plataforma (Vector Up)
        normalPlatform = transform.up;

        // Dibuja una línea que representa la normal
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + normalPlatform * 2);
    }
}
