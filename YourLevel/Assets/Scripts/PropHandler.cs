using UnityEngine;
using System.Collections;

public class PropHandler : MonoBehaviour {

	public Sprite oldSprite;

	// Use this for initialization
	void Start () {
		GameObject.Find("GameMaster").GetComponent<ReadLevel>().props.Add(gameObject);
		//Debug.Log(iCont);
		GameObject.Find("GameMaster").GetComponent<ReadLevel>().iCount += 1;
		oldSprite = GetComponent<SpriteRenderer>().sprite;
		GetComponent<SpriteRenderer>().sprite = null;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
