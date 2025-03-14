using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessModificator : MonoBehaviour
{
    public Volume volume;
    private DepthOfField dof;
    private ColorAdjustments ca;
    private WinLoseCollision winLoseCollision;
    void Start()
    {
        // Busca el Volume en la escena
        winLoseCollision = GameObject.FindGameObjectWithTag("Ball").GetComponent<WinLoseCollision>();

        // Intenta obtener el DepthOfField del Volume
        if (volume != null && volume.profile.TryGet(out dof))
        {
            //Debug.Log("Depth of Field encontrado!");
        }
        if (volume != null && volume.profile.TryGet(out ca))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (winLoseCollision == null) return;
        if (winLoseCollision.BallWin())
        {
            DepthOfFieldMod();
        }
        if (winLoseCollision.BallLose())
        {
            
        }
    }

    public void DepthOfFieldMod()
    {
        if (dof != null) // Asegurar que dof no sea nulo
        {
            float newFocusDistance = Mathf.Lerp(dof.focusDistance.value, 1.0f, Time.deltaTime * 2);
            dof.focusDistance.Override(newFocusDistance);
        }
    }

    public void ColorAdjustmentsLose()
    {
        if (ca != null)
        {
            float newPostExposure = Mathf.Lerp(ca.postExposure.value, 1.0f, Time.deltaTime * 10);
            float newSaturarion = Mathf.Lerp(ca.saturation.value, -100.0f, Time.deltaTime * 5);
            ca.postExposure.Override(newPostExposure);
            //ca.saturation.Override(newSaturarion);
        }
    }
    
}
