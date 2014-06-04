using UnityEngine;
using System.Collections;

public class angular : MonoBehaviour {

	public float rotateSpeed;
	public float targetAngle;
	public ConfigurableJoint thisConfJoint;

	// Use this for initialization
	void Start () {
		rotateSpeed = 20f;
		targetAngle = 0.0f;
		thisConfJoint = GetComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;
	}
	
	// Update is called once per frame
	void Update () {
	//	Input.GetAxis("Horizontal")
		targetAngle +=  Time.deltaTime * rotateSpeed;
		Quaternion targetRotation = Quaternion.AngleAxis( targetAngle, Vector3.right );
		thisConfJoint.targetRotation = targetRotation;
		//Debug.Log(targetAngle);
	
	}
}
