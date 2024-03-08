using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteClass : MonoBehaviour
{
    private Volume volumeComponent;
    private Vignette vignetteComponent;
    private string directionIntensity = "increase";
    public float intensityChange;
    public bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        volumeComponent = GetComponent<Volume>();
        volumeComponent.profile.TryGet(out vignetteComponent);
        
        vignetteComponent.color.Override(Color.black);
        vignetteComponent.intensity.Override(.288f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayVignetteHurt()
    {
        isPlaying = true;
        vignetteComponent.color.Override(Color.red);
        for (;;)
        {
            float currentIntensity = vignetteComponent.intensity.GetValue<float>();

            // If it is too instense, swap to decreasing
            if (directionIntensity == "increase" && currentIntensity > .5f)
            {
                directionIntensity = "decrease";
            }
            // If it reaches the bottom threshold, swap to increasing
            if (directionIntensity == "decrease" && currentIntensity < .288f)
            {
                directionIntensity = "increase";
            }

            // Adjust the intensity as necessary
            if (directionIntensity == "increase")
            {
                vignetteComponent.intensity.Override(currentIntensity + intensityChange);
            }
            if (directionIntensity == "decrease")
            {
                vignetteComponent.intensity.Override(currentIntensity - intensityChange);
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    public void StopVignetteHurt()
    {
        isPlaying = false;
        StopCoroutine(PlayVignetteHurt());
        vignetteComponent.color.Override(Color.black);
        vignetteComponent.intensity.Override(.288f);
    }
}
