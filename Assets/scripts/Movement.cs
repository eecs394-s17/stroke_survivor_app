using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class User {
	public string username;
	public string email;
	public int repCount;

	public User() {
	}

	public User(string username, string email) {
		this.username = username;
		this.email = email;
		this.repCount = 0;
	}
}


public class Movement : MonoBehaviour {
	public float m_direction = 1f;
	public float m_speed = 9f;
	public int m_jumpPower = 400;
	public Text repCountText;
	public Text timer;
	public float timeLeft = 60.0f;

	private bool isGrounded = true;
	private bool isOverThreshold = false;
	private  float distToGround;
	private Rigidbody2D m_rigidBody;
	private Collision2D m_collision;
	private bool isGameOver = false;


	public int repCounter;

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

		timeLeft -= UnityEngine.Time.deltaTime;
		timer.text = "" + Mathf.Round(timeLeft);
		print (timeLeft.ToString ());

		if (timeLeft < 0) {
			timeLeft = 0;
			isGameOver = true;
		}

		if (isGameOver) {
			// Set up the Editor before calling into the realtime database.
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://strokesurvivors-605a1.firebaseio.com/");

			// Get the root reference location of the database.
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

			User user = new User("chankyuoh", "chankyu@gmail.com");
			user.repCount = this.repCounter;
			string json = JsonUtility.ToJson(user);

			reference.Child("users").Child("1").SetRawJsonValueAsync(json);
			SceneManager.LoadScene (1);
			isGameOver = false;
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

	void updateTimerText() {
		timer.text = "Time Left: " + "10";
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
