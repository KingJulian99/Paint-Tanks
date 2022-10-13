using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoubleBarrelPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Tank")
        {
            SpawnTurret(collider.gameObject);

            DespawnPowerUp();
        }
    }

    private void SpawnTurret(GameObject tank)
    {
        var t = Resources.Load("double_barrel", typeof(GameObject));

        GameObject turret = tank.transform.GetChild(0).gameObject;
        Transform t_transform = turret.transform;

        Destroy(turret);

        if (t != null)
        {
            GameObject new_t = Instantiate(t, t_transform) as GameObject;
            new_t.transform.SetParent(tank.transform);
        }
    }

    private void DespawnPowerUp()
    {
        Destroy(gameObject, 0.2f);
    }
}
