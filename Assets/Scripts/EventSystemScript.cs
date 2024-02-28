using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemScript : MonoBehaviour
{
    private static EventSystemScript _instance; // make a static private variable of the component data type
    public static EventSystemScript Instance { get { return _instance; } } // make a public way to access the private variable
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
