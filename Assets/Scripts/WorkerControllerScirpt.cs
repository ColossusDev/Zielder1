using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerControllerScirpt : MonoBehaviour
{
    List<GameObject> workerList;
    [SerializeField] GameObject worker;

    private void Start()
    {
        workerList = new List<GameObject>();

        AddWorker();
    }

    void AddWorker()
    {
        Vector3 v = new Vector3(1.25f, 1.25f, 0);
        GameObject go = Instantiate(worker, v, Quaternion.identity);
        go.GetComponent<ObjectScript>().posX = 5;
        go.GetComponent<ObjectScript>().posY = 5;
        workerList.Add(go);
    }


}
