using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : Menu
{
    public override void OpenMenu()
    {
        base.OpenMenu();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
    }
}