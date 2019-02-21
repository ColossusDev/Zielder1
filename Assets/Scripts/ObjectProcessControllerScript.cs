using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ObjectProcessControllerScript : MonoBehaviour
{
    public string processorTag;
    public int processorRange;

}

class ObjectProcessControllerSystem : ComponentSystem
{
    struct Componets
    {
        public Transform transform;
        public ObjectScript obs;
        public ObjectGrowControllerScript grow;
        public ObjectProcessControllerScript process;
    }

    protected override void OnUpdate()
    {
        foreach (var obj in GetEntities<Componets>())
        {
            GameObject[] allCutterHuts = GameObject.FindGameObjectsWithTag(obj.process.processorTag);

            if (obj.grow.growIndicator >= obj.grow.growTime)
            {
                foreach (GameObject go in allCutterHuts)
                {
                    Vector3 diff = go.transform.position - obj.transform.position;
                    float curDistance = diff.sqrMagnitude;

                    if (curDistance < obj.process.processorRange)
                    {
                        bool full = go.GetComponent<ResourceGatheringScript>().cuttingJobs.AddJob(go, obj.obs.me, curDistance);
                    }
                }
            }
        }
    }
}
