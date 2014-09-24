using UnityEngine;
using System.Collections;

public class NetworkBall : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
		
	bool gotFirstUpdate = false;
	
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(photonView.isMine){
			//Do nothing, we own the ball
		}
		else{
			transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation,0.1f);
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		
		if(stream.isWriting){
			//This is OUR ball. We need to send our actual position to the network.
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		
		}
		else{
			//This is somone else's ball. We need to receive their position (as of a few
			// millisecond ago, and update our version of that ball.
			
			// Right now, "realPosition" holds the other person's position at the LAST frame.
			// Instead of simply updating "realPosition" and continuing to lerp, 
			// we MAY want to set our transform.position immediately to this old "realPosition"
			// and then update realPosition
			
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();

			
			if(gotFirstUpdate == false){
				transform.position = realPosition;
				transform.rotation = realRotation;
				gotFirstUpdate = true;
			}
		}
	}
}
