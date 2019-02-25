using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectResourceControllerScript : MonoBehaviour
{
    //need refactor
    public int maxWeight;

    public Resource.Res type;
    public int count;
   
  
    public bool PullResource(Resource.Res type, int amount)
    {
            if (this.type == type)
            {
                if (count >= amount)
                {
                    count -= amount;
                    return true;
                }
            }
        Debug.Log("Falscher Type oder voller Lagerplatz");
        return false;
    }

    public bool PushResource(Resource.Res type, int amount)
    {
            if (this.type == type)
            {
                if (count <= maxWeight)
                {
                    count += amount;
                    return true;
                }
            }
        return false;
    }
}
