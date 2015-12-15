using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogUser : MonoBehaviour 
{
	public GameObject gameModeText;
	public GameObject timeBtn;
	public GameObject noTimeBtn;
	public InputField alias;
	public InputField age;
	public Dropdown gender;
	public Dropdown education;
	public Dropdown destro;

	public void EnterData()
	{
		PlayerInfo.alias = alias.text;
		PlayerInfo.gender = gender.options[gender.value].text;
		PlayerInfo.age = age.text;
		PlayerInfo.education = education.options[education.value].text;
		PlayerInfo.destro = destro.options[destro.value].text;
		gameModeText.SetActive (true);
		timeBtn.SetActive(true);
		noTimeBtn.SetActive(true);
		gameObject.SetActive(false);	
	}

	public void DeleteHeaderGenero()
	{
//		int index = gender.options.FindIndex((Dropdown.OptionData o)=>{return o.text == "Gênero";});
//		print (index);
//			gender.value -= 1;
//			gender.options.RemoveAt(index);
	}

	// Use this for initialization
	void Start () 
	{

	}

	void OnEnable()
	{
		if(PlayerInfo.age == "" || PlayerInfo.alias == "" || PlayerInfo.gender == "")
		{
			gameModeText.SetActive (false);
			timeBtn.SetActive(false);
			noTimeBtn.SetActive(false);
		}
		else
		{
			gameModeText.SetActive (true);
			timeBtn.SetActive(true);
			noTimeBtn.SetActive(true);
			gameObject.SetActive(false);
		}
		
		gender.options.Clear ();
		gender.options.Add(new Dropdown.OptionData("Masculino"));
		gender.options.Add(new Dropdown.OptionData("Feminino"));
		gender.value = 0;
		
		education.options.Clear ();
		education.options.Add(new Dropdown.OptionData("Fundamental incompleto"));
		education.options.Add(new Dropdown.OptionData("Fundamental completo"));
		education.options.Add(new Dropdown.OptionData("Superior incompleto"));
		education.options.Add(new Dropdown.OptionData("Superior completo"));
		education.value = 0;

		destro.options.Clear ();
		destro.options.Add(new Dropdown.OptionData("Destro"));
		destro.options.Add(new Dropdown.OptionData("Canhoto"));
		destro.value = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
