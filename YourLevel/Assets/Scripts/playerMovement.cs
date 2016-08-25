using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerMovement : MonoBehaviour {

	Rigidbody2D rig;

	float moveX, moveY;
	bool grounded = false;

	public float movePower;
	public float jumpPower;

	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D>();
		rig.freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
		moveX = Input.GetAxis("Horizontal") * movePower * Time.deltaTime;
		Vector3 moveVector = new Vector3(moveX, 0, 0);

		if (moveX > 0.02f)
			moveX = 0.01f;

		if (moveX < -0.02f)
			moveX = -0.01f;

		moveX = towards(transform.position.x, moveX, transform.position.x + moveX);

		transform.Translate(moveVector);

		if (grounded == true) {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				Vector2 force = new Vector2(0, jumpPower);
				rig.AddForce(force, ForceMode2D.Impulse);
			}
		}
	}

	public float towards(float x, float deltaX, float targetX) {
		if (Mathf.Abs((targetX - x)) - Mathf.Abs(deltaX) < 0) {
			deltaX = targetX - x;
		}
		return deltaX;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.transform.position.y < transform.position.y && coll.gameObject.layer == 8) {
			grounded = true;
		}
	}
}
