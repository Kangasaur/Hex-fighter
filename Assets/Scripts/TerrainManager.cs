using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [Header ("Prefabs")]
    [SerializeField] GameObject hexObject;
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    
    [Header ("Grid options")]
    [SerializeField] public float gridDistance = 1f;
    [SerializeField] int gridWidth = 10;
    [SerializeField] int gridHeight = 10;
    [SerializeField] float heightVariation = 0.5f;

    GameObject[,] grid;
    List<Vector2> gamePositions;

    [Header("Game setup")]
    [SerializeField] int enemyNum;
    [SerializeField] int cardNum;

    [Header("Visuals")]
    [SerializeField] Color colorVariation;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[gridWidth, gridHeight];

        //Generate a grid of hexagons
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                float newPosX = (float)j/2 == (int)((float)j/2) ? i * gridDistance : i * gridDistance + gridDistance / 2;
                float newPosZ = j * gridDistance * Mathf.Sin(Mathf.PI / 3);
                float newPosY = Random.Range(-heightVariation, heightVariation);
                GameObject newHex = Instantiate(hexObject, new Vector3(newPosX, newPosY, newPosZ), Quaternion.Euler(new Vector3(90, 0, 0)));

                //For visual variety change the color slightly
                Material mat = newHex.GetComponent<Renderer>().material;
                Color newColor = new(
                    Random.Range(mat.color.r - colorVariation.r, mat.color.r + colorVariation.r),
                    Random.Range(mat.color.b - colorVariation.b, mat.color.b + colorVariation.b),
                    Random.Range(mat.color.g - colorVariation.g, mat.color.g + colorVariation.g));
                mat.color = newColor;

                grid[i, j] = newHex;

            }
        }

        gamePositions = new List<Vector2>();
        gamePositions.Add(new Vector2(4, 4));

        player.transform.position = grid[(int)gamePositions[0].x, (int)gamePositions[0].y].transform.position + Vector3.up;

        for (int i = 0; i < enemyNum; i++)
        {
            Vector2 newPos = RandomEmptyPosition();
            gamePositions.Add(newPos);
            Instantiate(enemy, grid[(int)newPos.x, (int)newPos.y].transform.position + Vector3.up, Quaternion.Euler(Vector3.zero));
        }
    }

    /// <summary>
    /// Gets a position in the hex grid which isn't occupied by other game objects
    /// </summary>
    /// <returns></returns>
    Vector2 RandomEmptyPosition()
    {
        Vector2 newPosition = new Vector2(4, 4);
        while (gamePositions.Contains(newPosition))
        {
            int x = Random.Range(0, gridWidth);
            int y = Random.Range(0, gridHeight);
            newPosition = new Vector2(x, y);
        }
        return newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
