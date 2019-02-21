using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public Player player;
    bool isBuilding = false;
    [SerializeField] GameObject mapController;
    GameObject blueprintObject;
    [SerializeField] Camera camera1;

    // Start is called before the first frame update
    void Start()
    {
        player = new Player();
            
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            InBuildingModus();
        }
    }

    public void ActivateBuildingModus(GameObject blueprint)
    {
        isBuilding = true;
        blueprintObject = Instantiate(blueprint);
        blueprintObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    void InBuildingModus()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            blueprintObject.GetComponent<SpriteRenderer>().enabled = true;
            blueprintObject.transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, 0);
        }
        else
        {
            blueprintObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            bool success = mapController.GetComponent<MapController>().BuildObject(blueprintObject.GetComponent<BlueprintScript>().building, hit.collider.gameObject.GetComponent<TileControllerScript>().posX, hit.collider.gameObject.GetComponent<TileControllerScript>().posY);
            if (success)
            {
                isBuilding = false;
                Destroy(blueprintObject);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
                isBuilding = false;
                Destroy(blueprintObject);
        }
    }
}
