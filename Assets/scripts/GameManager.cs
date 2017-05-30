using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;

public class GameData {
	public int repCount;
	public string date;
	public int score;
	public GameData() {
		this.repCount = 0; 
		this.score = 0;
	}

}
	
public class GameManager : MonoBehaviour {
	public Text timer;
	public float timeLeft; 
	private bool gameOver = false;
	private Movement m_movement;
	public GameObject m_ballInstance;
	public Text gameOverRepCountText;
	public Text inGameRepCountText;
	public Text heightScoreText;
	public Canvas GameOverScreen;
	public Button easyButton;
	public Button mediumButton;
	public Button hardButton;
	public Canvas difficultyScreen;
	public static bool gameStarted = false;
	public int userHighScore;
	private int currGameScore;
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser firebUser;


	// Use this for initialization
	void Start () {
		initializeFirebase();
		initializeScreens ();
		initializeMovementScript ();
		initializeDifficultyButtons ();
		setUserHighScoreFromPreviousGames (); 
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.gameStarted) {
			currGameScore = m_movement.planksJumped;
			timeLeft -= UnityEngine.Time.deltaTime;
			UpdateAllGameTextsDisplayed ();
			this.gameOver = isGameOver ();
			if (this.gameOver) {
				disableBallMovement ();
				setGameOverText ();
				enableGameOverScreen ();
				resetGameData ();
				resetGameSettings ();
				sendGameDataToFirebase ();
			}
		}
	}

	void initializeScreens ()
	{
		difficultyScreen.gameObject.SetActive (true);
		GameOverScreen.gameObject.SetActive (false);
	}

	void initializeMovementScript ()
	{
		m_ballInstance = GameObject.Find ("BallSprite");
		m_movement = m_ballInstance.GetComponent<Movement> ();
	}

	void initializeDifficultyButtons ()
	{
		easyButton.onClick.AddListener (easyClick);
		mediumButton.onClick.AddListener (mediumClick);
		hardButton.onClick.AddListener (hardClick);
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

	bool isGameOver ()
	{
		if (timeLeft < 0) {
			timeLeft = 0; // if timeLeft is negative, reset it to 0
			return true;
		} 
		else {
			return false;
		}
	}		

	void enableGameOverScreen ()
	{
		GameOverScreen.gameObject.SetActive (true);
	}

	void disableBallMovement()
	{
		m_movement.enabled = false;
	}

	void setGameOverText ()
	{
		gameOverRepCountText.text = "Rep Count: " + m_movement.repCounter.ToString ();
		gameOverRepCountText.text += "\n Height Score: " + currGameScore.ToString ();
	}
		
	void resetGameSettings()
	{
		this.gameOver = false;
		GameManager.gameStarted = false;
	}

	void resetGameData ()
	{
		m_movement.planksJumped = 0;
		m_movement.plankCount = 0;
	}


	void initializeFirebase() {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://strokesurvivors-605a1.firebaseio.com/");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);

	}

	// Track state changes of the auth object.
	void AuthStateChanged(object sender, System.EventArgs eventArgs) {	
		if (auth.CurrentUser != firebUser) {
			bool signedIn = firebUser != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && firebUser != null) {
				Debug.Log("Signed out plankgame" + firebUser.UserId);
			}
			firebUser = auth.CurrentUser;
			if (signedIn) {
				Debug.Log("Signed in plankgame " + firebUser.UserId);
			}
		}
	}

	void sendGameDataToFirebase ()
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		string gameDataJson = getCurentGameDataAsJson ();
		int newHighScore = getUpdatedHighScore ();
		Debug.LogFormat("sending data for uID: {0}",firebUser.UserId);
		reference.Child ("users").Child (firebUser.UserId).Child ("games").Push().SetRawJsonValueAsync(gameDataJson);
		reference.Child ("users").Child (firebUser.UserId).Child ("highScore").SetValueAsync (newHighScore);;

	}

	string getCurentGameDataAsJson()
	{
		GameData currGameData = new GameData ();
		currGameData.repCount = m_movement.repCounter;
		currGameData.date = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
		currGameData.score = currGameScore;
		string json = JsonUtility.ToJson (currGameData);
		Debug.LogFormat ("gameData: {0}", json);
		return json;
	}
	// returns the higher number between the current game score vs the high score from all past games
	int getUpdatedHighScore()
	{
		if (currGameScore > userHighScore) {
			return currGameScore;
		} 
		else {
			return userHighScore;
		}
	}
	// sets the class variable to the user's current highest score from all games played
	void setUserHighScoreFromPreviousGames()
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		FirebaseDatabase.DefaultInstance
			.GetReference("users").
			GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.Log("ERROR GETTING HIGH SCORE");
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					userHighScore =int.Parse(snapshot.Child(firebUser.UserId).Child("highScore").GetRawJsonValue());
				}
			});

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
