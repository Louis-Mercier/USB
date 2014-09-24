using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public bool displayScore = false;
	int redTeamScore = 0;
	int greenTeamScore = 0;
	int touchdown = 50;
	int goldGoal = 30;
	int silverGoal = 15;
	int kill = 1;
	int scoreMax = 100;
	public GameObject playercontroller;
	int winnerTeamID = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(redTeamScore >= scoreMax || greenTeamScore >= scoreMax){
			EndGame();
		}
	}

	void OnGUI(){
		if(displayScore){
			GUILayout.BeginArea(new Rect(Screen.width/2-150,0, 300, 20));
			GUILayout.BeginHorizontal();
			GUILayout.Label("Red Team : " + redTeamScore.ToString());
			GUILayout.FlexibleSpace();
			GUILayout.Label("Green Team : " + greenTeamScore.ToString());
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		if(winnerTeamID != 0){
			GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height-50));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();

			if(winnerTeamID == 1){
				GUILayout.Label("Red Team wins the game !");
			}
			else if(winnerTeamID == 2) {
				GUILayout.Label("Green Team wins the game !");
			}
			
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}

	[RPC]
	void Scoring(string scoreType, int teamID){
		int amountToScore = 0;
		if(scoreType == "Touchdown"){
			amountToScore = touchdown;
		}
		else if (scoreType == "SilverGoal"){
			amountToScore = silverGoal;
		}
		else if (scoreType == "GoldGoal") {
			amountToScore = goldGoal;
		}
		else if (scoreType == "Kill"){
			amountToScore = kill;
		}

		if(teamID == 1){
			redTeamScore += amountToScore;
		}
		else if(teamID == 2){
			greenTeamScore += amountToScore;
		}

	}

	public void EndGame(){

		((MonoBehaviour)playercontroller.GetComponent("PlayerMovement")).enabled = false;
		((MonoBehaviour)playercontroller.GetComponent("PlayerShooting")).enabled = false;

		if(redTeamScore > greenTeamScore){
			winnerTeamID = 1;
		}
		else if (redTeamScore < greenTeamScore){
			winnerTeamID = 2;
		}
		else {
			// this is draw end by time
		}
	}
}
