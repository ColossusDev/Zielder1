using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using PathFind;

public class WorkerScript : MonoBehaviour
{

    public MapController mapController;
    public JobController jobController;

    bool storageFull = false;

    public Job myJob = null;
    string resourceType = null;
    [SerializeField] int resourceAmount = 0;
    int maxCarry = 5;

    public List<PathFind.Point> wayToTarget = null;
    public float nextWayPointX;
    public float nextWayPointY;
    public bool nextWayPoint = false; 

    public  float walkSpeed = 1f;
    public float gatherSpeed = 1f;

    public float timeToResource = 0F;

    // Start is called before the first frame update
    void Start()
    {
        mapController = GameObject.Find("MapController").GetComponent<MapController>();
        jobController = GameObject.Find("JobController").GetComponent<JobController>();
    }

    public float energy = 1;
    public float food = 1;
    public float water = 1;


    public bool onBase = false;
    public bool onTarget = false;
    public bool movingToTarget = false;
    public Point currentTarget;

}

class WorkerControllerSystem : ComponentSystem
{
    struct Componets
    {
        public WorkerScript work;
        public ObjectScript obs;
        public ObjectResourceControllerScript resource;
        public Transform transform;
    }

    protected override void OnUpdate()
    {
        float timer = Time.deltaTime;
        float Ttimer = Time.deltaTime / 10;

        foreach (var wo in GetEntities<Componets>())
        {
            wo.work.water -= Ttimer;
            wo.work.food -= Ttimer;
            wo.work.energy -= Ttimer;

            //Job Routine
            if (wo.work.myJob == null)
            {
                wo.work.myJob = wo.work.jobController.NextFreeJob(wo.work);
            }
            else if (wo.work.myJob.done == true)
            {
                wo.work.myJob = wo.work.jobController.NextFreeJob(wo.work);
            }
            else if (wo.work.myJob.done == false)
            {

            }


            if (wo.work.movingToTarget == true)
            {
                if (wo.work.wayToTarget == null)
                    {
                        wo.work.wayToTarget = wo.work.mapController.Astar(wo.obs.posX, wo.obs.posY, wo.work.currentTarget.x, wo.work.currentTarget.y - 1);
                    }
                    else if (wo.work.wayToTarget.Count == 0)
                    {
                        wo.work.wayToTarget = null;
                        wo.work.movingToTarget = false;
                        wo.work.onTarget = true;
                        wo.work.nextWayPoint = false;
                    }
                    else if (wo.work.nextWayPoint == false)
                    {
                        wo.work.nextWayPointX = wo.work.wayToTarget[0].x * 0.25f;
                        wo.work.nextWayPointY = wo.work.wayToTarget[0].y * 0.25f;
                        wo.work.nextWayPoint = true;
                    }
                    else if (Vector3.Distance(new Vector3(wo.transform.position.x, wo.transform.position.y, 0), new Vector3(wo.work.nextWayPointX, wo.work.nextWayPointY, 0)) <= 0.01f)
                    {
                        wo.obs.posX = wo.work.wayToTarget[0].x;
                        wo.obs.posY = wo.work.wayToTarget[0].y;
                        wo.work.wayToTarget.RemoveAt(0);
                        wo.work.nextWayPoint = false;
                    }
                    else if (Vector3.Distance(new Vector3(wo.transform.position.x, wo.transform.position.y, 0), new Vector3(wo.work.nextWayPointX, wo.work.nextWayPointY, 0)) >= 0.01f)
                    {
                        wo.transform.position = Vector3.MoveTowards(wo.transform.position, new Vector3(wo.work.nextWayPointX, wo.work.nextWayPointY, 0), timer * wo.work.walkSpeed);
                    }
            }
        }
    }
}