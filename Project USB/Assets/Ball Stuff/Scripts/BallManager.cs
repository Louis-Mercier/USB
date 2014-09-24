using UnityEngine;
using System.Collections;

public class BallManager : MonoBehaviour {

	public GameObject ballPrefab;
	public float throwForce = 4f;
	GameObject ballCarrier;
	GameObject[] players;
	GameObject ball;
	float timeThrow = 0f;



	void Update(){
		players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players){
			NetworkCharacter networkCharacter = player.GetComponent<NetworkCharacter>();
			if(networkCharacter != null){
				if(networkCharacter.haveTheBall){
					ballCarrier = player;
				}
			}
			else{
				Debug.LogError("networkCharacter is null");
				return;
			}
		}

		// time for resetting collision with the player that launched the ball
		timeThrow += Time.deltaTime;
		if(timeThrow >= 0.2f && ball!= null){
			Physics.IgnoreCollision(ballCarrier.collider, ball.collider, false);
		}

	}


	public void ThrowTheBall(Vector3 launcherPos, Quaternion launcherRot, float timeCharged, bool lob){
		Debug.Log("ThrowTheBall");
		
		if(ballPrefab != null){
			ball = (GameObject)PhotonNetwork.Instantiate("BallPrefab", launcherPos, launcherRot,0);
			Debug.Log("the ball is instantiated");
			ball.name = "Ball";
			ball.tag = "Ball";


			Physics.IgnoreCollision(ballCarrier.collider, ball.collider, true);
			Debug.Log ("ignore collison ok");

			//Debug.Log("timeCharged = " + (endingTimeCharged-startingTimeCharged) + " force = " + (Mathf.Log(endingTimeCharged-startingTimeCharged+5)+1)*throwForce);
			if (lob) {
				ball.rigidbody.velocity = ball.transform.TransformDirection(new Vector3(0,(float)((Mathf.Log(timeCharged*10+5)+1)*(throwForce*0.6)),(float)((Mathf.Log(timeCharged*10+10)+1)*(throwForce*0.5))));
			} else {
				ball.rigidbody.velocity = ball.transform.TransformDirection(new Vector3(0,(float)((Mathf.Log(timeCharged*10+5)+1)*(throwForce*0.4)),(float)((Mathf.Log(timeCharged*10+10)+1)*throwForce)));
			}
			ball.transform.RotateAround(ball.transform.position, ball.transform.right, 90);

			timeThrow = 0;

		}
		else {
			Debug.LogError("ballPrefab is missing");
		}
	}
}
