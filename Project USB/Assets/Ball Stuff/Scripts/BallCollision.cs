using UnityEngine;
using System.Collections;

public class BallCollision : MonoBehaviour {

	NetworkCharacter networkCharacter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision collision){
		/*Debug.Log("collision enter with : " +  collision.collider.gameObject.name + " tag : " + collision.collider.gameObject.tag);
		if(collision.collider.gameObject.tag == "Player"){
			Debug.Log("blabla");


			//gameObject.GetComponent<PhotonView>().RPC ("DestroyBall",0);
		}*/
	}
}
