using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    static public FollowCam S; // a flowCam Singleton
    //fields set in the Unity Inspector pane
    public float easing = 0.05f;
    public Vector2 minXY;
    public bool ____________________;
    //fields set dynamically
    public GameObject poi;  // the Point Of Interest
    public float camZ; //the z-position of the camera

	// Not overiding Start(), overiding Awake() instead.
	void Awake () {
        S = this;
        camZ = this.transform.position.z;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (poi == null) return; //if poi isn't set yet dont run this part of the script


        // This makes the camera able to look L/R and up/down but not to move further away from the projectile/poi
        //get the position of the poi
        Vector3 destination = poi.transform.position;
        // Limit the X & Y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x);//returns max of a comparison
        destination.y = Mathf.Max(minXY.y, destination.y);
        // Interpolate from the current Cmaera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        // retain a destination.z of camZ
        destination.z = camZ; // change z cordinate to match camera
        //set the camera to the destination
        transform.position = destination;
        // set the orthographicSize of the Camera to keep Ground in view
        this.GetComponent<Camera>().orthographicSize = destination.y + 10;
	}
}
