using UnityEngine;

public class AutoResettingSmoothToggle : SmoothToggle
{
	private const float MaxUpTime = 0.1f; //before extracting this to be configurable, check where this class gets used
	private float _timeSinceTrue = 0;

	public AutoResettingSmoothToggle(bool initialValue, float fadingTime = DefaultFadingTime,
		Smoothing smoothingMode = DefaultSmoothing) : base(initialValue, fadingTime, smoothingMode)
	{
	}

	protected override void UpdateState()
	{
		base.UpdateState();
		CheckReset();
	}

	private void CheckReset()
	{
		if (_currentState == State.True) {
			_timeSinceTrue += Time.deltaTime;
		} else {
			_timeSinceTrue = 0;
		}

		if (_timeSinceTrue > MaxUpTime) {
			SetFalse();
		}
	}

	public void ResetResetTimer()
	{
		_timeSinceTrue = 0;
	}
}
