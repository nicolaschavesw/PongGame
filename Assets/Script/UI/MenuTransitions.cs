using System;
using System.Collections;
using UnityEngine;

public class MenuTransitions : MonoBehaviour
{
    private Camera menuCamera;
    private GameObject menuCameraGameObject;
    [SerializeField] private Transform menuTransform;
    [SerializeField] private Transform levelTransform;
    void Start()
    {
        menuCamera = Camera.main ?? FindAnyObjectByType<Camera>();
        if (menuCamera == null)
        {
            Debug.LogWarning("No se encontró ninguna cámara en la escena.");
        }
        menuCameraGameObject = menuCamera.gameObject;
    }
    void Update()
    {

    }

    public void SetMenuCameraToMenu()
    {
        StartCoroutine(MoveCamera(menuTransform));
    }
    public void SetMenuCameraToLevel()
    {
        StartCoroutine(MoveCamera(levelTransform));
    }

    private IEnumerator MoveCamera(Transform targetPosition)
    {
        float duration = 1.5f; // Duración de la transición en segundos
        float elapsedTime = 0;
        Vector3 startPosition = menuCameraGameObject.transform.position;
        Quaternion startRotation = menuCameraGameObject.transform.rotation;

        while (elapsedTime < duration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsedTime / duration);
            menuCameraGameObject.transform.position = Vector3.Lerp(startPosition, targetPosition.position, t);
            menuCameraGameObject.transform.rotation = Quaternion.Slerp(startRotation, targetPosition.rotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        menuCameraGameObject.transform.position = targetPosition.position; // Asegurar posición final exacta
        menuCameraGameObject.transform.rotation = targetPosition.rotation;
    }
}
