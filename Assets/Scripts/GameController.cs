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
	public float respawnDelay;
	public float alternatingWait;
	public GameObject playerExplosion;

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText livesText;
	
	private int score;
	private int lives;
	private bool gameOver;
	private bool playerDead;
	private bool invincible;
	private Renderer playerRenderer;

	void Start () {
		playerRenderer = player.GetComponent<Renderer> ();

		score = 0;
		lives = 3;
		gameOver = false;
		playerDead = false;
		invincible = false;
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
		}
	}

	// Adds the points earned to the score
	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScores ();

		if ((score % 500) == 0) {
			AddLives (1);
			UpdateLives ();
		}
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
		if (!invincible) {
			AddLives (-1);
			player.SetActive (false);
			Instantiate (playerExplosion, player.transform.position, player.transform.rotation);
			playerDead = true;
		
			if (lives < 1) {
				Destroy (player);
				GameOver ();
			} else {
				StartCoroutine (Respawn ());
			}
		}
	}

	IEnumerator Respawn () {
		invincible = true;
		yield return new WaitForSeconds (respawnDelay);

		player.transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
		player.transform.rotation = Quaternion.identity;
		player.SetActive (true);
		playerDead = false;

		bool alternate = true;
		for (int i = 0; i < 25; i++) {
			playerRenderer.enabled = alternate;
			alternate = !alternate;
			yield return new WaitForSeconds (alternatingWait);
		}
		player.SetActive (true);
		invincible = false;
	}

	// Ending the game
	public void GameOver () {
		gameOverText.text = "Game Over!";
		gameOver = true;
		restartText.text = "Press 'R' for restart.";
	}
}
