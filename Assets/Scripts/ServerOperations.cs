using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JsonFx.Json;

public class ServerOperations
{
	public static readonly string gameServerLocation = "neurogol.numec.prp.usp.br:8090/";
	public static readonly string webProtocol = "http://";
	
	public delegate void LogRegistered (bool success,string response, List<RandomEvent> log, MonoBehaviour mb, string playId);
	
	static private ServerOperations _instance;
	static public ServerOperations instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new ServerOperations();
			}
			
			return _instance;
		}
	}
	
	public void Callback (bool success,string response, List<RandomEvent> log, MonoBehaviour mb, string playId)
	{
		if(success)
		{
			mb.StartCoroutine(RegisterLog(log, playId));
		}
	}
	
	IEnumerator RegisterLog (List<RandomEvent> log, string playId)
	{
		foreach(RandomEvent l in log)
		{
			WWW www = null;
			WWWForm form = new WWWForm ();
			form.AddField ("resultInt", l.resultInt.ToString());
			form.AddField ("optionChosenInt", l.optionChosenInt.ToString());
			form.AddField ("correct", l.correct.ToString());
			form.AddField ("playID", playId);
			form.AddField ("time", l.time.ToString());

			string loginURL = webProtocol + gameServerLocation + "/insertLog";
			www = new WWW (loginURL, form);
			
			yield return www;
		}		
	}
	
	public void RegisterPlay (MonoBehaviour mb, string stageID, int totalCorrect, float successRate, List<RandomEvent> log)
	{
		
		WWW www = null;
		WWWForm form = new WWWForm ();
		form.AddField ("playerAlias", PlayerInfo.alias);
		form.AddField ("gender", PlayerInfo.gender);
		form.AddField ("age", PlayerInfo.age);
		form.AddField ("education", PlayerInfo.education);
		form.AddField ("destro", PlayerInfo.destro);
		form.AddField ("totalPlays", log.Count.ToString());
		form.AddField ("totalCorrect", totalCorrect.ToString());
		form.AddField ("successRate", successRate.ToString());
		var playId = PlayerInfo.alias+","+DateTime.Now.ToShortDateString().Replace("/", "")+","+DateTime.Now.Hour+DateTime.Now.Minute+DateTime.Now.Second;
		form.AddField ("playID", playId);
		form.AddField ("stageID", stageID);
		
		string loginURL = webProtocol + gameServerLocation + "/insertPlay";
		www = new WWW (loginURL, form);
		mb.StartCoroutine (WaitRegister (www, Callback, log, mb, playId));
	}
	
	IEnumerator WaitRegister (WWW www, LogRegistered callback, List<RandomEvent> log, MonoBehaviour mb, string playId)
	{
		yield return www;
		
		// check for error first
		if (www.error != null && !String.IsNullOrEmpty (www.error))
		{
			if (callback != null)
			{
				Debug.Log ("WWW Error text - Wait Login: " + www.text + "; error message: " + www.error + "; url: " + www.url);
				callback (false, www.text, log, mb, playId);
			}
		}
		
		if (www.text != null && !String.IsNullOrEmpty (www.text))
		{
			Debug.Log ("Got smthg! " + www.text);
			
			//I've received a response from the server
			if (callback != null)
			{
				callback (true, www.text, log, mb, playId);
			}
		}
		else
		{
			//Something went wrong
			if (callback != null)
			{
				Debug.Log ("Error text: " + www.text + " error message " + www.error);
				callback (false, www.error, log, mb, playId);
			}
		}
	}
	

}
