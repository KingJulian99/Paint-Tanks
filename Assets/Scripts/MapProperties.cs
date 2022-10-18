using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapProperties : MonoBehaviour
{
    public GameObject map;
    public string mapName;

    private void Start()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(mapName);
    }
}
