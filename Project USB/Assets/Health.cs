using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float hitPoints = 100f;
	float currentHitPoints;

	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	}
	
	[RPC]
	public void TakeDamage (float amt) {
		currentHitPoints -= amt;

		if(currentHitPoints <=0){
			Die();
		}
	}

	/*void OnGUI(){
		if(GetComponent<PhotonView>().isMine && gameObject.tag == "Player"){
			if(GUI.Button(new Rect(Screen.width - 100,0 ,100, 40), "Suicide!")){
				Die();
			}
		}
	}*/

	void Die(){
		if(GetComponent<PhotonView>().instantiationId == 0){
			Destroy(gameObject);
		}
		else {
			if(GetComponent<PhotonView>().isMine){
				if(gameObject.tag == "Player"){
					// a player dies, it gives 1 point for opposite team
					KillScoring();

					if(gameObject.GetComponent<NetworkCharacter>().haveTheBall){
						SpawnBallOnDeath();
					}

					NetworkManager nm = GameObject.FindObjectOfType<NetworkManager>();
					//this is my actual PLAYER object, then initiate respawn process
					nm.standbyCamera.SetActive(true);
					nm.respawnTimer = 3f;
				}
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}

	void KillScoring(){
		TeamMember tm = gameObject.GetComponent<TeamMember>();
		Score score = GameObject.FindObjectOfType<Score>();
		if(tm.teamID == 1){
			score.GetComponent<PhotonView>().RPC ("Scoring", PhotonTargets.AllBuffered, "Kill", 2);
		}
		else if (tm.teamID == 2){
			score.GetComponent<PhotonView>().RPC ("Scoring", PhotonTargets.AllBuffered, "Kill", 1);
		}
	}

	void SpawnBallOnDeath(){
		Transform weaponHolder = transform.Find("soldier/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/WeaponHolder");
		if(weaponHolder==null){
			Debug.LogError("no weapon holder");
			return;
		}
		GameObject ball = (GameObject)PhotonNetwork.Instantiate("BallPrefab", weaponHolder.position, weaponHolder.rotation,0);
		Debug.Log("the ball is instantiated");
		ball.name = "Ball";
		ball.tag = "Ball";
	}
}
