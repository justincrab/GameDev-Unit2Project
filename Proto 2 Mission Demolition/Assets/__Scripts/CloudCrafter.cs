using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// uses two arrays, one holds prefabs; other holds instances
public class CloudCrafter : MonoBehaviour {
    // fields set in the Unity Inspector pane
    public int numClouds = 40;          // the # of clouds to make
    public GameObject[] cloudPrefabs;   // the prefabs of the clouds
    public Vector3 cloudPosMin;         // min position
    public Vector3 cloudPosMax;         // max position
    public float cloudScaleMin = 1;     // min scale
    public float cloudScaleMax = 5;     // max scale
    public float cloudSpeedMult = 0.5f; //speed multiplier

    public bool _______________________________;
    //  fields set dynamically
    public GameObject[] cloudInstances; //holds instances

	// Use this for initialization

        // change Start() to Awake()
	void Awake () {
        // Make an array large enough to hold all of the clouds
        cloudInstances = new GameObject[numClouds]; //numClouds controls how many entries in array
        //  find the cloud anchor parent gameObject
        GameObject anchor = GameObject.Find("CloudAnchor");
        // iterate through and make our Cloud_s
        GameObject cloud;
        for (int i=0; i < numClouds; i++)
        {
            // First Pick an integer between 0 and cloudPrefabs.length - 1
            // random.range will not ever pick as high as top number
            int prefabNum = Random.Range(0, cloudPrefabs.Length);

            // make na instance
            cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject; //HUHUHUH
            //position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //smaller clouds (with smaller scaleU) should be nearer to gorund
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //samller clouds should be further away
            cPos.z = 100 - 90 * scaleU;
            //apply these transformations
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //make the cloud a child of the anchor
            cloud.transform.parent = anchor.transform;
            // add cloud to cloudInstances array
            cloudInstances[i] = cloud;
        }
	}
	
	// Update is called once per frame
	void Update () {
		// Iterate over each cloud that was created
        foreach(GameObject cloud in cloudInstances)
        {
            // get the scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            // move larger clouds faster
            // take scale value times delta times cloud speed mult
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //  if cloud has moved too far to the left
            if (cPos.x <= cloudPosMin.x)
            {
                // move it to the far right - creates appeareance of a loop
                cPos.x = cloudPosMax.x;
            }
            // apply the new position to the cloud
            cloud.transform.position = cPos;

        }
	}
}
