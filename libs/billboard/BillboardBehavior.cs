using UnityEngine;
using System.Collections;

public class BillboardBehavior : MonoBehaviour {
	// Update is called once per frame
	public Vector3 fixedPosition;
	public Vector3 axisPosition;
	public bool positionFixed;
	void Update () {
		Vector3 globalPosition = transform.position;
		if (positionFixed) {
			Quaternion q = transform.parent.rotation;
			Vector3 ps = q * fixedPosition;
			// fixed 
			Vector3 position=Quaternion.Inverse(q) * fixedPosition;
			transform.localPosition = position + axisPosition;
		}
		transform.rotation=Camera.main.transform.rotation;
		transform.rotation*=Quaternion.Euler(180,180,0);//540
		
	}
}
