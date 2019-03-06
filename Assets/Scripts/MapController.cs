using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] int mapSizeX = 5;
    [SerializeField] int mapSizeY = 5;
    [Space]
    [SerializeField] GameObject tile;
    [SerializeField] GameObject tree;
    [SerializeField] GameObject bush;
    [SerializeField] GameObject stone;

    [SerializeField] [Range(1, 19)] int treeSpawn = 5;
    [SerializeField] [Range(1, 19)] int stoneSpawn = 5;

    [SerializeField] bool spawnTrees = false;
    [SerializeField] bool spawnStones = false;

    GameObject[,] groundMap;
    float[,] navMesh;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
        GenerateMap();
    }

    void StartGame()
    {
        groundMap = new GameObject[mapSizeX, mapSizeY];
        navMesh = new float[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                navMesh[x, y] = 1f;
            }
        }
    }

    void GenerateMap()
    {
        //Generate all Tiles
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                GameObject obj = Instantiate(tile);
                obj.GetComponent<TileControllerScript>().posX = x;
                obj.GetComponent<TileControllerScript>().posY = y;
                obj.transform.position = new Vector2(x * 0.25f, y * 0.25f);
                obj.name = "TileX" + x + "Y" + y;
                obj.transform.SetParent(GameObject.Find("GroundTileMap").transform);
                groundMap[x, y] = obj;
            }
        }

        // Generate all Objects
        // die Art und Weise wie Objekte erstellt werden sollte noch verbessert werden
        // so, dass man auch im Menü konfigurieren kann, wieviel von was gespawned wird
        for (int x = 1; x < mapSizeX - 1; x++)
        {
            for (int y = 1; y < mapSizeY - 1; y++)
            {
                int caseRng = Random.Range(1, 3);

                if (caseRng == 1 && spawnTrees == true)
                {
                    int rng = Random.Range(treeSpawn, 20);

                    if (rng == 19)
                    {
                        GameObject obj = Instantiate(tree);
                        obj.GetComponent<ObjectTypControllerScript>().ChooseRandomType();
                        obj.GetComponent<ObjectGrowControllerScript>().RandomGrowth(25f,100f);
                        obj.transform.position = new Vector2(x * 0.25f, y * 0.25f);
                        obj.GetComponent<ObjectScript>().posX = x;
                        obj.GetComponent<ObjectScript>().posY = y;
                        obj.name = "tree";
                        obj.transform.SetParent(GameObject.Find("ObjectTileMap").transform);
                        navMesh[x, y] = 0.0f;
                    }
                }
                else if (caseRng == 2 && spawnStones == true)
                {
                    int rng = Random.Range(stoneSpawn, 20);

                    if (rng == 19)
                    {
                        GameObject obj = Instantiate(stone);
                        obj.GetComponent<ObjectTypControllerScript>().ChooseRandomType();
                        obj.GetComponent<ObjectTypControllerScript>().ChooseRandomFlip();
                        obj.GetComponent<ObjectTypControllerScript>().ChooseRandomScale(0.35f,0.75f);
                        obj.transform.position = new Vector2(x * 0.25f, y * 0.25f);
                        obj.GetComponent<ObjectScript>().posX = x;
                        obj.GetComponent<ObjectScript>().posY = y;
                        obj.name = "stone";
                        obj.transform.SetParent(GameObject.Find("ObjectTileMap").transform);
                        navMesh[x, y] = 0.0f;
                    }
                }

            }
        }
    }

    public bool BuildObject(GameObject building, int x, int y)
    {
        if (navMesh[x,y] >= 1 && navMesh[x, y-1] >= 1)
        {
            GameObject obj = Instantiate(building);
            obj.transform.position = new Vector2(x * 0.25f, y * 0.25f);
            obj.GetComponent<ObjectScript>().posX = x;
            obj.GetComponent<ObjectScript>().posY = y;
            obj.transform.SetParent(GameObject.Find("ObjectTileMap").transform);
            navMesh[obj.GetComponent<ObjectScript>().posX, obj.GetComponent<ObjectScript>().posY] = 0.0f;
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<PathFind.Point> Astar(int startX, int startY, int targetX, int targetY)
    {
        PathFind.Grid grid = new PathFind.Grid(mapSizeX, mapSizeY , navMesh);

        PathFind.Point _from = new PathFind.Point(startX, startY);
        PathFind.Point _to = new PathFind.Point(targetX, targetY);

        return PathFind.Pathfinding.FindPath(grid, _from, _to);
    }

    //Funktion get Way to nearest Tile of the Type (water, sand, stone....)
}
