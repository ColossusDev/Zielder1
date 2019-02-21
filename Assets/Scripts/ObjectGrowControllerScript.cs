using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ObjectGrowControllerScript : MonoBehaviour
{
    public float growIndicator;
    public float growTime = 100;
    public float growSpeed = 1;

    public void RandomGrowth(float min, float max)
    {
        float rng = Random.Range(min, max);
        growIndicator = rng;
    }
}

class ObjectGrowControllerSystem : ComponentSystem
{
    struct Componets
    {
        public Transform transform;
        public ObjectScript obs;
        public ObjectGrowControllerScript grow;
        public ObjectResourceControllerScript resource;
    }

    protected override void OnUpdate()
    {
        float t = Time.deltaTime;

        foreach (var obj in GetEntities<Componets>())
        {
            if (obj.grow.growIndicator < obj.grow.growTime)
            {
                obj.grow.growIndicator += t * obj.grow.growSpeed;
                obj.resource.rescource = 1 + (int)(obj.grow.growIndicator / 11);

                float scaleByGrowth = obj.grow.growIndicator / 100;

                obj.transform.localScale = new Vector3(scaleByGrowth, scaleByGrowth, 1);
            }
        }
    }
}