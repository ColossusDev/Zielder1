using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobController : MonoBehaviour
{

    //hier fehlen noch JobQueue ähnliche Funktionenen und Strukturen
    public List<Job> allJobs;
    public int jobListCount;

    private void Start()
    {
        allJobs = new List<Job>();
    }

    private void Update()
    {
        jobListCount = allJobs.Count;
    }

    public bool JobInList(Job j)
    {
        for (int i = 0; i < allJobs.Count; i++)
        {
            if (allJobs[i].getGoal() == j.getGoal())
            {
                return true;
            }
        }
        return false;
    }

    public void AddJobToList(Job j)
    {
        allJobs.Add(j);
    }

    public void JobIsDone(Job j)
    {
        allJobs.Remove(j);
    }

    public Job NextFreeJob()
    {

        for (int i = 0; i < allJobs.Count; i++)
        {
            if (allJobs[i].GetInProgress() == false)
            {
                return allJobs[i];
            }
        }
        return null;
    }
}
