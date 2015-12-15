using UnityEngine;
using System.Collections;

public class VersusMode : MonoBehaviour 
{
	public UIManager ui;
	public NewProbCalculator probs;
	
	static private VersusMode _instance;
	static public VersusMode instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.Find("VersusModeManager").GetComponent<VersusMode>();
			}
			
			return _instance;
		}
	}
	
	public void ChooseGKMove()
	{		
		if(!probs.gameObject.activeInHierarchy)
		{
			probs.gameObject.SetActive(true);
			probs.Start();
		}
		int e = probs.GetEvent();
		print ("Resultado "+ e);
		string gkMove = "";
				
		if(e == 0) //esquerda
		{
			gkMove = "esquerda";
		}
		else if (e == 1)
		{
			gkMove = "direita";
		}else
		{
			gkMove = "centro";
		}
		
		ui.BtnActionGetEvent(gkMove);
	}
	
	void Start ()
	{
		ui = GetComponent<UIManager>();
	}
	
	void Update()
	{
	
	}
}
