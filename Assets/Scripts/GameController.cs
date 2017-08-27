using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject player;
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public GameObject playerExplosion;

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText livesText;
	
	private int score;
	private int lives;
	private bool gameOver;
	//private bool restart;
	private bool playerDead;
	void Start () {
		score = 0;
		lives = 3;
		gameOver = false;
		//restart = false;
		playerDead = false;
		restartText.text = "";
		gameOverText.text = "";

		UpdateScores ();
		UpdateLives ();
		StartCoroutine (SpawnWaves ());
	}

	void Update () {
		if (gameOver) {
			if (Input.GetKeyDown (KeyCode.R)) {
				SceneManager.LoadScene(SceneManager.GetActiveScene ().name);
			}
		}
	}

	//to make wait work, it must be made into a coroutine
	IEnumerator SpawnWaves () {
		yield return new WaitForSeconds (startWait);
		while (!gameOver || !playerDead) {
			for (int i = 0; i < hazardCount; i++) {
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			/*if (playerDead) {
				if (lives > 0) {
					Respawn ();
				} else {
					Destroy (player);
					GameOver ();
				}
			}*/

		}
	}

	// Adds the points earned to the score
	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScores ();
	}

	// Update score text with new score
	void UpdateScores () {
		scoreText.text = "Score: " + score;
	}

	void AddLives (int amount) {
		lives += amount;
		UpdateLives ();
	}

	void UpdateLives (){
		livesText.text = "Lives: " + lives;
	}

	// Destroying the player
	public void DestroyPlayer () {
		AddLives (-1);
		player.SetActive (false);
		Instantiate (playerExplosion, player.transform.position, player.transform.rotation);
		playerDead = true;
		
		if (lives < 1) {
			GameOver ();
		} else {
			Respawn ();
		}

		//Destroy (player);
		//GameOver ();
	}

	void Respawn () {
		player.transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
		player.transform.rotation = Quaternion.identity;
		player.SetActive (true);
		playerDead = false;
	}

	// Ending the game
	public void GameOver () {
		gameOverText.text = "Game Over!";
		gameOver = true;
		restartText.text = "Press 'R' for restart.";
		//restart = true;
	}
}
