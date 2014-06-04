using UnityEngine;
using System.Collections;

public class Extension : MonoBehaviour {
	public float extensionSpeed = 1.0f;
	public double maximumDistance = 5.0;
	public bool extend = true;
	public bool activated = false;
	public double minimumDistance = 1.05;
    public Transform thisObjectParent ;
	public double deadZone = 0.05;
	float extensionDistance;
	double neutralState = 0;
	// Use this for initialization
	void Start () {
		thisObjectParent = this.transform.parent;
		neutralState = (maximumDistance + minimumDistance)/2;
	
	}
	
	// Update is called once per frame
	void Update () {
		float changer = Time.deltaTime * extensionSpeed;
		//this is how far this object is from its parent
		extensionDistance = Vector3.Distance(thisObjectParent.transform.position, this.transform.position);
		//extensionDistance = Vector3.Distance(this.transform.parent.transform.position, this.transform.position);
		//if activated and extension is on then extend to maximum range from parent object
		
		if(extend && activated && (extensionDistance <= maximumDistance - deadZone)){
	
//			ConfigurableJoint joint = this.gameObject.GetComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;
	//   	joint.yMotion = ConfigurableJointMotion.Free;
			transform.Translate(Vector3.up * changer);

			//transform.localScale += new Vector3(0, changer * 2.0F, 0);
			//else if extend is not on and the module is activated then contract
			//until minimum distance from parent is reached
			}else if(!extend && activated && (extensionDistance > minimumDistance + deadZone)){
				
				Debug.Log(extensionDistance + " > " + minimumDistance);
			//	transform.localScale -= new Vector3(0,changer * 2, 0);
				transform.Translate(Vector3.down * changer);	
			
			}
		
		// if the module is not activated then it should move to its neutral position
		// half way between its max and minimum extension length
		if(!activated){
			
			if(extensionDistance < neutralState - deadZone){
				transform.Translate(Vector3.up * Time.deltaTime * extensionSpeed);
			
				}else if(extensionDistance > neutralState + deadZone){
						transform.Translate(Vector3.down * Time.deltaTime * extensionSpeed);
			
					}
		}
	}
	
}
