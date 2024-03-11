using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoneMenu : Menu
{
    public List<GameObject> hearts;
    public List<GameObject> foodSprites;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Sprite heartFilled;
    [SerializeField] private Sprite heartEmpty;
    [SerializeField] private Sprite foodFilled;
    [SerializeField] private Sprite foodEmpty;
    private void Start()
    {
    }

    // HEALTH MUST BE BETWEEN MIN HEALTH AND MAX HEALTH
    public void setHearts(int health)
    {
        int healthToHearts = health - 1;
        for (int i = 0; i < 10; i++)
        {
            if (i <= healthToHearts)
            {
                hearts[i].GetComponent<Image>().sprite = heartFilled;
            } else
            {
                hearts[i].GetComponent<Image>().sprite = heartEmpty;
            } 
        }
    }

    // HEALTH MUST BE BETWEEN MIN HUNGER AND MAX HUNGER
    public void setHunger(int hunger)
    {
        int hungerToSprites = hunger - 1;
        for (int i = 0; i < 10; i++)
        {
            if (i <= hungerToSprites)
            {
                foodSprites[i].GetComponent<Image>().sprite = foodFilled;
            }
            else
            {
                foodSprites[i].GetComponent<Image>().sprite = foodEmpty;
            }
        }
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