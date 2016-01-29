using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

public class LoadStages : MonoBehaviour 
{

	public Text text;

	// Use this for initialization
	void Start () 
	{
		text.text = Application.dataPath;
		StartCoroutine (LoadFromURL());
	}

	IEnumerator LoadFromURL()
	{
		string url;// = "file://"+Application.dataPath + "/oi.txt";

		if (Application.dataPath.StartsWith ("http")) 
		{
			url = Application.dataPath + "/oi.txt";
		}
		else
		{
			url = "file://"+Application.dataPath + "/oi.txt";
		}


		WWW www = new WWW (url);
		
		yield return www;

//		StreamReader sr = new StreamReader(

		//string s = www.bytes;

		Debug.Log (www.error);

		Debug.Log(System.Text.Encoding.Default.GetString(www.bytes));
		if (www.text != null)
		{
			Debug.Log ("Got smthg! " + www.text);
		}

		text.text += "\n" + System.Text.Encoding.Default.GetString (www.bytes) + "\n" + www.error + "\n" + www.text;
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
}
