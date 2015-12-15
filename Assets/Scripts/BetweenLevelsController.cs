using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BetweenLevelsController : MonoBehaviour 
{
	public GameObject btnExit;
	public GameObject btnContinue;
	public Text msg;

	private readonly string endMsg = "Parabéns!! Você é um ótimo goleiro! Obrigado por jogar o NeuroGol!";
	private readonly string middleMsg = "Parabéns, você atingiu o próximo nível!";
	private readonly string postEndMsg = "Parabéns, Agora você já é um profissional. Deseja encarar desafios ainda maiores?";

	// Use this for initialization
	void Start () {
		btnContinue.SetActive (true);
		msg.text = middleMsg;
	}

	public void MiddleGame()
	{
		btnContinue.SetActive (true);
		msg.text = middleMsg;
	}

	public void PostEndGame()
	{
		btnContinue.SetActive (true);
		msg.text = postEndMsg;
	}

	public void EndGame()
	{

		btnContinue.SetActive (false);
		msg.text = endMsg;
	}
}
