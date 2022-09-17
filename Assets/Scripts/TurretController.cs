using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    public Camera viewCamera;
    public float maxRotationSpeed;
    public GameObject projectile;
    private GameObject muzzle;
    
    void Start()
    {
        this.muzzle = this.transform.GetChild(0).transform.GetChild(0).gameObject;
        this.viewCamera = Camera.main;
        this.maxRotationSpeed = 10f;
    }

    void Update()
    {
        Debug.DrawLine(this.muzzle.transform.position, this.muzzle.transform.position + this.muzzle.transform.forward * 10, Color.red);
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 pointOnPlane = ray.GetPoint(rayDistance);

            //Debug.DrawLine(ray.origin, pointOnPlane, Color.green);
            
            this.LookAt(pointOnPlane);
        }

        if(Input.GetMouseButtonDown(0)) {
            this.Shoot();
        }
    }

    void LookAt(Vector3 point) {
        Vector3 heightCorrectedPoint = new Vector3(point.x, this.transform.position.y, point.z);

        //Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 10, Color.red);

        this.transform.Rotate(new Vector3(0, this.GetAngleBetweenBarrelAndPoint(heightCorrectedPoint) * this.maxRotationSpeed * Time.deltaTime, 0));
    }

    float GetAngleBetweenBarrelAndPoint(Vector3 correctedPoint) {
        Vector3 directionToPoint = -(this.transform.position - correctedPoint).normalized;

        //Debug.DrawLine(this.transform.position, this.transform.position + directionToPoint * 10, Color.blue);

        return -Vector3.SignedAngle(directionToPoint, this.transform.forward, Vector3.up);
    }

    void Shoot() {
        GameObject shot = Instantiate(projectile, this.muzzle.transform.position, this.muzzle.transform.rotation);
    }
}
