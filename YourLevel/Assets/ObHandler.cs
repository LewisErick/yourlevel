using UnityEngine;
using System.Collections;

public class ObHandler : MonoBehaviour {

	public string namae; 
  	public string type;
  	public int movement; 
  	public float speed; 
  	public string effect;
  	public int life;
  	public bool invincible;
  	public bool init = false;

  	public Sprite oldSprite;
  	public RuntimeAnimatorController cont;

  	public int patternCount = 0;

  	bool canHit = true;
  	int hitCount = 0;

  	bool grounded = false;

  	Rigidbody2D rig;

	// Use this for initialization
	void Start () {
		//oldSprite = GetComponent<SpriteRenderer>().sprite
		if (init == false) {
			cont = GetComponent<Animator>().runtimeAnimatorController;
		}
		GetComponent<SpriteRenderer>().sprite = null;
		GetComponent<Animator>().runtimeAnimatorController = null;
		//GetComponent<Animator>().enabled = false;
		rig = GetComponent<Rigidbody2D>();
		rig.freezeRotation = true;
	}

	public void Initialize(string nm, string tp, int mv, float sp, string ef, int lf, bool inv){
		namae = nm;
		type = tp;
		movement = mv;
		speed = sp/50f;
		effect = ef;
		life = lf;
		invincible = inv;
		init = true;
	}

	//
	Vector3 getPlayerInput() {
		float moveX = Input.GetAxis("Horizontal") * speed;
		Vector3 input = new Vector3(moveX, 0,0);
		if (grounded == true) {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				Vector2 force = new Vector2(0, speed*3f);
				rig.AddForce(force, ForceMode2D.Impulse);
			}
		}
		return input;
	}


	Vector3 handleHorPat() {
		Vector3 input = new Vector3(0,0);
		patternCount = patternCount % 200;
		if (patternCount > 100) {
			input.x = speed;
		} else {
			input.x = -speed;
		}
		patternCount += 1;
		return input;
	}

	Vector3 handleVerPat() {
		Vector3 input = new Vector3(0,0);
		patternCount = patternCount % 200;
		if (patternCount > 100) {
			input.y = speed;
		} else {
			input.y = -speed;
		}
		patternCount += 1;
		return input;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("GameMaster").GetComponent<ReadLevel>().levelStart == false) {
			return;
		}
		if (movement != 3 && movement != 4 && init) {
			rig.isKinematic = false;
		}
		if (type == "player") {
			if (canHit == false) {
				hitCount += 1;
				if (hitCount == 200) {
					hitCount = 0;
					canHit = true;
					Physics2D.IgnoreLayerCollision(9, 10, false);
				}
			}
			GameObject.Find("Main Camera").transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.Find("Main Camera").transform.position.z);
		}
		Vector2 moveVector = new Vector2(0,0);
		switch (movement) {
			case 0:
				moveVector = getPlayerInput(); break;
			case 1:
				moveVector = new Vector2(-speed, 0); break;
			case 2:
				moveVector = new Vector2(speed, 0); break;
			case 3:
				moveVector = handleHorPat(); break;
			case 4:
				moveVector = handleVerPat(); break;
		}
		transform.Translate(moveVector*Time.deltaTime);
	}

	void jump(float amount) {
		Vector2 force = new Vector2(0, amount);
		rig.AddForce(force, ForceMode2D.Impulse);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.transform.position.y < transform.position.y && coll.gameObject.layer == 8) {
			grounded = true;
		}
		if (coll.gameObject.layer == 10 && gameObject.layer == 9) {
			if (transform.position.y - (GetComponent<BoxCollider2D>().size.y/2f) > coll.gameObject.transform.position.y) {
				if (coll.gameObject.GetComponent<ObHandler>().invincible == false) {
					coll.gameObject.GetComponent<ObHandler>().life -= 10;
				}
				if (coll.gameObject.GetComponent<ObHandler>().life <= 0) {
					Destroy(coll.gameObject);
				}
				grounded = false;
				Vector2 force = new Vector2(0, speed*3f);
				rig.AddForce(force, ForceMode2D.Impulse);
			} else {
				if (canHit == true) {
					string effect = coll.gameObject.GetComponent<ObHandler>().effect;
					switch (effect) {
						case "life-plus":
							life = life + 10; break;
						case "life-minus":
							if (coll.gameObject.transform.position.y >= transform.position.y) {
								life = life - 10; 
							}
							break;
						case "score":
							GameObject.Find("Score").GetComponent<ScoreHandler>().score += 5; break;
					}
					canHit = false;
					Physics2D.IgnoreLayerCollision(9, 10, true);
				}
			}
		}
		//Coin
		if (coll.gameObject.layer == 12) {
			string effect = coll.gameObject.GetComponent<ObHandler>().effect;
			switch (effect) {
				case "life-plus":
					life = life + 10; break;
				case "life-minus":
					if (coll.gameObject.transform.position.y >= transform.position.y) {
						life = life - 10; 
					}
					break;
				case "score":
					GameObject.Find("Score").GetComponent<ScoreHandler>().score += 5; break;
			}
			Destroy(coll.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (invincible || init == false ){
			return;
		}
		if (type=="player") {
			string effect = coll.gameObject.GetComponent<ObHandler>().effect;
			switch (effect) {
				case "life-plus":
					life = life + 10; break;
				case "life-minus":
					if (coll.gameObject.transform.position.y >= transform.position.y) {
						life = life - 10; 
					}
					break;
				case "score":
					GameObject.Find("Score").GetComponent<ScoreHandler>().score += 5; break;
			}
			return;
		}
		if (coll.gameObject.layer == 8 || coll.gameObject.layer == 11) {

		} else {
			if (coll.gameObject.GetComponent<ObHandler>().type == "player") {
				if (coll.gameObject.transform.position.y < transform.position.y) {
					coll.gameObject.GetComponent<ObHandler>().jump(3f);
					life -= 10;
					if (life <= 0) {
						GameObject.Find("Score").GetComponent<ScoreHandler>().score += 5;
						Destroy(gameObject);
					}
				}
			}
		}
	}
}
