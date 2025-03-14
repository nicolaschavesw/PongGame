using UnityEngine;
using System.Collections;

public class BallSoundInteraction : MonoBehaviour
{
 [Space(5)]
    [Header("Tag y Sonidos")]
    public string selectedTag; // Tag de los objetos plataforma
    public AudioSource collisionSound; // Sonido de colision
    public AudioClip[] notes; // Arreglos de notas 
    public AudioClip[] notesStreched; // Arreglo de notas sostenidas
    [Header("Selección de Notas")]
    private AudioClip[] notesSelected; // Arreglo seleccionado
    public enum ArrayType { notes, notesStreched } // Enum para seleccionar
    public ArrayType selectedArray; // Variable seleccionable en el Inspector

    private void Start()
    {
        collisionSound = GetComponent<AudioSource>();

        switch (selectedArray)
        {
            case ArrayType.notes:
                notesSelected = notes;
                break;
            case ArrayType.notesStreched:
                notesSelected = notesStreched;
                break;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(selectedTag))
        {
            int randomNumber = Random.Range(0, notesSelected.Length);
            Debug.Log("salio el: " + randomNumber + "----:" + notesSelected[randomNumber].name + " sonido");
            collisionSound.PlayOneShot(notesSelected[randomNumber]);
        }
    }
}
