using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerScripts : MonoBehaviour
{

    public int health;
    public int hunger;
    public int maxHealth;
    public int maxHunger;
    public SpriteRenderer sRen;
    private Rigidbody2D rb;
    public Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        sRen = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
