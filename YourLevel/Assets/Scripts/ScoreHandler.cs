using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreHandler : MonoBehaviour {

	public int score = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("GameMaster").GetComponent<ReadLevel>().levelStart) {
			GetComponent<Text>().text = "Score: " + score.ToString();
		} else {
			GetComponent<Text>().text = "";
		}
	}
}
