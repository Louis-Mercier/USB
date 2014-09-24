using UnityEngine;
using System.Collections;

public class NetworkTime : Photon.MonoBehaviour {

	float realTime = 0;
	
	bool gotFirstUpdate = false;

	TimeManager timeManager;
	
	// Use this for initialization
	void Start () {
		timeManager = gameObject.GetComponent<TimeManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(photonView.isMine){
			//Do nothing, we own the time
		}
		else{
			timeManager.gameTime = realTime;
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		
		if(stream.isWriting){
			//This is OUR time. We need to send our actual time to the network.
			stream.SendNext(timeManager.gameTime);			
		}
		else{
			//This is somone else's ball. We need to receive their position (as of a few
			// millisecond ago, and update our version of that ball.
			
			// Right now, "realPosition" holds the other person's position at the LAST frame.
			// Instead of simply updating "realPosition" and continuing to lerp, 
			// we MAY want to set our transform.position immediately to this old "realPosition"
			// and then update realPosition
			
			realTime = (float)stream.ReceiveNext();			
			
			if(gotFirstUpdate == false){
				timeManager.gameTime = realTime;
				gotFirstUpdate = true;
			}
		}
	}
}
