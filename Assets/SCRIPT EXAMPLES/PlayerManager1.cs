using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float distanceToGround;
    public int currentHealth;
    public int maxHealth;
    private static PlayerManager _instance; // make a static private variable of the component data type
    public static PlayerManager Instance { get { return _instance; } } // make a public way to access the private variable\
    public GameObject Player { get { return GameObject.FindWithTag("Player"); } }

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
        // This will place a shadow under the player every half second
        StartCoroutine(PlaceShadow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlaceShadow()
    {
        for(;;)
        {
            // Spawn the shadow directly down from the player every loop
            Ray ray = new Ray(this.Player.transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(this.Player.transform.position, Vector3.down, out hit, distanceToGround))
            {
                // Get a shadow from our pool, and set it to the position of the raycast, destroying it after its lifespan
                GameObject shadow = ShadowPool.Instance.GetPooledObject();
                if (ShadowPool.Instance.GetPooledObject() != null)
                {
                    shadow.transform.position = hit.point + new Vector3(0, .1f, 0);
                    shadow.SetActive(true);
                    StartCoroutine(ShadowPool.Instance.Destroy(shadow));
                }
            }

            yield return new WaitForSeconds(.1f);
        }
    }
}