using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Menu
{
    public override void OpenMenu()
    {
        base.OpenMenu();
        Cursor.lockState = CursorLockMode.None;
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        Cursor.lockState = CursorLockMode.None;
    }
}