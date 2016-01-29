using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressionBar : MonoBehaviour 
{
	public Image fill;
	public float totalTime;
	
	private float timeTicker;
	private float initialValue = .1f;
	private float currTarget = .1f;
	private float currBaseValue = .1f;
	private float barFactor;
	private ProbCalculator probCalculator;
	
	public void SetInitialValue (float v)
	{
		initialValue = v / barFactor;
	}
	
	public void SetValue (float v)
	{
		fill.fillAmount = v / barFactor;
	}
	
	public void Grow(float target)
	{
		currTarget = target;
		currBaseValue = fill.fillAmount;
	}
	
	public void RecalculateBarFactor ()
	{
		float max = probCalculator.GetCurrentLimitValue()/100;
		barFactor = max;
	}
	
	void Start () 
	{
		fill.fillAmount = initialValue;	
		probCalculator = ProbCalculator.instance;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!currTarget.Equals(fill.fillAmount))
		{
			fill.fillAmount = Mathf.Lerp(currBaseValue, currTarget, timeTicker/totalTime);
			timeTicker += Time.deltaTime;
		}
		else
		{
			currBaseValue = currTarget;
			timeTicker = 0;	
		}
	}
}
