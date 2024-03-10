using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoneMenu : Menu
{
    private List<GameObject> hearts;
    private List<GameObject> foodSprites;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Sprite heartFilled;
    [SerializeField] private Sprite heartEmpty;
    [SerializeField] private Sprite foodFilled;
    [SerializeField] private Sprite foodEmpty;
    private void Start()
    {
        hearts = new List<GameObject>();
        foodSprites = new List<GameObject>();


        // Get the height of the canvas
        RectTransform canvasRectTransform = GetComponent<RectTransform>();
        float canvasHeight = canvasRectTransform.rect.height;

        // Spawn the heart sprites
        for (int i = 0; i < 10; i++)
        {
            GameObject heart = Instantiate(heartPrefab, this.gameObject.transform, false);

            // Calculate the y-position relative to the top of the canvas
            float yPos = canvasHeight;

            heart.GetComponent<RectTransform>().position = new Vector3(50 + 50 * i, -150);
            hearts.Add(heart);
        }

        // Spawn the food sprites
        for (int i = 0; i < 10; i++)
        {
            GameObject foodSprite = Instantiate(foodPrefab, this.gameObject.transform, false);

            // Calculate the y-position relative to the top of the canvas
            float yPos = canvasHeight;

            foodSprite.GetComponent<RectTransform>().position = new Vector3(50 + 50 * i, -190);
            foodSprites.Add(foodSprite);
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