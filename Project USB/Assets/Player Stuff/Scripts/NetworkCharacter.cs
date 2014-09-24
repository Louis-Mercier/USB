using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	float realAimAngle = 0f;
	public bool haveTheBall = false;

	Animator anim;

	bool gotFirstUpdate = false;

	// Use this for initialization
	void Start () {
		CacheComponents();
	}

	void CacheComponents(){
		if(anim == null){
			anim = GetComponent<Animator>();
			if(anim == null){
				Debug.LogError("no animator in the prefab");
			}
		}

		// Cache more components here if require

	}

	// Update is called once per frame
	void Update () {
		if(photonView.isMine){
			//Do nothing, the character motor/input/etc... is moving us
		}
		else{
			transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation,0.1f);
			anim.SetFloat("AimAngle",Mathf.Lerp(anim.GetFloat("AimAngle"),realAimAngle,0.1f));
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){

		if(stream.isWriting){
			//This is OUR player. We need to send our actual position to the network.
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(anim.GetFloat("Speed"));
			stream.SendNext(anim.GetBool("Jumping"));
			stream.SendNext(anim.GetFloat("AimAngle"));
		}
		else{
			//This is somone else's player. We need to receive their position (as of a few
			// millisecond ago, and update our version of that player.

			// Right now, "realPosition" holds the other person's position at the LAST frame.
			// Instead of simply updating "realPosition" and continuing to lerp, 
			// we MAY want to set our transform.position immediately to this old "realPosition"
			// and then update realPosition

			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
			anim.SetFloat("Speed",(float)stream.ReceiveNext());
			anim.SetBool("Jumping",(bool)stream.ReceiveNext());
			realAimAngle = (float)stream.ReceiveNext();

			if(gotFirstUpdate == false){
				transform.position = realPosition;
				transform.rotation = realRotation;
				gotFirstUpdate = true;
			}
		}
	}

	/*[RPC]
	void BallActivation(){
		emptyBall.gameObject.SetActive(true);
		placeholderpistol.gameObject.SetActive(false);
	}

	[RPC]
	void BallDeactivation(){
		emptyBall.gameObject.SetActive(false);
		placeholderpistol.gameObject.SetActive(true);
	}*/
}
