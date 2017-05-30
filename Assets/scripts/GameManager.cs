using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;

public class User {
	public string uID;
	public int repCount;
	public string date;
	public int score;
	public User() {
	}

	public User(string userID) {
		this.uID = userID;
		this.repCount = 0; 
	}
}
	
public class GameManager : MonoBehaviour {
	public Text timer;
	public float timeLeft; 
	private bool isGameOver = false;
	private Movement m_movement;
	public GameObject m_ballInstance;
	public Text gameOverRepCountText;
	public Text inGameRepCountText;
	public Text heightScoreText;
	public Canvas GameOverScreen;
	private bool dataSent = false;
	public Button easyButton;
	public Button mediumButton;
	public Button hardButton;
	public Canvas difficultyScreen;
	public static bool gameStarted = false;
	public int currHighScore;

	private int currGameScore;
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;





	// Use this for initialization
	void Start () {
		InitializeFirebase();
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://strokesurvivors-605a1.firebaseio.com/");
		difficultyScreen.gameObject.SetActive (true);
		GameOverScreen.gameObject.SetActive (false);
		m_ballInstance = GameObject.Find ("BallSprite");
		m_movement = m_ballInstance.GetComponent<Movement> ();
		easyButton.onClick.AddListener(easyClick);
		mediumButton.onClick.AddListener(mediumClick);
		hardButton.onClick.AddListener(hardClick);
		setCurrentHighScore ();


	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.gameStarted) {
			currGameScore = m_movement.planksJumped;
			timeLeft -= UnityEngine.Time.deltaTime;
			UpdateAllGameTextsDisplayed ();

			if (timeLeft < 0) {
				timeLeft = 0;
				isGameOver = true;
			}


			if (this.isGameOver) {
				enableGameOverScreen (); 
				resetGame ();
			}
			if (!this.dataSent) {
				sendDataToFirebase ();
			}

		}

	}

	void UpdateAllGameTextsDisplayed ()
	{
		setRepCountText ();
		setTimerText ();
		setScoreCountText ();
	}

	void setTimerText ()
	{
		this.timer.text = "Time Left: " + Mathf.Round (timeLeft);
	}

	void setScoreCountText() {
		heightScoreText.text = "Height Score: " +currGameScore.ToString ();
	}

	void setRepCountText() {
		inGameRepCountText.text = "Rep Count: " + m_movement.repCounter.ToString ();
	}
		

	void enableGameOverScreen ()
	{
		gameOverRepCountText.text = "Rep Count: " + m_movement.repCounter.ToString ();
		gameOverRepCountText.text += "\n Height Score: " + currGameScore.ToString ();
		GameOverScreen.gameObject.SetActive (true);
		m_movement.enabled = false;

	}

	void resetGame ()
	{
		isGameOver = false;
		GameManager.gameStarted = false;
		m_movement.planksJumped = 0;
		m_movement.plankCount = 0;
	}

	void sendDataToFirebase ()
	{
		this.dataSent = true;
		print ("entering firebase");
		// Set up the Editor before calling into the realtime database.
//		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://strokesurvivors-605a1.firebaseio.com/");
		// Get the root reference location of the database.
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		User currUser = new User (user.UserId);
		currUser.repCount = m_movement.repCounter;
		currUser.date = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
		currUser.score = currGameScore;

		string json = JsonUtility.ToJson (currUser);
		int newHighScore = currHighScore;
		print ("current highest score: " + currHighScore);
		print ("current game high score: " + currGameScore);
		if (currGameScore > newHighScore) {
			newHighScore = currGameScore;
		}
		print ("new high score: " + newHighScore);
			

		Debug.LogFormat("curr user id: {0}",currUser.uID);
		reference.Child ("users").Child (currUser.uID).Child ("games").Push().SetRawJsonValueAsync(json);
		//reference.Child("users").Child(currUser.uID).Child("games").SetRawJsonValueAsync(json);
		reference.Child ("users").Child (currUser.uID).Child ("highScore").SetValueAsync (newHighScore);;
		//reference.Child ("users").Push ().SetRawJsonValueAsync (json);
	}
	// sets the class variable to the user's current highest score from all games played
	void setCurrentHighScore()
	{
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://strokesurvivors-605a1.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		FirebaseDatabase.DefaultInstance
			.GetReference("users").
			GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					// Handle the error...
					print("ERROR GETTING HIGH SCORE");
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					//print(snapshot.Child("YLQHehbNvUUwyzlYo4Hy6tjtQIx2").Child("highScore").GetValue().ToString());
					//highScore = ((int)snapshot.Child("YLQHehbNvUUwyzlYo4Hy6tjtQIx2").Child("games").Child("highScore").GetValue(true));
					currHighScore =int.Parse(snapshot.Child(user.UserId).Child("highScore").GetRawJsonValue());
				}
			});

	}

	void InitializeFirebase() {
		Debug.Log("Setting up Firebase Auth plankgame");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);

	}

	// Track state changes of the auth object.
	void AuthStateChanged(object sender, System.EventArgs eventArgs) {	
		if (auth.CurrentUser != user) {
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null) {
				Debug.Log("Signed out plankgame" + user.UserId);
			}
			user = auth.CurrentUser;
			if (signedIn) {
				Debug.Log("Signed in plankgame " + user.UserId);
			}
			print ("done");

		}
	}

	void easyClick()
	{
		this.timeLeft = 60f;
		m_movement.m_speed = 5f;
		difficultyScreen.gameObject.SetActive (false);
		GameManager.gameStarted = true;
	}

	void mediumClick()
	{
		this.timeLeft = 120f;
		m_movement.m_speed = 7f;
		difficultyScreen.gameObject.SetActive (false);
		GameManager.gameStarted = true;
	}

	void hardClick()
	{
		this.timeLeft = 5f;
		m_movement.m_speed = 9f;
		difficultyScreen.gameObject.SetActive (false);
		GameManager.gameStarted = true;
	}
}
