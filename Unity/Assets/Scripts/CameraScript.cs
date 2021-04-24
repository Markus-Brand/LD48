using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraScript : MonoBehaviour
{

	private class Shake
	{
		public readonly float intensity;
		public readonly float duration;
		public float timeLeft;
		public readonly Vector2 direction;

		public Shake(float intensity, float duration, Vector2 direction)
		{
			this.intensity = intensity;
			this.duration = duration;
			this.timeLeft = duration;
			this.direction = direction;
		}
	}
	public Transform followed;

	public float deadZoneSize;

	public float softZoneSize;

	public float speed;

	private Vector2 cameraPosition; //current position, without shake applied

	private List<Shake> currentShakes;

	public float DistanceToTarget;

	void Start()
	{
		currentShakes = new List<Shake>();
		cameraPosition = transform.position;
		//this.MakeUndirectedShake(1, 2);
	}

	void Update()
	{
		Vector3 targetPosition = GetWorldFocusPoint();
		Vector2 direction = targetPosition - transform.position;

		DistanceToTarget = direction.magnitude;
		
		var speedSoftener =
			Math.Max(Math.Min(1, (direction.magnitude - deadZoneSize) / softZoneSize), 0);
		cameraPosition += direction.normalized * (speed * speedSoftener * Time.deltaTime);

		Shake currentShake = null;
		foreach (var shake in currentShakes) {
			shake.timeLeft -= Time.deltaTime;
			if (shake.timeLeft > 0) {
				if (currentShake == null || shake.intensity > currentShake.intensity) {
					currentShake = shake;
				}
			}
		}

		currentShakes.RemoveAll(shake => shake.timeLeft < 0);

		var random = new Vector2(0, 0);
		if (currentShake != null) {
			var relativeTimeShaking = currentShake.timeLeft / currentShake.duration;
			var currentIntensity = relativeTimeShaking * currentShake.intensity;
			var timeFactor = (float) Math.Sqrt(Time.timeScale);
			if (currentShake.direction.sqrMagnitude > 0) {
				//directional
				random = currentShake.direction * ((Random.value * 2 - 1) * currentIntensity * timeFactor);
			} else {
				//un-directional
				random = new Vector2((Random.value * 2 - 1) * currentIntensity * timeFactor,
					(Random.value * 2 - 1) * currentIntensity * timeFactor);
			}
		}


		var actualPosition = cameraPosition + random;
		transform.position = new Vector3(actualPosition.x, actualPosition.y, -10);
	}

	/**
	 * the weighted center of all the important things on the map --> where the camera should look at
	 */
	private Vector2 GetWorldFocusPoint()
	{
		return followed.transform.position;
	}

	public void MakeUndirectedShake(float intensity, float duration)
	{
		MakeDirectedShake(intensity, duration, new Vector2(0, 0));
	}

	public void MakeDirectedShake(float intensity, float duration, Vector2 direction)
	{
		currentShakes.Add(new Shake(intensity, duration, direction));
	}

	public void JumpTo(Vector2 targetPosition)
	{
		cameraPosition = targetPosition;
	}
}
