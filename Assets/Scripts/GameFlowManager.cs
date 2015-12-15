using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using JsonFx.Json;

public class GameFlowManager : MonoBehaviour 
{
	public GameObject game;
	public CanvasGroup gameCanvas;
	public GameObject betweenLevels;
	public GameObject intro;
	public GameObject introMenu;
	public GameObject gameModeMenu;
	public GameObject versusMode;
	public ScoreMonitor scoreMonitor;
	public bool useTimer;
	public float defaultTime;
	public GameObject userInfoForm;
	public GameObject quitGameMenu;
	public int playLimit = 0;

	private NewProbCalculator probCalculator;
	private UIManager uiManager;
	private UIManager uiManagerVersusMode;
	private bool barCalculated = false;
	private bool onVersusMode;
	private VersusMode versusModeManager;

	static private GameFlowManager _instance;
	static public GameFlowManager instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
			}

			return _instance;
		}
	}

	void OnEnable ()
	{
		UIManager.OnAnimationEnded += OnAnimationEnded;
		UIManager.OnAnimationStarted += OnAnimationStarted;
	}
	
	void OnDisable()
	{
		UIManager.OnAnimationEnded -= OnAnimationEnded;
		UIManager.OnAnimationStarted -= OnAnimationStarted;
	}
	
	IEnumerator StartCountDown(float waitTime)
	{
		print("waited for " + waitTime + " seconds, shot");
		if(onVersusMode)
		{
			versusModeManager.ui.timeCount.GetComponent<CountDown>().time = (int)waitTime;
			versusModeManager.ui.timeCount.SetActive(true);
			yield return new WaitForSeconds(waitTime);
			versusModeManager.ChooseGKMove();
		}
		else
		{
			uiManager.timeCount.GetComponent<CountDown>().time = (int)waitTime;
			uiManager.timeCount.SetActive(true);
			yield return new WaitForSeconds(waitTime);
			var pointer = new PointerEventData(EventSystem.current);
			ExecuteEvents.Execute(uiManager.optBtns[2], pointer, ExecuteEvents.pointerClickHandler);
		}
	}
	
	void OnAnimationStarted ()
	{

	}
	
	void OnAnimationEnded ()
	{
		if(onVersusMode)
		{
			versusModeManager.ui.scoreMonitor.UpdateMenu();
		}
		else
		{
			scoreMonitor.UpdateMenu();
		}
		if (useTimer)
		{
			StartCoroutine(StartCountDown(defaultTime));
		}
		if(!onVersusMode)
		{
			if(playLimit > 0)
			{
				if(playing && uiManager.events.Count >= playLimit)
				{
					ShowInBetween();
				}
			}
			else if(playing && uiManager.GetSccessRate() * 100 >= probCalculator.GetCurrentLimitValue())
			{
				ShowInBetween();
			}
			
		}
	}
	
	void Start () 
	{
		probCalculator = NewProbCalculator.instance;
		uiManager = UIManager.instance;
//		versusModeManager = VersusMode.instance;
		intro.SetActive(true);
		introMenu.SetActive(true);
//		versusMode.SetActive(false);
		gameModeMenu.SetActive(false);
		betweenLevels.SetActive(false);
		gameCanvas.interactable = false;
		game.SetActive(false);
		quitGameMenu.SetActive (false);
	}
	
	public void GoToIntro()
	{
		print ("go to intro");
		if(game.activeInHierarchy)
		{
			uiManager.ResetEventList();
		}
		playing = false;
		intro.SetActive(true);
		betweenLevels.SetActive(false);
		gameCanvas.interactable = false;
		game.SetActive(false);		
		introMenu.SetActive(true);
		gameModeMenu.SetActive(false);
		probCalculator.ResetToInitialMachine ();
	}
	
	public void ShowGameModeMenu()
	{
		introMenu.SetActive(false);
		gameModeMenu.SetActive(true);
		userInfoForm.SetActive (true);
		versusMode.SetActive(false);
	}
	

	public void NewGame (bool timer)
	{
		useTimer = timer;
		probCalculator.ResetToInitialMachine ();
		uiManager.CorrectPhaseArt ();
		StartGame();	
	}
	public void NewGame ()
	{
		useTimer = false;
		probCalculator.ResetToInitialMachine ();
		uiManager.CorrectPhaseArt ();
		StartGame();	
	}
	public void StartGame()
	{
		uiManager.ResetEventList();
		game.SetActive(true);
		intro.SetActive(false);
		betweenLevels.SetActive(false);
		gameCanvas.interactable = true;
		versusMode.SetActive(false);
		quitGameMenu.SetActive (false);

		playing = true;
		barCalculated = false;
		uiManager.progressionBar.Grow (0.1f);
		if (useTimer)
		{
			StartCoroutine(StartCountDown(defaultTime));
		}
	}
	
	public void NextLevel()
	{
		print ("To next level and beyond.... not really");
		if(!probCalculator.CanGoToNextMachine())
		{
			uiManager.ResetEventList();
			GoToIntro();
		}
		else
		{
			uiManager.ResetEventList();
			probCalculator.GotoNextMachine();
			uiManager.CorrectPhaseArt();
			StartGame();		
		}		
	}
	
	public void ShowInBetween()
	{
		playing = false;
		if(probCalculator.CanGoToNextMachine())
		{			
			game.SetActive(true);
			intro.SetActive(false);
			betweenLevels.SetActive(true);
			if(probCalculator.GetCurrMachineIndex() + 1 >= uiManager.GetTotalLevelArts())
			{
				betweenLevels.GetComponent<BetweenLevelsController>().PostEndGame();
			}
			else
			{
				betweenLevels.GetComponent<BetweenLevelsController>().MiddleGame();
			}

			gameCanvas.interactable = false;
			versusMode.SetActive(false);
		}
		else
		{
			uiManager.ResetEventList();
			game.SetActive(true);
			intro.SetActive(false);
			betweenLevels.SetActive(true);
			betweenLevels.GetComponent<BetweenLevelsController>().EndGame();
			gameCanvas.interactable = false;
			versusMode.SetActive(false);
//			GoToIntro();
		}
	}
	
	public void LoadVersusMode ()
	{
		gameModeMenu.SetActive(false);
		versusMode.SetActive(true);
		intro.SetActive(false);
		betweenLevels.SetActive(false);
		gameCanvas.interactable = true;
		playing = true;
		barCalculated = false;
		onVersusMode = true;
		useTimer = true;
		StartCoroutine(StartCountDown(defaultTime));		
	}

	public void CloseApp()
	{
		print ("Closing");
		Application.Quit ();
	}

	bool playing = false;
	public void Update()
	{
				
		if(!barCalculated && playing)
		{
			uiManager.progressionBar.RecalculateBarFactor();
			barCalculated = true;
		}
	}
}

//Bom, preciso voltar pro role do professor, mas precisava te falar isso. 