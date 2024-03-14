using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMenu
{
    None,
    MainMenu,
    Options,
    Credits,
    GameOver
}

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Menus; // array of menus to be used, objects in the scene
    public GameMenu startingMenu;
    public GameMenu currentMenu;
    public KeyCode nextKey;
    public KeyCode[] menuKeys; // these should be linked to Menus by order entered. 1st key opens 1st menu, etc.

    private static UIManager _instance; // make a static private variable of the component data type
    public static UIManager Instance { get { return _instance; } } // make a public way to access the private variable
    private void Awake()
    {
        if (_instance != null && _instance != this)
        { // if there is already a value assigned to the private variable and its not this, destroy this
            Destroy(this.gameObject);
        }
        else
        { // if there is no value assigned to the private variable, assign this as the reference
            _instance = this;

        }
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 960, false);

        // Menus = GameObject.FindGameObjectsWithTag( "Menu" );
        foreach (GameObject menu in Menus)
        {
            if (menu.GetComponent<Menu>() == null)
            {
                Debug.LogError("No Menu found on " + menu.name);
                continue;
            }
            menu.GetComponent<Menu>().CloseMenu(); //make sure all menus start closed
            menu.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        OpenMenu(startingMenu); //open a starting menu if one is declared

    }

    // Update is called once per frame
    void Update()
    {
        CheckMenuInput();
    }

    public void CheckMenuInput()
    {
        // Quick debug solution for menu changing. Would recommend adding Input Manager buttons
        if (Input.GetKeyDown(nextKey))
        {
            if ((int)currentMenu == Menus.Length - 1)
            {
                GoToMenu((GameMenu)0);
                return;
            }
            GoToMenu((GameMenu)(int)currentMenu + 1);
            return;
        }
        for (int n = 0; n < menuKeys.Length; n++)
        {
            if (Input.GetKeyDown(menuKeys[n]))
            {
                GoToMenu((GameMenu)n);
                return;
            }
        }
    }

    private bool CloseMenu(GameMenu Menu)
    {
        Menus[(int)Menu].GetComponent<Menu>().CloseMenu();

        return true;
    }

    private bool OpenMenu(GameMenu Menu)
    {
        Menus[(int)Menu].GetComponent<Menu>().OpenMenu();
        currentMenu = Menu;

        return true;
    }

    public void GoToMenu(GameMenu Menu)
    {
        if (currentMenu == Menu)
        {
            Debug.LogWarning("Cannot move to " + Menu + ", currently on");
        }
        else
        {
            CloseMenu(currentMenu);
            currentMenu = Menu;
            OpenMenu(currentMenu);
        }
    }

    public GameObject GetMenu(GameMenu Menu)
    {
        return Menus[(int)Menu];
    }

    public void SetHearts(int health)
    {
        // Call the setHearts function in the NoneMenu script
        Menus[(int) GameMenu.None].GetComponent<NoneMenu>().setHearts(health);
    }

    public void SetHunger(int hunger)
    {
        // Call the setHunger function in the NoneMenu script
        Menus[(int) GameMenu.None].GetComponent<NoneMenu>().setHunger(hunger);
    }

    // How to make exit game button https://www.youtube.com/watch?v=6nenEHhcNwQ&ab_channel=ThegamedevTraum
    public void Exit()
    {
        Application.Quit();
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        GoToMenu(GameMenu.None);
    }
}