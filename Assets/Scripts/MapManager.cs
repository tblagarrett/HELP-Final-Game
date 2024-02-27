using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap terrainMap;
    [SerializeField] private Tilemap objectMap;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private GameObject foodContainer;
    [SerializeField] private int howMuchFood;

    // Singleton Initialization
    private static MapManager _instance; // make a static private variable of the component data type
    public static MapManager Instance { get { return _instance; } } // make a public way to access the private variable

    // array of all food on the map
    // for monster manager to read
    public Vector2[] activeFood;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Makes the size of the tilemap be bounded to the outermost tiles, basically it makes sure the size is right
        terrainMap.CompressBounds();

        // Place down some food at the start
        PlaceFood(howMuchFood);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Generates food based on the size of the map, placed randomly within it
    void PlaceFood(int howManyFood)
    {
        int xSize = terrainMap.size[0];
        int ySize = terrainMap.size[1];

        // instatialize food array
        activeFood = new Vector2[howMuchFood];

        // places the given number of food in random spots throughout the map
        for (int i = 0; i < howManyFood; i++)
        {
            int x = Random.Range(-(xSize/2) + 1, xSize/2);
            int y = Random.Range(-(ySize/2) + 1, ySize/2);

            // Creates an instance of the object and sets its random position
            GameObject currentFood = Instantiate(foodPrefab, foodContainer.transform);
            currentFood.transform.position = new Vector3(x, y, 0);
            currentFood.layer = 10;

            // add food position to array
            activeFood[i] = currentFood.transform.position;
            currentFood.GetComponent<FoodScript>().foodArray = i;
        }
    }
}
