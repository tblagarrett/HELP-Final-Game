using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    // variables for monster
    public int health;
    public int hunger;

    // variables for radius of view
    // How far away the monster can see depending on state
    public int curVisRadius;
    public int maxVisRadius;
    public int minVisRadius;

    // collider
    [SerializeField] private BoxCollider2D body;

    private void Start()
    {
        body = GetComponent<BoxCollider2D>();
    }



}
