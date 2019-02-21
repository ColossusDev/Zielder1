using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public int posX;
    public int posY;

    public GameObject me;

    private void Start()
    {
        me = this.gameObject;
    }

    public void DestroyThisObject()
    {
        Destroy(this.gameObject);
    }
}
