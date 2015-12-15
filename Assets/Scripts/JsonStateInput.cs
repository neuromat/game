using System;

public class JsonStateInput
{
	public string path;
	public string probEvent0;
	public string probEvent1;
	
	public float GetProbEvent0()
	{
		return (float)Convert.ToDouble(probEvent0);
	}

	public float GetProbEvent1()
	{
		return (float)Convert.ToDouble(probEvent1);
	}
}