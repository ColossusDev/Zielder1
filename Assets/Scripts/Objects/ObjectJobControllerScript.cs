using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ObjectJobControllerScript : MonoBehaviour
{
    public JobQueue cuttingJobs;
    public Job nextFreeJob;
    [SerializeField] public int jobsAmount;

    private void Start()
    {
        cuttingJobs = new JobQueue(25);
    }

    public void GiveJob(GameObject start, GameObject goal, float dist)
    {
        cuttingJobs.AddJob(start, goal, dist);
    }
}

class ObjectJobControllerSystem : ComponentSystem
{
    struct Componets
    {
        public ObjectJobControllerScript objcs;
        public ObjectResourceControllerScript orcs;
        public ObjectScript obs;
    }

    protected override void OnUpdate()
    {
        foreach (var obj in GetEntities<Componets>())
        {
            if (obj.objcs.nextFreeJob == null)
            {
                obj.objcs.nextFreeJob = obj.objcs.cuttingJobs.GetNextJob();
            }
            obj.objcs.jobsAmount = obj.objcs.cuttingJobs.GetCount();
        }
    }
}