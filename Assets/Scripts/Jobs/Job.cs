using System.Collections;
using System.Collections.Generic;
using PathFind;
using UnityEngine;

public class Job
{
    GameObject start;
    GameObject goal;
    public float dist;

    bool inProgress = false;


    public Job(GameObject start, GameObject goal, float dist)
    {
        this.start = start;
        this.goal = goal;
        this.inProgress = false;
    }

    public Point GetStartPoint()
    {
        return new Point((int)start.transform.position.x * 4, (int)start.transform.position.y * 4);
    }

    public Point GetEndPoint()
    {
        return new Point((int)goal.transform.position.x * 4, (int)goal.transform.position.y * 4);
    }

    public float GetWork()
    {
        return goal.GetComponent<ObjectResourceControllerScript>().rescource;
    }

    public void SetInProgress()
    {
        inProgress = true;
    }

    public void SetWaiting()
    {
        inProgress = false;
    }

    public bool GetInProgress()
    {
        return inProgress;
    }

    public GameObject getGoal()
    {
        return goal;
    }

    public bool SameJobCheck(Job jobCheck)
    {
        if (this.goal == jobCheck.goal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
