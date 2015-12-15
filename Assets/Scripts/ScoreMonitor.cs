
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreMonitor : MonoBehaviour 
{
	public UIManager manager;
	public int nShoots;
	public List<ScoreElement> elements;
	
	private Animator anim;
	
	public void UpdateMenu()
	{
		List<RandomEvent> events = manager.events;
		
		print (transform.parent.parent.name);
		RandomEvent e = events[events.Count - 1];
		
		if(events.Count <= nShoots)
		{
			if(e.correct)
			{
				elements[events.Count - 1].img.sprite = manager.rightUISprite[e.resultInt];
			}
			else
			{
				elements[events.Count - 1].img.sprite = manager.wrongUISprite[e.resultInt];
			}
		}
		else
		{
			if(e.correct)
			{
				elements[nShoots].img.sprite = manager.rightUISprite[e.resultInt];
			}
			else
			{
				elements[nShoots].img.sprite = manager.wrongUISprite[e.resultInt];
			}
			anim.SetTrigger("slide");
		}
	}
	
	
	public void Reposition ()
	{
		for(int i = 0; i < nShoots; i++)
		{
			elements[i].img.sprite = elements[i + 1].img.sprite;
		}
	}
	
	public void Reset ()
	{
		for(int i = 0; i < nShoots; i++)
		{
			elements[i].img.sprite = manager.neutralUISprite;
		}
	}
	
	int stillStateHash;
	int currentState;
	void Start()
	{
		anim = GetComponent<Animator>();
		stillStateHash = anim.GetCurrentAnimatorStateInfo(0).shortNameHash;
		currentState = stillStateHash;
	}
	
	void OnEnable () 
	{
		anim = GetComponent<Animator>();
		foreach(ScoreElement e in elements)
		{
			e.img.sprite = manager.neutralUISprite;
		}
	}
	
	void Update () 
	{
		AnimatorStateInfo currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		
		if(currentState != currentBaseState.shortNameHash)
		{
			if (currentBaseState.shortNameHash == stillStateHash)
			{
				Reposition();
			}
		}
		
		currentState = currentBaseState.shortNameHash;
	}
}
