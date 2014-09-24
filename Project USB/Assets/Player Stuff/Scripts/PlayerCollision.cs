using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

	NetworkCharacter networkCharacter;
	Transform emptyBall;
	Transform placeholderpistol;
	Score score;
	TeamMember teamMember;

	// Use this for initialization
	void Start () {
		teamMember = GetComponent<TeamMember>();
		score = GameObject.FindObjectOfType<Score>();
		networkCharacter = gameObject.GetComponent<NetworkCharacter>();
		emptyBall = transform.Find("soldier/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/WeaponHolder/EmptyBall");
		placeholderpistol = transform.Find ("soldier/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/WeaponHolder/placeholderpistol");
	}
	
	// Update is called once per frame
	void Update () {
		/*if(networkCharacter.haveTheBall){
			gameObject.GetComponent<PhotonView>().RPC("BallActivation", PhotonTargets.AllBuffered);
		}
		else{
			gameObject.GetComponent<PhotonView>().RPC("BallDeactivation", PhotonTargets.AllBuffered);
		}*/
		TouchdownDetection();
	}

	void OnControllerColliderHit(ControllerColliderHit hit){
		Debug.Log("OnControllerColliderHit");

		if(hit.collider.gameObject.tag == "Ball"){
			Debug.Log(gameObject.name + "hit by Ball");
			networkCharacter.haveTheBall = true;

			gameObject.GetComponent<PhotonView>().RPC("BallActivation", PhotonTargets.AllBuffered);

			hit.gameObject.GetComponent<PhotonView>().RPC ("DestroyBall",0);
		}
	}

	/*void OnCollisionEnter(Collision collision){
		Debug.Log("OnCollisionEnter");
		
		if(collision.collider.gameObject.tag == "Ball"){
			Debug.Log(gameObject.name + "hit by Ball");
			networkCharacter.haveTheBall = true;
			
			gameObject.GetComponent<PhotonView>().RPC("BallActivation", PhotonTargets.AllBuffered);
			
			collision.gameObject.GetComponent<PhotonView>().RPC ("DestroyBall",0);
		}
	}*/

	[RPC]
	void BallActivation(){
		emptyBall.gameObject.SetActive(true);
		placeholderpistol.gameObject.SetActive(false);
	}

	[RPC]
	void BallDeactivation(){
		if(emptyBall != null){
			emptyBall.gameObject.SetActive(false);
		}
		if(placeholderpistol != null){
			placeholderpistol.gameObject.SetActive(true);
		}
	}

	void TouchdownDetection(){
		// raycast to know if we are in the end zone if we have the ball
		if(networkCharacter.haveTheBall){
			Ray ray = new Ray(transform.position, -transform.up);
			RaycastHit[] hits = Physics.RaycastAll(ray);
			foreach (RaycastHit hit in hits){
				if(hit.collider.gameObject.name == "End_Zone_1" && teamMember.teamID == 2){
					score.GetComponent<PhotonView>().RPC("Scoring", PhotonTargets.AllBuffered, "Touchdown", teamMember.teamID);
					networkCharacter.haveTheBall = false;
				}
				else if(hit.collider.gameObject.name == "End_Zone_2" && teamMember.teamID == 1){
					score.GetComponent<PhotonView>().RPC("Scoring", PhotonTargets.AllBuffered, "Touchdown", teamMember.teamID);
					networkCharacter.haveTheBall = false;
				}
			}
		}
	}

}
