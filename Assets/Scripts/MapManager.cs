using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tile foodTile;
    [SerializeField] private Tilemap terrainMap;
    [SerializeField] private Tilemap objectMap;
    [SerializeField] private int howMuchFood;

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

        // places the given number of food in random spots throughout the map
        for (int i = 0; i < howManyFood; i++)
        {
            int x = Random.Range(-(xSize/2) + 1, xSize/2);
            int y = Random.Range(-(ySize/2) + 1, ySize/2);
            objectMap.SetTile(new Vector3Int(x, y, 1), foodTile);
        }
    }
}
