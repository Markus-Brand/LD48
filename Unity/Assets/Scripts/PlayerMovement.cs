using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using EventSystem.Events;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float movementSpeed = 1f; // only default value
	public float rotationSpeed = 1f; // only default value

	void Start()
	{
		EventManager.getInstance().On<FactStateChangedEvent>(e =>
			Debug.Log(Notebook.getNotebookFactText()));
	}

	void Update()
	{
		Vector2 direction = Vector2.zero;
		if (Input.GetKey(KeyCode.W)) {
			direction += Vector2.up;
			Fact.KingsAdvisorBehavesWeird.setState(FactState.Discovered);
		}
		if (Input.GetKey(KeyCode.S)) {
			direction += Vector2.down;
			Fact.KingWantsYourHelpWithFathersDeath.setState(FactState.Discovered);
		}
		if (Input.GetKey(KeyCode.A)) {
			direction += Vector2.left;
			Fact.KingsFatherDiedMysteriously.setState(FactState.Discovered);
		}
		if (Input.GetKey(KeyCode.D)) {
			direction += Vector2.right;
			Fact.KingsFatherWasPoisoned.setState(FactState.Discovered);
		}
		if (direction.sqrMagnitude > 0.5) {
			direction *= movementSpeed * Time.deltaTime;
			gameObject.transform.position += new Vector3(direction.x, direction.y, 0);
		}
		float rotation = 0;
		if (Input.GetKey(KeyCode.Q)) {
			Fact.KingsFatherHadWeirdSymptoms.setState(FactState.Discovered);
			rotation -= 1;
		}
		if (Input.GetKey(KeyCode.E)) {
			rotation += 1;
		}
		if (Math.Abs(rotation) > 0.5) {
			gameObject.transform.Rotate(Vector3.forward, rotation * Time.deltaTime * rotationSpeed);
		}
	}
}
