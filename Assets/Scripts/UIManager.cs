using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RandomEvent 
{
	public int resultInt;
	public int optionChosenInt;
	public string result;
	public string optionChosen;
	public bool correct;
	public string state;
	public float time;
}

public class UIManager : MonoBehaviour 
{
	public Text eventsLog;
	public GKAnimController[] gkAnim;
	public Animator ball;
	public int eventWindow = 10;
	public Text successRateText;
	public ProgressionBar progressionBar;
	public float successRate = 0;
	public ScoreMonitor scoreMonitor;
	public GameObject btnsAndQuestion;
	public GameObject timeCount;
	public Animator goal;
	public Animator pegoal;

	public AudioSource cheer;
	public AudioSource lament;
			
	public Sprite neutralUISprite;
	public Sprite [] rightUISprite;
	public Sprite [] wrongUISprite;
	
	public List<GameObject> optBtns;

	private float stopwatch;
	private ProbCalculator probs;
	private int eventCount = 0;
	private int success = 0;
	private List<RandomEvent> _events = new List<RandomEvent> ();
	
	public delegate void AnimationEnded();
	public static event AnimationEnded OnAnimationEnded;
	
	public delegate void AnimationStarted();
	public static event AnimationStarted OnAnimationStarted;
			
	public List<RandomEvent> events
	{
		get
		{
			print (gameObject.name +" " +_events.Count);
			return _events;
		}
	}
	
	static private UIManager _instance;
	static public UIManager instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.Find("UIManager").GetComponent<UIManager>();
			}
			
			return _instance;
		}
	}
	
	public float GetSccessRate()
	{
		if(_events.Count > eventWindow)
			return successRate;
		
		return 0;
	}
	
	public void BtnActionGetEvent(string input)
	{		
		print ("getting event from player");
	
		btnsAndQuestion.SetActive(false);
		
		int e = probs.GetEvent();
		print ("Resultado "+ e);
		string dirEsq = "";
		
		timeCount.SetActive(false);
		
		if(OnAnimationStarted != null)
			OnAnimationStarted();
		btnsAndQuestion.SetActive(false);
		
		RandomEvent eLog = new RandomEvent ();

		eLog.time = stopwatch;
		stopwatch = 0;
		eLog.resultInt = e;
		if(e == 0) //esquerda
		{
			dirEsq = "esquerda";
		}
		else if (e == 1)
		{
			dirEsq = "centro";
		}else
		{
			dirEsq = "direita";
		}
		
		eLog.result = dirEsq;
		
		eLog.optionChosen = input;
		
		if(input.Equals(dirEsq))
		{
			eLog.correct = true;
			success ++;
		}
		else
		{
			eLog.correct = false;
		}
		
		int targetAnim = probs.GetCurrMachineIndex ();
		if(targetAnim >= gkAnim.Length)
		{
			targetAnim = gkAnim.Length - 1;
		}
		if(input == "esquerda")
		{
			eLog.optionChosenInt = 0;
			if(eLog.correct)
			{
				gkAnim[targetAnim].Play("esq", dirEsq.Substring(0, 3));
			}
			else
			{
				gkAnim[targetAnim].Play("esq_goal", dirEsq.Substring(0, 3)+"_goal");
			}
		}
		else if(input == "direita")
		{
			eLog.optionChosenInt = 2;
			if(eLog.correct)
			{
				gkAnim[targetAnim].Play("dir", dirEsq.Substring(0, 3));
			}
			else
			{
				gkAnim[targetAnim].Play("dir_goal", dirEsq.Substring(0, 3)+"_goal");
			}
			
		}
		else 
		{
			eLog.optionChosenInt = 1;
			if(eLog.correct)
			{
				gkAnim[targetAnim].Play("cen", dirEsq.Substring(0, 3));
			}
			else
			{
				gkAnim[targetAnim].Play("cen_goal", dirEsq.Substring(0, 3)+"_goal");
			}
		}
			
		_events.Add(eLog);
		print ("log saved");
		print (events.Count);
		
		eventCount++;
		
		
		int successCountInWindow = 0;
		for(int i = 0; i < eventWindow; i++)
		{
			if(eventCount - 1 - i < 0)
			{
				break;
			}
			if(_events[eventCount - 1 - i].correct)
			{
				successCountInWindow ++;
			}
		}
		
		successRate = ((float) successCountInWindow)/((float)eventWindow);
	}

	public void SendEventsToServer()
	{
		if(_events != null && _events.Count > 0)
			ServerOperations.instance.RegisterPlay (GameFlowManager.instance, probs.CurrentMachineID(), success, successRate, _events);
	}

	public void CorrectPhaseArt()
	{
		int targetAnim = probs.GetCurrMachineIndex ();
		if(targetAnim >= gkAnim.Length)
		{
			targetAnim = gkAnim.Length - 1;
		}

		for(int i = 0; i < gkAnim.Length; i++)
		{
			if(i != targetAnim)
			{
				gkAnim[i].gameObject.SetActive(false);
			}
			else
			{
				gkAnim[i].gameObject.SetActive(true);
			}
		}
	}

	public int GetTotalLevelArts()
	{
		return gkAnim.Length;
	}

	public void ResetEventList()
	{
		SendEventsToServer ();
		_events = new List<RandomEvent> ();
		eventCount = 0;
		success = 0;
		successRate = 0;
		scoreMonitor.Reset();
	}
	
	public void PostAnimThings ()
	{
		stopwatch = 0;
		if(events[events.Count -1].correct)
		{
			cheer.gameObject.SetActive(true);
			pegoal.SetTrigger("pegoal");
		}
		else
		{
			lament.gameObject.SetActive(true);
		}
		
		if(successRate > .1)
		{
			progressionBar.Grow(successRate);
		}
		else
		{
			progressionBar.Grow(.1f);
		}
	}

	public void QuitGame()
	{
		GameFlowManager.instance.quitGameMenu.SetActive (true);
	}

	public void OnEnable()
	{
		OnAnimationEnded += PostAnimThings;
	}

	public void OnDisable()
	{
		OnAnimationEnded -= PostAnimThings;
	}
	
	int centerStateHash;
	int currentState;
	void Start () 
	{
		probs = ProbCalculator.instance;
		int targetAnim = probs.GetCurrMachineIndex ();
		if(targetAnim >= gkAnim.Length)
		{
			targetAnim = gkAnim.Length - 1;
		}
		centerStateHash = gkAnim[targetAnim].gk.GetCurrentAnimatorStateInfo(0).shortNameHash;
		currentState = centerStateHash;
	}

	void Update () 
	{
		int currAnim = probs.GetCurrMachineIndex ();
		if(currAnim >= gkAnim.Length)
		{
			currAnim = gkAnim.Length - 1;
		}

		AnimatorStateInfo currentBaseState = gkAnim[currAnim].gk.GetCurrentAnimatorStateInfo(0);
		
		if(currentState != currentBaseState.shortNameHash)
		{
			if (currentBaseState.shortNameHash == centerStateHash)
			{
				if(OnAnimationEnded != null)
					OnAnimationEnded();
				btnsAndQuestion.SetActive(true);
			}

		}

		if(btnsAndQuestion.activeInHierarchy)
		{
			if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				BtnActionGetEvent("centro");
				btnsAndQuestion.SetActive(false);
			}
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				BtnActionGetEvent("esquerda");
				btnsAndQuestion.SetActive(false);
			}
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				BtnActionGetEvent("direita");
				btnsAndQuestion.SetActive(false);
			}
		}

		stopwatch += Time.deltaTime;

		currentState = currentBaseState.shortNameHash;
	}
}
