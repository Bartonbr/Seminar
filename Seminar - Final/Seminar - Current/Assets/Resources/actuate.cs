using UnityEngine;
using System.Collections;

public class actuate : MonoBehaviour
{

		// Use this for initialization

		ConfigurableJoint aJoint;
		bool goingUp;
		float maxEx = 80;
		float minEx = -80;
		float dampers = 200;
		float actuateSpeed;

		void Start ()
		{
				aJoint = (ConfigurableJoint)gameObject.GetComponent ("ConfigurableJoint");
				aJoint.highAngularXLimit = new SoftJointLimit (){ limit = maxEx, bounciness = 0, damper = dampers, spring = 0 };
				aJoint.lowAngularXLimit = new SoftJointLimit (){ limit = minEx, bounciness = 0, damper = dampers, spring = 0 };
		aJoint.angularXDrive = new JointDrive (){mode = JointDriveMode.Position, positionSpring = 1000, positionDamper = 200, maximumForce = 150};
		goingUp = true;s
				actuateSpeed = 0.10f;
		}
	
		// Update is called once per frame
		void Update ()
		{

	
				if (aJoint.targetRotation.x < -1 && !goingUp) {

						goingUp = true;

				} else if (aJoint.targetRotation.x > 1 && goingUp) {
					
						goingUp = false;
				
				}


				if (goingUp) {
		//	Debug.Log ("im trying dont hurt me! up");
	
			aJoint.targetRotation = new Quaternion((aJoint.targetRotation.x + actuateSpeed),0,0,1);
				//	aJoint.targetRotation.Set((aJoint.targetRotation.x + actuateSpeed),0,0,1);
		

				} else if (!goingUp) {
			//Debug.Log ("im trying dont hurt me! down");
			aJoint.targetRotation = new Quaternion((aJoint.targetRotation.x - actuateSpeed),0,0,1);
				//	aJoint.targetRotation.Set((aJoint.targetRotation.x - actuateSpeed),0,0,1);


				}



		}
}
