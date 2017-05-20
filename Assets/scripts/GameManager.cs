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
	public string date;
	public User() {
	}

	public User(string username, string email) {
		this.username = username;
		this.email = email;
		this.repCount = 0; 

	}
}

public class GameManager : MonoBehaviour {
	public Text timer;
	public float timeLeft; // time in seconds left before game over
	private bool isGameOver = false;
	private Movement m_movement;
	public GameObject m_instance;
	// Use this for initialization
	void Start () {
		m_instance = GameObject.Find ("BallSprite");
		m_movement = m_instance.GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= UnityEngine.Time.deltaTime;
		this.timer.text = "" + Mathf.Round(timeLeft);
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
			user.repCount = m_movement.repCounter;
			user.date = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
			string json = JsonUtility.ToJson(user);

			reference.Child("users").Child("2").SetRawJsonValueAsync(json);
			SceneManager.LoadScene (1);
			isGameOver = false;
		}
	}
}
