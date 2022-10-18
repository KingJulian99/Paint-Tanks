using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadRotationLockController : MonoBehaviour
{
    // Get the target object
    GameObject target;

    public float threshold;

    // Start is called before the first frame update
    void Start()
    {
        this.target = this.transform.parent.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
    }

    void FixedUpdate() {
        this.transform.position = this.transform.parent.gameObject.transform.GetChild(0).transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = this.target.transform.position; 

        Vector3 heightCorrectedPoint = new Vector3(point.x, this.transform.position.y, point.z);

        // Point towards the target when the distance is above a certain point.
        if(DistanceFurtherThanThreshold(this.transform.position, heightCorrectedPoint, this.threshold)) {
            this.transform.Rotate(new Vector3(0, this.GetAngleBetweenThisAndPoint(heightCorrectedPoint) * 10f * Time.deltaTime, 0));
        }
        
    }

    bool DistanceFurtherThanThreshold(Vector3 pointOne, Vector3 pointTwo, float threshold) {
        if(Vector3.Distance(pointOne, pointTwo) > threshold) {
            return true;
        } else {
            return false;
        }
    }

    float GetAngleBetweenThisAndPoint(Vector3 correctedPoint) {
        Vector3 directionToPoint = -(this.transform.position - correctedPoint).normalized;
        return -Vector3.SignedAngle(directionToPoint, this.transform.forward, Vector3.up);
    }
}
