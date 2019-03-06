using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ObjectProcessControllerScript : MonoBehaviour
{
    public string processorTag;
    public int processorRange;
    public JobController jobController;

    private void Start()
    {
        jobController = GameObject.Find("JobController").GetComponent<JobController>();
    }
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

                    //process Range sollte ich mir auch aus dem Gebäude holen und somit je nach gebäude die Range einstellbar machen
                    if (obj.obs.inJobList == false && curDistance < obj.process.processorRange)
                    {
                        obj.obs.inJobList = true;
                        obj.process.jobController.AddJobToList(new Job(go, obj.obs.me));
                    }
                }
            }
        }
    }
}
