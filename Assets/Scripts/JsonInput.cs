using System;


public class JsonInput
{
	public string id;
	public string choices;
	public string depth;
	public string limitValue;
	public JsonStateInput [] states;

	public int GetChoices()
	{
		return Convert.ToInt16(choices);
	}
	
	public int GetDepth()
	{
		return Convert.ToInt16(depth);
	}

	public int GetLimitValue()
	{
		return Convert.ToInt16(limitValue);
	}
	
	public JsonInput ()
	{
	
	}
}
