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
        public ObjectProcessControllerScript process;
    }

    protected override void OnUpdate()
    {
        foreach (var obj in GetEntities<Componets>())
        {
            GameObject[] allProcessHuts = GameObject.FindGameObjectsWithTag(obj.process.processorTag);

            if (obj.obs.fullGrow == true || obj.obs.nonGrow == true)
            {
                foreach (GameObject go in allProcessHuts)
                {
                    Vector3 diff = go.transform.position - obj.transform.position;
                    float curDistance = diff.sqrMagnitude;

                    //process Range kann ich mir auch aus dem Gebäude holen und somit je nach gebäude die Range einstellbar machen
                    if (curDistance < obj.process.processorRange)
                    {
                        bool full = go.GetComponent<ResourceGatheringScript>().cuttingJobs.AddJob(go, obj.obs.me, curDistance);
                    }
                }
            }
        }
    }
}
