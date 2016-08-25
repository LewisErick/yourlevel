using UnityEngine;
using System.Collections;

public class CameraMov : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("GameMaster").GetComponent<ReadLevel>().levelStart) {
		} else {
			float moveX = Input.GetAxis("Horizontal") * Time.deltaTime;
			float moveY = Input.GetAxis("Vertical") * Time.deltaTime;
			transform.Translate(moveX, moveY, 0f);
		}
	}
}
