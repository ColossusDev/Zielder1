using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using PathFind;

public class WorkerScript : MonoBehaviour
{

    public GameObject mapController;
    public GameObject workstation;

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
        mapController = GameObject.Find("MapController");
    }

    public void SetWorkstation(GameObject ws)
    {
        workstation = ws;
    }

    public int energy = 1;
    public bool working = false;
    public bool onBase = false;
    public bool onTarget = false;
    public bool movingToBase = false;
    public bool movingToTarget = false;

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

        foreach (var wo in GetEntities<Componets>())
        {
            if (wo.work.movingToBase == false && wo.work.movingToTarget == false)
            {
                if (wo.work.working == false && wo.work.energy > 0)
                {
                    wo.work.working = true;
                }
                else if (wo.work.working == true)
                {
                    if (wo.work.myJob != null)
                    {
                        if (wo.work.myJob.getGoal() == null)
                        {
                            wo.work.workstation.GetComponent<ObjectJobControllerScript>().cuttingJobs.JobIsDone(wo.work.myJob);
                            wo.work.myJob = null;
                            Debug.Log("Job entfernt");
                        }
                    }

                    if (wo.work.onBase == false && (wo.work.myJob == null || wo.resource.count >= wo.resource.maxWeight))
                    {
                        wo.work.movingToBase = true;
                        wo.work.onTarget = false;
                    }
                    else if (wo.work.onBase == true && (wo.work.myJob == null || wo.resource.count > 0))
                    {
                        if (wo.work.myJob == null)
                        {
                            wo.work.myJob = wo.work.workstation.GetComponent<ObjectJobControllerScript>().nextFreeJob;
                            wo.work.workstation.GetComponent<ObjectJobControllerScript>().nextFreeJob = null;
                        }
                        if (wo.resource.count > 0)
                        {
                            bool transfered = wo.work.workstation.GetComponent<ObjectResourceControllerScript>().PushResource(wo.resource.type, wo.resource.count);

                            if (transfered == true)
                            {
                                wo.resource.count = 0;
                            }
                        }
                    }
                    else if (wo.work.onTarget == false && wo.work.myJob != null && wo.resource.count == 0)
                    {
                        wo.work.movingToTarget = true;
                        wo.work.onBase = false;
                    }
                    else if (wo.work.onTarget == true && wo.work.myJob != null && wo.resource.count < wo.resource.maxWeight)
                    {
                        if (wo.work.timeToResource < (1 / wo.work.gatherSpeed))
                        {
                            wo.work.timeToResource += timer;
                        }
                        else if (wo.work.timeToResource >= (1/wo.work.gatherSpeed))
                        {
                            wo.resource.PushResource(wo.resource.type, 1);
                            wo.work.timeToResource = 0;
                            wo.work.myJob.getGoal().GetComponent<ObjectResourceControllerScript>().PullResource(wo.resource.type, 1);
                            if (wo.work.myJob.getGoal().GetComponent<ObjectResourceControllerScript>().count <= 0)
                            {
                                GameObject temp =  wo.work.myJob.getGoal();
                                wo.work.workstation.GetComponent<ObjectJobControllerScript>().cuttingJobs.JobIsDone(wo.work.myJob);
                                wo.work.myJob = null;
                                Debug.Log("Job entfernt");
                                temp.GetComponent<ObjectScript>().DestroyThisObject();
                            }
                        }
                    }
                    else if (wo.work.onTarget == true && wo.work.myJob != null && wo.resource.count >= wo.resource.maxWeight)
                    {
                        wo.work.movingToBase = true;
                        wo.work.onTarget = false;
                    }
                    else if (wo.work.onBase == true && wo.work.myJob != null && wo.resource.count >= wo.resource.maxWeight)
                    {
                        bool transfered = wo.work.workstation.GetComponent<ObjectResourceControllerScript>().PushResource(wo.resource.type, wo.resource.count);

                        if (transfered == true)
                        {
                            wo.resource.count = 0;
                        }
                    }
                }
            }



            //Move Routine
            if (wo.work.movingToBase == true)
            {
                if (wo.work.wayToTarget == null)
                {
                    wo.work.wayToTarget = wo.work.mapController.GetComponent<MapController>().Astar(wo.obs.posX, wo.obs.posY, wo.work.workstation.GetComponent<ObjectScript>().posX, wo.work.workstation.GetComponent<ObjectScript>().posY -1);
                }
                else if (wo.work.wayToTarget.Count == 0)
                {
                    wo.work.wayToTarget = null;
                    wo.work.movingToBase = false;
                    wo.work.onBase = true;
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

            if (wo.work.movingToTarget == true)
            {
                if (wo.work.wayToTarget == null)
                {
                    wo.work.wayToTarget = wo.work.mapController.GetComponent<MapController>().Astar(wo.obs.posX, wo.obs.posY, wo.work.myJob.getGoal().GetComponent<ObjectScript>().posX, wo.work.myJob.getGoal().GetComponent<ObjectScript>().posY - 1);
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