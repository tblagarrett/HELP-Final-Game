using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VignetteClass : MonoBehaviour
{
    private Vignette vignetteComponent;
    private string direction = "increase";
    public float intensityChange;
    public bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        vignetteComponent = GetComponent<Vignette>();
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
            // If it is too instense, swap to decreasing
            if (direction == "increase" && vignetteComponent.intensity.GetValue<float>() > .5f)
            {
                direction = "decrease";
            }

            yield return new WaitForSeconds(.1f);
        }
    }
}
