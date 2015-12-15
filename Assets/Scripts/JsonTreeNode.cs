using System;

public class JsonTreeNode
{
	public string probEvent0;
	public JsonTreeNode right;
	public JsonTreeNode left;
	public string label;
	public string id;
	
	public int GetId()
	{
		return Convert.ToInt16(id);
	}

	public float GetProbEvent0()
	{
		return (float)Convert.ToDouble(probEvent0);
	}
			
	public JsonTreeNode ()
	{
	}
}
