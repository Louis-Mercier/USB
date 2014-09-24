using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public float gameTime = 0;
	float maxGameTime = 1200f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameTime += Time.deltaTime;

		if(gameTime >= maxGameTime){
			GameObject.FindObjectOfType<Score>().EndGame();
		}
	}

	void OnGUI() {
		int minutes = Mathf.FloorToInt(gameTime / 60F);
		int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
		
		string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

		GUILayout.BeginArea(new Rect(Screen.width/2-20,0,40, 20));
		GUILayout.BeginHorizontal();
		GUILayout.Label(niceTime);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

	}
}
