using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuChangeButton : MonoBehaviour
{
    [SerializeField] private GameMenu targetMenu;
    private UIManager UIMan;
    // Start is called before the first frame update
    void Start()
    {
        Button thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(GoToMenu);

        UIMan = UIManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GoToMenu()
    {
        UIMan.GoToMenu(targetMenu);
    }


}