using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurretScript : MonoBehaviour
{
    [SerializeField]
    public GameObject parent;

    // Update is called once per frame
    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        float ray_length;

        if(ground.Raycast(mouseRay, out ray_length))
        {
            Vector3 pointToLook = mouseRay.GetPoint(ray_length);
            Debug.DrawLine(mouseRay.origin, pointToLook, Color.cyan);

            //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(pointToLook.x, 0, pointToLook.z));
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turn_speed);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}
