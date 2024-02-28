using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneMenu : Menu
{

    private void Start()
    {
    }

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