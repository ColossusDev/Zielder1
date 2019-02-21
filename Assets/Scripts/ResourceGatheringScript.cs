using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ResourceGatheringScript : MonoBehaviour
{
    public bool isBuild = false;
    int posX;
    int posY;

    public JobQueue cuttingJobs;
    [SerializeField] public Job nextFreeJob;
    [SerializeField] public int jobListCount;

    List<GameObject> workerList;
    [SerializeField] GameObject worker;

    string resourceType = "wood";
    [SerializeField] int resourceAmount = 0;
    int maxCarry = 10000;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetupBuilding()
    {
        isBuild = true; 

        cuttingJobs = new JobQueue(50);
        workerList = new List<GameObject>();

        posX = this.gameObject.GetComponent<ObjectScript>().posX;
        posY = this.gameObject.GetComponent<ObjectScript>().posY;

        AddWorker();
    }

    void AddWorker()
    {
        Vector3 v = new Vector3(this.transform.position.x, this.transform.position.y - 0.25f, this.transform.position.z);
        GameObject go = Instantiate(worker, v, Quaternion.identity);
        go.GetComponent<ObjectScript>().posX = posX;
        go.GetComponent<ObjectScript>().posY = posY - 1;
        go.GetComponent<WorkerScript>().SetWorkstation(this.gameObject);
        workerList.Add(go);
    }

    public void GiveJob(GameObject start, GameObject goal, float dist)
    {
        cuttingJobs.AddJob(start, goal, dist);
    }

    public bool transferResources(int w)
    {
        if (resourceAmount < maxCarry)
        {
            resourceAmount = resourceAmount + w;
            return true;
        }
        return false;
    }


}

class RGControllerSystem : ComponentSystem
{
    struct Componets
    {
        public ResourceGatheringScript rgs;
        public ObjectScript obs;
    }

    protected override void OnUpdate()
    {
        foreach (var obj in GetEntities<Componets>())
        {
            if (obj.rgs.nextFreeJob == null)
            {
                obj.rgs.nextFreeJob = obj.rgs.cuttingJobs.GetNextJob();
            }
            obj.rgs.jobListCount = obj.rgs.cuttingJobs.GetCount();
        }
    }
}

