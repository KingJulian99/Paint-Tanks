using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour
{
    private GameObject map;
    private GameObject previousSelected;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform m in gameObject.transform)
        {
            m.gameObject.GetComponent<SelectMapTrigger>().Clicked += GetMap;
        }
        
    }

    public void GetMap(GameObject go)
    {
        if (!go.GetComponentInChildren<Toggle>().isOn)
        {
            // Select new panel
            go.GetComponentInChildren<Toggle>().isOn = true;
            go.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.5f);

            //Deselect previously selected
            if (previousSelected != null && previousSelected != go)
            {
                previousSelected.GetComponentInChildren<Toggle>().isOn = false;
                previousSelected.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }

            map = go.GetComponent<MapProperties>().map;
        }
        else
        {
            go.GetComponentInChildren<Toggle>().isOn = false;
            go.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }


        previousSelected = go;
    }

    public GameObject GetSelectedMap()
    {
        return map;
    }
}
