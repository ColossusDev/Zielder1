using System.Collections;
using System.Collections.Generic;
using PathFind;
using UnityEngine;
using System.Linq;

public class JobQueue
{
    int maxLenght;
    List<Job> jobQueue;

    public JobQueue(int maxLenght)
    {
        this.maxLenght = maxLenght;
        jobQueue = new List<Job>();
    }

    public bool AddJob(GameObject start, GameObject goal, float dist) //return if queue is full or not
    {
        if (maxLenght == jobQueue.Count)
        {
            return true;
        }
        Job tempJob = new Job(start, goal, dist);
        if (JobIsInList(tempJob))
        {
            return false;
        }
        jobQueue.Add(tempJob);
        return false;
    }

    public int GetCount()
    {
        return jobQueue.Count;
    }

    public Job GetNextJob()
    {
        for (int i = 0; i < jobQueue.Count; i++)
        {
            if (jobQueue[i].GetInProgress() == false)
            {
                jobQueue[i].SetInProgress();
                return jobQueue[i];
            }
        }
        return null;
    }

    public void JobIsDone(Job doneJob)
    {
        jobQueue.Remove(doneJob);
    }

    bool JobIsInList(Job j)
    {
        for (int i = 0; i < jobQueue.Count; i++)
        {
            if (j.getGoal() == jobQueue[i].getGoal())
            {
                return true;
            }
        }
        return false;
    }
}
