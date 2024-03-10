using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoneMenu : Menu
{
    private List<GameObject> hearts;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Sprite heartFilled;
    [SerializeField] private Sprite heartEmpty;
    private void Start()
    {
        hearts = new List<GameObject>();


        // Get the height of the canvas
        RectTransform canvasRectTransform = GetComponent<RectTransform>();
        float canvasHeight = canvasRectTransform.rect.height;

        for (int i = 0; i < 10; i++)
        {
            GameObject heart = Instantiate(heartPrefab, this.gameObject.transform, false);

            // Calculate the y-position relative to the top of the canvas
            float yPos = canvasHeight;

            heart.GetComponent<RectTransform>().position = new Vector3(50 + 50 * i, -150);
            hearts.Add(heart);
        }
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