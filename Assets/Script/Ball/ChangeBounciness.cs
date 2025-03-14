using UnityEngine;

public class ChangeBounciness : MonoBehaviour
{
    private Rigidbody rbBall;
    [SerializeField] private float inputValue = 1.0f;  // Puede cambiar dinámicamente
    [SerializeField] private float outputValue = 1.0f;  // Salida
    [SerializeField] private float inverseLerpValue = 1.0f;  // Entrada
    void Start()
    {
        rbBall = GetComponent<Rigidbody>();
    }
    void Update()
    {
        inputValue = rbBall.linearVelocity.magnitude;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("velocidad antes de colisionar: " + rbBall.linearVelocity.magnitude);
        if(inputValue > 30.0f)
        {
            inverseLerpValue = 0;
        }
        else
        {
            inverseLerpValue = 1.0f - Mathf.InverseLerp(1.0f, 30.0f, inputValue);
        }
        outputValue = Mathf.Lerp(0.3f, 2.3f, inverseLerpValue);
        rbBall.linearVelocity *= outputValue;
        Debug.Log("velocidad despues de colisionar: " + rbBall.linearVelocity.magnitude);
    }

}