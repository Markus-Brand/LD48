using System;
using UnityEngine;

public class SmoothToggle
{
	public const float DefaultFadingTime = 0.8f;
	public float FadingTime = DefaultFadingTime;
	public const Smoothing DefaultSmoothing = Smoothing.SmoothStep;
	public Smoothing SmoothingMode = DefaultSmoothing;

	public Action<bool> FinishHandler = null;

	protected State _currentState = State.False;
	private float _timeInState;

	protected enum State
	{
		False, FadingTrue, True, FadingFalse
	}

	public enum Smoothing
	{
		None, SmoothStep, ToFalse, ToTrue, ToEnds
	}

	public SmoothToggle(bool initialValue, float fadingTime = DefaultFadingTime, Smoothing smoothingMode = DefaultSmoothing)
	{
		SetTo(initialValue, true);
		FadingTime = fadingTime;
		SmoothingMode = smoothingMode;
	}

	public virtual void Update()
	{
		Update(Time.deltaTime);
	}

	public virtual void UpdateUnscaled()
	{
		Update(Time.unscaledDeltaTime);
	}
	
	public void Update(float delta)
	{
		_timeInState += delta;
		UpdateState();
	}

	private float CurrentRawValue
	{
		get {
			switch (_currentState) {
				case State.FadingTrue:
					return _timeInState / FadingTime;
				case State.True:
					return 1;
				case State.FadingFalse:
					return 1f - (_timeInState / FadingTime);
				case State.False:
				default:
					return 0;
			}
		}
	}

	public float CurrentValue {
		get {
			float raw = CurrentRawValue;
			switch (SmoothingMode) {
				case Smoothing.SmoothStep:
					return Mathf.SmoothStep(0, 1, raw);
				case Smoothing.ToFalse:
					return raw * raw * raw;
				case Smoothing.ToTrue:
					return Util.SmoothToOne(raw);
				case Smoothing.ToEnds:
					if (raw > 0.5f) {
						float normal = raw * 2f - 1f;
						float smoothed = normal * normal * normal;
						return smoothed * 0.5f + 0.5f;
					} else {
						float normal = raw * 2f;
						float smoothed = Util.SmoothToOne(normal);
						return smoothed * 0.5f;
					}
				case Smoothing.None:
				default:
					return raw;
			}
		}
	}

	/// <summary>
	/// lerp between two values, based on the current value of this object
	/// </summary>
	/// <param name="falseValue">returned when currently false</param>
	/// <param name="trueValue">returned when currently true</param>
	/// <returns>a mix of the two given values, based on the current state of this object</returns>
	public float Lerp(float falseValue, float trueValue)
	{
		float value = CurrentValue;
		return falseValue + value * (trueValue - falseValue);
	}

	/// <see cref="Lerp(float,float)"/>
	public Color Lerp(Color falseValue, Color trueValue)
	{
		float value = CurrentValue;
		return Color.Lerp(falseValue, trueValue, value);
	}

	/// <see cref="Lerp(float,float)"/>
	public Vector3 Lerp(Vector3 falseValue, Vector3 trueValue)
	{
		float value = CurrentValue;
		return Vector3.Lerp(falseValue, trueValue, value);
	}

	public void SetTo(bool value, bool immediate = false)
	{
		if (value) {
			SetTrue(immediate);
		} else {
			SetFalse(immediate);
		}
	}
	
	public void SetTrue(bool immediate = false)
	{
		if (immediate) {
			_currentState = State.True;
			_timeInState = 0;
			return;
		}
		switch (_currentState) {
			case State.True:
			case State.FadingTrue:
				return; //abort setting to true!
			case State.FadingFalse:
				_timeInState = FadingTime - _timeInState;
				break;
			case State.False:
				_timeInState = 0;
				break;
		}

		_currentState = State.FadingTrue;
	}

	public void SetFalse(bool immediate = false)
	{
		if (immediate) {
			_currentState = State.False;
			_timeInState = 0;
			FinishHandler?.Invoke(true);
			return;
		}
		switch (_currentState) {
			case State.False:
			case State.FadingFalse:
				return; //abort setting to false!
			case State.FadingTrue:
				_timeInState = FadingTime - _timeInState;
				break;
			case State.True:
				_timeInState = 0;
				break;
		}

		_currentState = State.FadingFalse;
	}

	public void Flip()
	{
		SetTo(IsOrBecomesFalse());
	}

	public void FinishImmediately()
	{
		if (_currentState == State.FadingTrue) {
			SetTrue(true);
		} else if (_currentState == State.FadingFalse) {
			SetFalse(true);
		}
	}

	public bool IsTrue()
	{
		return _currentState == State.True;
	}

	public bool IsFalse()
	{
		return _currentState == State.False;
	}

	public bool IsOrBecomesTrue()
	{
		return _currentState == State.True || _currentState == State.FadingTrue;
	}

	public bool IsOrBecomesFalse()
	{
		return !IsOrBecomesTrue();
	}

	public bool IsTrueBy(float amount)
	{
		if (IsTrue()) return true;
		if (IsFalse()) return false;
		return CurrentValue > amount;
	}

	public bool IsFalseBy(float amount)
	{
		return !IsTrueBy(1 - amount);
	}

	public bool IsAnimating()
	{
		return _currentState == State.FadingFalse || _currentState == State.FadingTrue;
	}

	protected virtual void UpdateState()
	{
		if (_timeInState > FadingTime) {
			if (_currentState == State.FadingTrue) {
				_timeInState = 0;
				_currentState = State.True;
				FinishHandler?.Invoke(true);
			} else if (_currentState == State.FadingFalse) {
				_timeInState = 0;
				_currentState = State.False;
				FinishHandler?.Invoke(false);
			}
		}
	}
}
