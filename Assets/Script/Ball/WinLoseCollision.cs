using UnityEngine;

public class WinLoseCollision : MonoBehaviour
{
    private bool win, lose;

    private void Start() {
        win = false;
        lose = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Win"))
        {
            win = true;
        }

        if (collision.gameObject.CompareTag("Lose"))
        {
            lose = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lose"))
        {
            lose = true;
        }
    }

    public bool BallWin()
    {
        return win;
    }

    public bool BallLose()
    {
        return lose;
    }
}
