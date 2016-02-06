using UnityEngine;
using System.Collections;

public class PopitemRotate: MonoBehaviour {

    [SerializeField, Range(1f, 360f)]
    private float RotatePower = 10f;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	   this.transform.RotateAroundLocal( Vector3.up, RotatePower * Time.deltaTime);
	}
}
