using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class controls the slingshot, yo.
/// </summary>
public class Slingshot : MonoBehaviour {
    // fields manually set in inspector. class fields - show up in inspector
    public GameObject launchPoint;
    public GameObject Projectile_Prefab;
    public float velocityMultiplier = 4f;
    public bool _____________;//purely decorative. used to seperate sections of variables in the scripts inspector
    // fields set dynamically
    public Vector3 launchPos;//has x,y,z coordinates for where projetile starts launch from
    public GameObject projectile;
    public bool aimingMode;//control game state

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint"); //find unity gameobject "LaunchPoint", create transform variable (has coordinates)
        launchPoint = launchPointTrans.gameObject; //= to coordinates of unity game object
        launchPoint.SetActive(false); //make launchpoint(halo) inactive
        launchPos = launchPointTrans.position;
    }
    void OnMouseDown()
    {
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true; //we are now in aiming mode
        // Instantiate a projetile
        projectile = Instantiate(Projectile_Prefab) as GameObject; //links VS projectile to unity's prefab
        // Start it at the launchPoint
        projectile.transform.position = launchPos;
        // Set it to isKenematic
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.isKinematic = true;

    }
    void OnMouseEnter()
    {
        launchPoint.SetActive(true);//turns on halo when mouse in
        print("Slingshot:OnMouseEnter()");
    }
    void OnMouseExit()
    {
        launchPoint.SetActive(false);//turns halo off when mouse out
        print("Slingshot:OnMouseExit()");
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        // If slingshot is not in aimingmode, don't run this code
        if (!aimingMode) return;
        // get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        // convert the mouse position to 3D world coordinates
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        // find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        // limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // move the projectile to the new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            // the mouse has been released
            aimingMode = false;
            projectile.GetComponent<Rigidbody>().isKinematic = false;
            projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMultiplier;
            FollowCam.S.poi = projectile;
            projectile = null;
        }
    }
}
