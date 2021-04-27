public class SmoothValue
{
	private float _startValue;
	private float _targetValue;
	private readonly SmoothToggle _targetReached;

	public SmoothValue(float initialValue, float fadingTime = SmoothToggle.DefaultFadingTime,
		SmoothToggle.Smoothing smoothingMode = SmoothToggle.DefaultSmoothing)
	{
		_startValue = initialValue;
		_targetValue = initialValue;
		_targetReached = new SmoothToggle(true, fadingTime, smoothingMode);
	}

	public void Update()
	{
		_targetReached.Update();
	}

	public void UpdateUnscaled()
	{
		_targetReached.UpdateUnscaled();
	}

	public float FadingTime {
		get => _targetReached.FadingTime;
		set => _targetReached.FadingTime = value;
	}

	public SmoothToggle.Smoothing SmoothingMode {
		get => _targetReached.SmoothingMode;
		set => _targetReached.SmoothingMode = value;
	}

	public float CurrentValue => _targetReached.Lerp(_startValue, _targetValue);

	public void SetTo(float newTarget, bool immediate = false)
	{
		if (newTarget.Equalish(_targetValue, 0.001f)) return;
		
		if (immediate) {
			_startValue = newTarget;
			_targetValue = newTarget;
			_targetReached.SetTrue(true);
			return;
		}
		
		_startValue = _targetReached.IsTrue() ? _targetValue : CurrentValue;
		_targetValue = newTarget;
		_targetReached.SetFalse(true);
		_targetReached.SetTrue();
	}

	public bool IsStationary => _targetReached.IsTrue();
}
