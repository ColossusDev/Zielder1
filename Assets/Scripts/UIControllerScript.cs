using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerScript : MonoBehaviour
{
    [SerializeField] GameObject overBuildingMenu;
    [SerializeField] Image resourceMenu;
    [SerializeField] Image buildingMenu;
    [SerializeField] Image gatheringBuildingMenu;

    [SerializeField] Text woodText;
    [SerializeField] Text stoneText;
    [SerializeField] Text ironText;
    [SerializeField] Text foodText;

    [SerializeField] GameObject playerController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawResources();
    }

    public void SwitchResourceMenu()
    {
        if (resourceMenu.enabled == true)
        {
            resourceMenu.enabled = false;

            for (int i = 0; i < resourceMenu.transform.childCount; i++)
            {
                var child = resourceMenu.transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(false);
            }
        }
        else
        {
            resourceMenu.enabled = true;

            for (int i = 0; i < resourceMenu.transform.childCount; i++)
            {
                var child = resourceMenu.transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(true);
            }
        }
    }


    public void SwitchBuildingMenu()
    {
        if (overBuildingMenu.activeInHierarchy == true)
        {
            overBuildingMenu.SetActive(false);

            for (int i = 0; i < buildingMenu.transform.childCount; i++)
            {
                var child = buildingMenu.transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(false);
            }
        }
        else
        {
            overBuildingMenu.SetActive(true);

            for (int i = 0; i < overBuildingMenu.transform.childCount; i++)
            {
                var child = overBuildingMenu.transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(true);
            }
        }
    }

    public void SwitchToGatheringBuilding()
    {
        for (int i = 0; i < buildingMenu.transform.childCount; i++)
        {
            var child = buildingMenu.transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(false);
        }

        gatheringBuildingMenu.enabled = true;

        for (int i = 0; i < gatheringBuildingMenu.transform.childCount; i++)
        {
            var child = gatheringBuildingMenu.transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(true);
        }

    }

    void DrawResources()
    {
        if (resourceMenu.enabled == true)
        {
            woodText.GetComponent<Text>().text = playerController.GetComponent<PlayerControllerScript>().player.wood.ToString();
            stoneText.GetComponent<Text>().text = playerController.GetComponent<PlayerControllerScript>().player.stone.ToString();
        }
    }
}
