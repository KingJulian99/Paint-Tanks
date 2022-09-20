using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RotateTurretScript : MonoBehaviour
{
    private Vector3 pointToLook;
    private bool isLocalPlayer;

    private void Start()
    {
        isLocalPlayer = gameObject.transform.root.GetComponent<PlayerTank>().isLocalPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer) { return; }

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float ray_length;

        if(ground.Raycast(mouseRay, out ray_length))
        {
            pointToLook = mouseRay.GetPoint(ray_length);
            Debug.DrawLine(mouseRay.origin, pointToLook, Color.cyan);

            //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(pointToLook.x, 0, pointToLook.z));
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turn_speed);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}
