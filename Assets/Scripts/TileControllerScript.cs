using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControllerScript : MonoBehaviour
{
    public int tileType;

    [SerializeField] Sprite gras;
    [SerializeField] Sprite sand;
    [SerializeField] Sprite stone;
    [SerializeField] Sprite water;

    public int posX;
    public int posY;

    void Start()
    {

    }

    public void SetUpTile(int tileType)
    {
        this.tileType = tileType;

        // hier fehlt noch die Spritewahl je nach Tile Type der übergeben wird...
    }
}
