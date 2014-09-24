using UnityEngine;
using System.Collections;

public class BallEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		rigidbody.angularVelocity = 50 * transform.up + 10 * transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	void DestroyBall(){
		PhotonView pv = GetComponent<PhotonView>();
		
		if(pv.isMine){
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
