using UnityEngine;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	public GameObject standbyCamera;
	SpawnSpot[] spawnSpots;
	SpawnSpot[] spawnSpotsRedTeam;
	SpawnSpot[] spawnSpotsGreenTeam;
	Score score;
	string[] redTeamList = new string[8];
	string[] greenTeamList = new string[8];
	bool[] redTeamToogleList = new bool[8];
	bool[] greenTeamToogleList = new bool[8];


	public bool offlineMode = false;

	bool connecting = false;

	List<string> chatMessages;
	int maxChatMessages = 5;

	public float respawnTimer = 0;

	bool hasPickedTeam = false;
	int teamID = 0;

	// Use this for initialization
	void Start () {
		for(int i=0 ; i < redTeamList.Length ; i++){
			redTeamList[i]="";
		}

		for(int i=0 ; i < greenTeamList.Length ; i++){
			greenTeamList[i]="";
		}

		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		List<SpawnSpot> listRedSpawnSpots = new List<SpawnSpot>();
		List<SpawnSpot> listGreenSpawnSpots = new List<SpawnSpot>();
		foreach (SpawnSpot spawnSpot in spawnSpots){
			if(spawnSpot.GetComponent<SpawnSpot>().teamId == 1){
				listRedSpawnSpots.Add(spawnSpot);
			}
			else if(spawnSpot.GetComponent<SpawnSpot>().teamId == 2){
				listGreenSpawnSpots.Add(spawnSpot);
			}
		}
		spawnSpotsRedTeam = listRedSpawnSpots.ToArray();
		spawnSpotsGreenTeam = listGreenSpawnSpots.ToArray();

		PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "Awesome Dude");
		chatMessages = new List<string>();

		score = GameObject.FindObjectOfType<Score>();
	}

	void OnDestroy(){
		PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
	}

	public void AddChatMessage(string m){
		GetComponent<PhotonView>().RPC ("AddChatMessage_RPC", PhotonTargets.AllBuffered, m);
	}

	[RPC]
	void AddChatMessage_RPC(string m){
		while(chatMessages.Count >= maxChatMessages){
			chatMessages.RemoveAt(0);
		}
		chatMessages.Add(m);
	}

	void Connect(){
		PhotonNetwork.ConnectUsingSettings("ProjectUSB v0001");
	}

	void OnGUI(){
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

		if(PhotonNetwork.connected == false && connecting == false){
			// We are not yet connected, so ask the player for offline or online mode
			GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Username: ");
			PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
			GUILayout.EndHorizontal();

			if(GUILayout.Button("Single Player")){
				connecting = true;
				PhotonNetwork.offlineMode = true;
				OnJoinedLobby();
			}
			if(GUILayout.Button("Multiplayer Player")){
				connecting = true;
				Connect();
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		if(PhotonNetwork.connected == true && connecting == false){
			if(hasPickedTeam){
				// We are fully connected, make sure to display the chat box
				/*GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height));
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();

				foreach(string msg in chatMessages){
					GUILayout.Label(msg);
				}

				GUILayout.EndVertical();
				GUILayout.EndArea();*/

				//dislay the score when the game is launched
				GetComponent<Score>().displayScore = true;
				// hide the cursor
				Screen.showCursor = false;

			}
			else {
				GUILayout.BeginArea(new Rect(0,Screen.height/4, Screen.width, Screen.height));
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
								
				if(GUILayout.Button("Red Team")){
					Debug.Log("red button");
					GetComponent<PhotonView>().RPC("AddPlayerToTeam", PhotonTargets.AllBuffered,PhotonNetwork.player.name,this.teamID,1);
					this.teamID = 1;
					SpawnMyPlayer(this.teamID);
				}

				if(GUILayout.Button("Green Team")){
					Debug.Log("green button");
					GetComponent<PhotonView>().RPC("AddPlayerToTeam", PhotonTargets.AllBuffered,PhotonNetwork.player.name,this.teamID,2);
					this.teamID = 2;
					SpawnMyPlayer(this.teamID);
				}

				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();

				GUILayout.BeginArea(new Rect(0,Screen.height/3, Screen.width, Screen.height));
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();

				for(int i = 0 ; i < redTeamList.Length ; i++){
					GUILayout.BeginHorizontal();
					GUILayout.Label(redTeamList[i]);
					if(redTeamList[i] != ""){
						if(redTeamToogleList[i] || redTeamList[i]!= PhotonNetwork.player.name){
							GUI.enabled = false;
							if(redTeamToogleList[i]){
								GetComponent<PhotonView>().RPC ("LockPlayerInTeam", PhotonTargets.AllBuffered, PhotonNetwork.player.name, 1);
							}
						}
						redTeamToogleList[i] = GUILayout.Toggle(redTeamToogleList[i],"");
						GUI.enabled = true;
					}
					GUILayout.EndHorizontal();
				}				                                          

				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
				
				for(int i = 0 ; i < greenTeamList.Length ; i++){
					GUILayout.BeginHorizontal();
					GUILayout.Label(greenTeamList[i]);
					if(greenTeamList[i] != ""){
						if(greenTeamToogleList[i] || greenTeamList[i]!= PhotonNetwork.player.name){
							GUI.enabled = false;
							if(greenTeamToogleList[i]){
								GetComponent<PhotonView>().RPC ("LockPlayerInTeam", PhotonTargets.AllBuffered, PhotonNetwork.player.name, 2);
							}
						}
						greenTeamToogleList[i] = GUILayout.Toggle(greenTeamToogleList[i],"");
						GUI.enabled = true;
					}
					GUILayout.EndHorizontal();
				}				
				
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();

			}
		}
	}

	void OnJoinedLobby(){
		Debug.Log("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed(){
		Debug.Log("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom(null);
	}

	void OnJoinedRoom(){
		Debug.Log("OnJoinedRoom");
		connecting = false;
		//SpawnMyPlayer();
	}

	void SpawnMyPlayer(int teamID){
		hasPickedTeam = true;
		//AddChatMessage("Spawning player " + PhotonNetwork.player.name);

		if(spawnSpots == null){
			Debug.Log("WTF ?!!?!!");
			return;
		}

		SpawnSpot mySpawnSpot;


		if(teamID == 1){
			mySpawnSpot = spawnSpotsRedTeam[Random.Range(0,spawnSpotsRedTeam.Length)];
		}
		else if(teamID == 2){
			mySpawnSpot = spawnSpotsGreenTeam[Random.Range(0,spawnSpotsGreenTeam.Length)];
		}
		else{
			mySpawnSpot = spawnSpots[Random.Range(0,spawnSpots.Length)];
		}

		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate("PlayerController",mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		score.playercontroller = myPlayerGO;
		standbyCamera.SetActive(false);

		//((MonoBehaviour)myPlayerGO.GetComponent("FPSInputController")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("MouseLook")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("PlayerMovement")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("PlayerShooting")).enabled = true;

		myPlayerGO.GetComponent<PhotonView>().RPC ("SetTeamID", PhotonTargets.AllBuffered, teamID);


		myPlayerGO.transform.FindChild("Main Camera").gameObject.SetActive(true);

		if(GameObject.FindGameObjectsWithTag("GameTime").Length == 0){
			GameObject gameTime = (GameObject)PhotonNetwork.Instantiate("GameTime", Vector3.zero, Quaternion.identity, 0);
		}
	}

	void Update(){
		if(respawnTimer > 0){
			respawnTimer -= Time.deltaTime;

			if(respawnTimer <= 0){
				//Time to respawn the player
				SpawnMyPlayer(this.teamID);
			}
		}
	}

	[RPC]
	void AddPlayerToTeam(string playerName, int actualTeamID, int futureTeamID){
		Debug.Log("adding player : " + playerName + " from team : " + actualTeamID + " to team : " + futureTeamID);
		if(futureTeamID != actualTeamID){
			if(futureTeamID == 1){
				for(int i=0 ; i< redTeamList.Length ; i++){
					if(redTeamList[i] == ""){
						redTeamList[i] = playerName;
						Debug.Log(playerName + " add in team 1");
						if(actualTeamID == 2){
							for(int j = 0 ; j<greenTeamList.Length ; j++){
								if(greenTeamList[j] == playerName){
									greenTeamList[j] = "";
									Debug.Log(playerName + " suppress from team : 2");
									break;
								} 
							}
						}
						break;
					}
				}
			}
			else if(futureTeamID == 2){
				for(int i=0 ; i< greenTeamList.Length ; i++){
					if(greenTeamList[i] == ""){
						greenTeamList[i] = playerName;
						Debug.Log(playerName + " add in team 2");
						if(actualTeamID == 1){
							for(int j = 0 ; j<redTeamList.Length ; j++){
								if(redTeamList[j] == playerName){
									redTeamList[j] = "";
									Debug.Log(playerName + " suppress from team : 1");
									break;
								}
							}
						}
						break;
					}
				}
			}
		}
	}

	[RPC]
	void LockPlayerInTeam(string playerName, int team){
		if (team == 1) {
			for (int i = 0 ; i < redTeamList.Length; i++){
				if(redTeamList[i] == playerName){
					redTeamToogleList[i] = true;
				}
			}
		}else if (team == 2) {
			for (int i = 0 ; i < greenTeamList.Length; i++){
				if(greenTeamList[i] == playerName){
					greenTeamToogleList[i] = true;
				}
			}
		}

	}

}
