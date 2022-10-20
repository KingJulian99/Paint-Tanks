using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShotPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Tank" && collider.gameObject.transform.Find("Turret").gameObject != null)
        {
            SpawnTurret(collider.gameObject);

            DespawnPowerUp();
        }
    }

    private void SpawnTurret(GameObject tank)
    {
        var t = Resources.Load("BigShot", typeof(GameObject));

        GameObject turret = tank.transform.Find("Turret").gameObject;
        Transform t_transform = turret.transform;

        Destroy(turret);

        if (t != null)
        {
            GameObject new_t = Instantiate(t, t_transform) as GameObject;
            new_t.transform.SetParent(tank.transform);
            new_t.name = "BigShot";
            new_t.GetComponent<BigShotTurretController>().setTeamColor(Color.magenta);
        }
    }

    private void DespawnPowerUp()
    {
        Destroy(gameObject, 0.2f);
    }
}
