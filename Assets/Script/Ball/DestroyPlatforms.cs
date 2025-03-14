using UnityEngine;

public class DestroyPlatforms : MonoBehaviour
{
    public string selectedTag; // Tag de los objetos plataforma

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(selectedTag))
        {
            DestroyAllWithTag(selectedTag);
        }
    }

    void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}
