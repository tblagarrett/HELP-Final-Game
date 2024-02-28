using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    protected bool isActive = false;
    protected bool isDirty = false;
    protected Canvas myCanvas;

    private void Awake()
    {
        if (myCanvas == null)
            myCanvas = GetComponent<Canvas>();
        InnerAwake();
    }

    protected virtual void InnerAwake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (isDirty)
        {
            isDirty = false;
            RefreshMenu();
        }

        InnerUpdate();
    }

    protected virtual void InnerUpdate()
    {

    }

    public virtual void RefreshMenu()
    {
        //do anything you need to 'reset' the menu to its expected state
        isDirty = false;
    }

    public virtual void CloseMenu()
    {
        // Any code that needs to run when a menu closes
        myCanvas.enabled = false;
    }

    public virtual void OpenMenu()
    {
        // Any code that needs to run when a menu first opens
        myCanvas.enabled = true;
    }
}