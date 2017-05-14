using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Movement : MonoBehaviour {
	public float m_direction = 1f;
	public float m_speed = 9f;
	public int m_jumpPower = 400;
	public Text repCountText;

	private bool isGrounded = true;
	private bool isOverThreshold = false;
	private  float distToGround;
	private Rigidbody2D m_rigidBody;
	private Collision2D m_collision;


	private int repCounter;

	public Transform plank;
	public Transform fullLengthPlank;

	private int plankCount = 0;
	private int plankDirection = 1;

	private float currentPlankMaxHeight = 0;
	private int heightDifferenceBetweenPlanks = 4;





	// Use this for initialization
	void Start () {
		m_rigidBody = GetComponent<Rigidbody2D> ();
		repCounter = 0;
		setCountText ();
		createPlank ();
//		createPlank ();

	}
		
	
	// Update is called once per frame
	private void FixedUpdate () {
			    
		Move ();

		//print(Input.acceleration.y);
		if (Mathf.Abs(Input.acceleration.y) > .9f && isGrounded)
		{
			
			if (!isOverThreshold) {
				jump ();

			}
			isOverThreshold = true;

				

			//print("JUMPING! x,y,z is: ");
			//print (Input.acceleration.x);
//			print(Input.acceleration.y);
			//print(Input.acceleration.z);

		}

		if (m_rigidBody.position.y > currentPlankMaxHeight-heightDifferenceBetweenPlanks*2) {
			createPlank ();
			fillHole ();
		}

		if (isOverThreshold && Mathf.Abs(Input.acceleration.y) < .5f) {
			print (Input.acceleration.y);

			repCounter += 1;
			isOverThreshold = false;
			setCountText ();
		}


		if (Input.GetKeyDown (KeyCode.Space) && isGrounded)
		{
			//print("JUMPING! x,y,z is: ");
			//print (Input.acceleration.x);
			//print(Input.acceleration.z);

			jump ();
			//Instantiate(Plank, Vector3 (1, 0, 0), Quaternion.identity);
		}

			
	}
	void fillHole() {
		Instantiate (fullLengthPlank, new Vector2 (0, currentPlankMaxHeight - heightDifferenceBetweenPlanks*4), Quaternion.identity);
		plank.name = "filledPlank" + plankCount;
	}




	void createPlank() {
		Instantiate(plank, new Vector2(2.29f*plankDirection, currentPlankMaxHeight), Quaternion.identity); 
		plankCount++;
		plankDirection *= -1;
		currentPlankMaxHeight += heightDifferenceBetweenPlanks;
//		plank.name = "plankNum"+ plankCount;
	}

	void setCountText() {
		repCountText.text = "Rep Count: " + repCounter.ToString ();
	}
		

	private void jump() {
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0,m_jumpPower), ForceMode2D.Impulse);
		isGrounded = false;
	}


	private void Move()
	{
		//Vector2 movement = transform.forward*1* m_speed * Time.deltaTime;
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
		//print (movement);
		m_rigidBody.MovePosition (m_rigidBody.position + movement);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//print (coll);
		if (coll.gameObject.tag == "RightWall" || coll.gameObject.tag == "LeftWall")
			m_direction *= -1;
		if (coll.gameObject.tag == "Ground"){
			//print ("COLLIDED WITH GROUND");
			/*
			if (!isGrounded) {
				repCounter += 1;
				print (repCounter);
			}
			*/

			isGrounded = true;
		}

	}
}
