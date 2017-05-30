using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Movement : MonoBehaviour {
	public float m_direction = 1f;
	public float m_speed = 9f;
	public float thresholdVal = .8f; // threshold y value that the phone must go over to jump
	public int jumpPower = 100;
	private bool isGrounded = true; // is the ball grounded
	private bool isOverThreshold = false; // is the phone tilted enough to make the ball jump
	private Rigidbody2D m_rigidBody; 
	private Collision2D m_collision;
	public int repCounter;
	public Transform plank;
	public int plankCount = 0;
	public int planksJumped = 0;
	private int plankDirection = 1;
	private float currentPlankMaxHeight = 0;
	private int heightDifferenceBetweenPlanks = 4;





	// Use this for initialization
	void Start () {
		m_rigidBody = GetComponent<Rigidbody2D> ();
		repCounter = 0;

		// create the first 2 planks 
		createPlank ();
		createPlank ();

	}


	// Update is called once per frame
	private void FixedUpdate () {
		if (GameManager.gameStarted) {
			
			MoveHorizontally ();

			if (Mathf.Abs (Input.acceleration.y) > thresholdVal) {
				isOverThreshold = true;
				jump ();
			}
			// if you are passing a plank, create a new plank
			if (m_rigidBody.position.y > currentPlankMaxHeight - heightDifferenceBetweenPlanks * 2) {
				planksJumped += 1;
				createPlank ();
			}

			// if your arm is lowered back down
			if (isOverThreshold && Mathf.Abs (Input.acceleration.y) < .5f) {

				repCounter += 1;
				isOverThreshold = false;
			}

			// press space to jump if played on unity player
			if (Input.GetKeyDown (KeyCode.Space)) {
				jump ();
			}

		}



	}

	void createPlank() {
		Instantiate(plank, new Vector2(2.29f*plankDirection, currentPlankMaxHeight), Quaternion.identity); 
		plankCount++;
		plankDirection *= -1; //change the direction of where the plank is made (left or right)
		currentPlankMaxHeight += heightDifferenceBetweenPlanks;
	}



	private void jump() {
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0,jumpPower), ForceMode2D.Impulse);
		isGrounded = false;
	}


	private void MoveHorizontally()
	{
		Vector2 movement;
		if (!isGrounded) 
		{
			movement = new Vector2(m_direction,0);
		} 
		else
		{
			movement = new Vector2(m_direction,m_rigidBody.velocity.y);
		}

		movement *= Time.deltaTime*m_speed;
		m_rigidBody.MovePosition (m_rigidBody.position + movement);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "RightWall" || coll.gameObject.tag == "LeftWall")
			m_direction *= -1;
		if (coll.gameObject.tag == "Ground"){
			isGrounded = true;
		}

	}

}