using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWorkerControllerScript : MonoBehaviour
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
        Vector3 v = new Vector3(this.transform.position.x, this.transform.position.y - 0.25f, this.transform.position.z);
        GameObject go = Instantiate(worker, v, Quaternion.identity);
        go.GetComponent<ObjectScript>().posX = this.GetComponent<ObjectScript>().posX;
        go.GetComponent<ObjectScript>().posY = this.GetComponent<ObjectScript>().posY - 1;
        go.GetComponent<WorkerScript>().SetWorkstation(this.gameObject);
        workerList.Add(go);
    }
}
