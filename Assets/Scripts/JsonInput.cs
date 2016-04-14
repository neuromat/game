/************************************************************************************/
//  Module written by scaroni <renato.scaroni@gmail.com>
//
//	This Module is a holder class that abstract the json containing the state machine
//	representation of a tree
/************************************************************************************/

using System;

public class JsonInput
{
	public string id;
	public string limitPlays;
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

	public int GetLimitPlays()
	{
		if(limitPlays != null)
		{
			return Convert.ToInt16(limitPlays);
		}

		return 0;
	}
	
	public JsonInput ()
	{
	
	}
}
