using UnityEngine;
using System.Collections;

public class MiscUtils : MonoBehaviour 
{
	public static IEnumerator WaitAndLoadLevel(string level, float t)
	{
		yield return new WaitForSeconds(t);

		Application.LoadLevel (level);
	}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
