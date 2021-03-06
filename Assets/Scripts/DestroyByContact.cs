﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public int scoreValue;
	private GameController gameController;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script.");
		}
	}

	void OnTriggerEnter (Collider other) {
		//compareTag(string) is v. v. v. slightly more performance efficient than ==
		if (other.CompareTag ("Boundary") || other.CompareTag ("Enemy")) {
			return;
		}
		if (explosion != null) {
			Instantiate (explosion, transform.position, transform.rotation);
		}
		if (other.CompareTag ("Player")) {
			gameController.DestroyPlayer ();
		}

		gameController.AddScore (scoreValue);
		Destroy (gameObject);
	}
}
