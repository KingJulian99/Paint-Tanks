using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TripleBarrelPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Tank" && collider.gameObject.transform.Find("Turret") != null)
        {
            SpawnTurret(collider.gameObject);

            DespawnPowerUp();
        }

        if (collider.gameObject.tag == "GamepadTank" && collider.gameObject.transform.Find("GamepadTurret").gameObject != null)
        {

            print("gamepadtank collided");
            SpawnGamepadTurret(collider.gameObject);

            DespawnPowerUp();
        }
    }

    private void SpawnTurret(GameObject tank)
    {
        var t = Resources.Load("TripleBarrel", typeof(GameObject));

        GameObject turret = tank.transform.Find("Turret").gameObject;
        Transform t_transform = turret.transform;

        Destroy(turret);

        if (t != null)
        {
            GameObject new_t = Instantiate(t, t_transform) as GameObject;
            new_t.transform.SetParent(tank.transform);
            new_t.name = "TripleBarrel";
            //new_t.GetComponent<DoubleBarrelTurretController>().setTeamColor(Color.black);
        }
    }

    private void SpawnGamepadTurret(GameObject tank)
    {
        var t = Resources.Load("GamepadTripleBarrel", typeof(GameObject));

        GameObject turret = tank.transform.Find("GamepadTurret").gameObject;
        Transform t_transform = turret.transform;

        Destroy(turret);

        if (t != null)
        {
            GameObject new_t = Instantiate(t, t_transform) as GameObject;
            new_t.transform.SetParent(tank.transform);
            new_t.name = "GamepadTripleBarrel";
            //new_t.GetComponent<DoubleBarrelTurretController>().setTeamColor(Color.black);
        }
    }

    private void DespawnPowerUp()
    {
        Destroy(gameObject, 0.2f);
    }
}
