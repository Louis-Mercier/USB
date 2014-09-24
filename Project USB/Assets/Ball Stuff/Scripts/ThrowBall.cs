using UnityEngine;
using System.Collections;


public class ThrowBall : MonoBehaviour {

	public AudioClip throwSound;
	float startingTimeCharged = 0;
	float timeCharged = 0;
	NetworkCharacter networkCharacter;
	public GameObject playerController;

	BallManager ballManager;


	
	// Use this for initialization
	void Start () {
		ballManager = GameObject.FindObjectOfType<BallManager>();
		
		if(ballManager == null){
			Debug.LogError("Couldn't find BallManager");
		}

		networkCharacter = gameObject.GetComponentInParent<NetworkCharacter>();

		if(networkCharacter == null){
			Debug.LogError("Couldn't find NetworkCharacter");
		}



	}
	
	// Update is called once per frame
	void Update () {

		if((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && (startingTimeCharged == 0) && networkCharacter.haveTheBall ){
			startingTimeCharged = Time.time;
			Debug.Log("starting = " + startingTimeCharged);
		}
		
		if (Input.GetButtonUp("Fire1") && networkCharacter.haveTheBall){
			ThrowTheBall(false);
		}
		
		if (Input.GetButtonUp("Fire2") && networkCharacter.haveTheBall){
			ThrowTheBall(true);
		}
		
		/*timeThrow += Time.deltaTime;
		if(timeThrow >= 1 && newBall!= null){
			Physics.IgnoreCollision(transform.root.collider, newBall.collider, false);
		}*/

	}

	void ThrowTheBall(bool lob){
		timeCharged = Time.time - startingTimeCharged;
		audio.PlayOneShot(throwSound);
		ballManager.ThrowTheBall(transform.position, transform.rotation, timeCharged, lob);
		//ballManager.GetComponent<PhotonView>().RPC("ThrowTheBall",PhotonTargets.AllBuffered,transform.position, transform.rotation, timeCharged, lob);
		startingTimeCharged = 0;
		networkCharacter.haveTheBall = false;



	}

	//Throw the fucking ball
	/*void  ThrowTheBall(bool lob) {
		endingTimeCharged = Time.time;
		audio.PlayOneShot(throwSound);
		newBall = (Rigidbody)Instantiate(ballPrefab, transform.position, transform.rotation);
		newBall.name = "ball";
		newBall.tag = "Ball";
		//Debug.Log("timeCharged = " + (endingTimeCharged-startingTimeCharged) + " force = " + (Mathf.Log(endingTimeCharged-startingTimeCharged+5)+1)*throwForce);
		if (lob) {
			newBall.rigidbody.velocity = transform.TransformDirection(Vector3(0,(Mathf.Log(endingTimeCharged-startingTimeCharged+5)+1)*(throwForce*0.6),(Mathf.Log(endingTimeCharged-startingTimeCharged+5)+1)*(throwForce*0.5)));
		} else {
			newBall.rigidbody.velocity = transform.TransformDirection(Vector3(0,(Mathf.Log(endingTimeCharged-startingTimeCharged+5)+1)*(throwForce*0.4),(Mathf.Log(endingTimeCharged-startingTimeCharged+5)+1)*throwForce));
		}
		newBall.transform.RotateAround(newBall.transform.position, newBall.transform.right, 90);
		Physics.IgnoreCollision(transform.root.collider, newBall.collider, true);
		haveTheBall = false;
		timeThrow = 0;	
		startingTimeCharged = 0;
	}*/


}

