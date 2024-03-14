using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionDropdown : MonoBehaviour
{
    public TMP_Dropdown d;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeResolution()
    {
        switch (d.value)
        {
            case 0:
                Screen.SetResolution(1280, 960, false);
                break;
            case 1:
                Screen.SetResolution(1920, 1440, false);
                break;
            case 2:
                Screen.SetResolution(800, 600, false);
                break;
            default:
                break;
        }
       
    }
}
