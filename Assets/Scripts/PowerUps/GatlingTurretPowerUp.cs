using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingTurretPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Tank" && collider.gameObject.transform.Find("Turret").gameObject != null)
        {
            SpawnTurret(collider.gameObject);

            DespawnPowerUp();
        }

        if (collider.gameObject.tag == "GamepadTank" && collider.gameObject.transform.Find("GamepadTurret").gameObject != null)
        {
            SpawnGamepadTurret(collider.gameObject);

            DespawnPowerUp();
        }
    }

    private void SpawnTurret(GameObject tank)
    {
        var t = Resources.Load("GatlingTurret", typeof(GameObject));

        GameObject turret = tank.transform.Find("Turret").gameObject;
        Transform t_transform = turret.transform;

        Destroy(turret);

        if (t != null)
        {
            GameObject new_t = Instantiate(t, t_transform) as GameObject;
            new_t.transform.SetParent(tank.transform);
            new_t.name = "GatlingTurret";
            new_t.GetComponent<GatlingTurretController>().setTeamColor(Color.magenta);
        }
    }

    private void SpawnGamepadTurret(GameObject tank)
    {
        var t = Resources.Load("GamepadGatlingTurret", typeof(GameObject));

        GameObject turret = tank.transform.Find("GamepadTurret").gameObject;
        Transform t_transform = turret.transform;

        Destroy(turret);

        if (t != null)
        {
            GameObject new_t = Instantiate(t, t_transform) as GameObject;
            new_t.transform.SetParent(tank.transform);
            new_t.name = "GamepadGatlingTurret";
            new_t.GetComponent<GamepadGatlingTurretController>().setTeamColor(Color.magenta);
        }
    }

    private void DespawnPowerUp()
    {
        Destroy(gameObject, 0.2f);
    }
}
