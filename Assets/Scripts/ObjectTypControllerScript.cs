using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTypControllerScript : MonoBehaviour
{

    [SerializeField] List<Sprite> spriteList;

    public void SelectType(int i)
    {
        if (i <= spriteList.Count)
        {
            this.GetComponent<SpriteRenderer>().sprite = spriteList[i];
        }
        else
        {
            Debug.Log("Type selected witch is not in list");
        }
    }

    public void ChooseRandomType()
    {
        int rng = Random.Range(0, spriteList.Count);
        this.GetComponent<SpriteRenderer>().sprite = spriteList[rng];
    }

    public void ChooseRandomFlip()
    {
        int rngFlip = Random.Range(0, 2);
        if (rngFlip == 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void ChooseRandomScale(float min, float max)
    {
        float scaleRng = Random.Range(min, max);
        this.transform.localScale = new Vector3(scaleRng, scaleRng, 1);
    }
}
