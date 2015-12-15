using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System.IO;

public class TreeCell
{
	public float probEvent0;
	public TreeCell right;
	public TreeCell left;
	public string label;
	public int id;
			
	public TreeCell ()
	{
		right = left = null;
	}
}

[System.Serializable]
public class HeapCell
{
	public bool useThisNode;
	public float probEvent0;
	public string label;
	
	public HeapCell (float prob, string lbl)
	{
		probEvent0 = prob;
		label = lbl;
	}
}

public class ProbCalculator : MonoBehaviour 
{
	public List <HeapCell> treeInput;

	private TreeCell tree;
	private TreeCell currentState;
	
	static private ProbCalculator _instance;
	static public ProbCalculator instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.Find("ProbCalculator").GetComponent<ProbCalculator>();
			}
			
			return _instance;
		}
	}
	
	JsonTreeNode LoadJson()
	{
		FileInfo theSourceFile = null;
		StreamReader reader = null;
		
		theSourceFile = new FileInfo (Application.dataPath + "/Trees/tree.txt");
		if ( theSourceFile != null && theSourceFile.Exists )
			reader = theSourceFile.OpenText();
		
		string json = "";
		if ( reader == null )
		{
			Debug.Log("puzzles.txt not found or not readable");
		}
		else
		{
			// Read each line from the file
			string txt;
			while ( (txt = reader.ReadLine()) != null )
			{
				json += txt;
				Debug.Log("-->" + txt);
			}
		}
		
		print (json);
		
		JsonTreeNode t = null;
		if(json != "")
			t = JsonReader.Deserialize<JsonTreeNode> (json);
		
		return t;
	}
	
	void CreateTreeFromJson (TreeCell t, JsonTreeNode j)
	{
		if(j == null)
		{
			return;
		}
		
		t.label = j.label;
		t.probEvent0 = j.GetProbEvent0();
		t.id = j.GetId();
		
		if(j.right != null)
		{
			TreeCell tmp = new TreeCell ();
			CreateTreeFromJson(tmp, j.right);
			t.right = tmp;
		}
		if(j.left != null)
		{
			TreeCell tmp = new TreeCell ();
			CreateTreeFromJson(tmp, j.left);
			t.left = tmp;
		}
		
	}
	
	void CreateTreeFromHeapInput(TreeCell t, int inputIndex)
	{
		if(inputIndex > treeInput.Count || !treeInput[inputIndex].useThisNode)
		{
			return;
		}
		
		t.label = treeInput[inputIndex].label;
		t.probEvent0 = treeInput[inputIndex].probEvent0;
		t.id = inputIndex;
		
		int i = 2 * inputIndex + 1;
		if(i < treeInput.Count && treeInput[i].useThisNode)
		{
			TreeCell tmp = new TreeCell ();
			CreateTreeFromHeapInput(tmp, i);
			t.right = tmp;
		}
		i = 2 * inputIndex + 2;
		if(i < treeInput.Count && treeInput[i].useThisNode)
		{
			TreeCell tmp = new TreeCell ();
			CreateTreeFromHeapInput(tmp, i);
			t.left = tmp;
		}
	}

	void PrintTree(TreeCell t)
	{
		if(t != null)
		{
			print(t.id + " "  + t.label + " " + t.probEvent0);
			
			PrintTree(t.right);
			PrintTree(t.left);
		}
	}
	
	void Start () 
	{
		tree = new TreeCell();
		currentState = tree;
		
		Text treeSource = GameObject.Find("TreeSource").GetComponent<Text>();
		
		JsonTreeNode t = LoadJson();
		if(t != null)
		{
			CreateTreeFromJson(tree, t);
			treeSource.text = "Recursos carregados do arquivo";
		}
		else
		{
			CreateTreeFromHeapInput(tree, 0);
			treeSource.text = "Recursos carregados do programa";
		}
			
		PrintTree(tree);
		print ("Tree loaded");
	}
	
	public int GetCurrentStateId()
	{
		return currentState.id;
	}

	public TreeCell GetCurrentState()
	{
		return currentState;
	}
	
	// This returns a random value for a current state and advances to the next
	public int GetEvent ()
	{
		float r  = Random.Range(0.0f, 1.0f);
		
		int result = 0;
		
		if(r > currentState.probEvent0)
		{
			result = 1;
			if(currentState.left != null)
				currentState = currentState.left;
			else
				currentState = tree;
		}
		else
		{
			if(currentState.right != null)
				currentState = currentState.right;
			else
				currentState = tree;
		}
		
		return result;
	}
	

	
	void Update () 
	{
	
	}
}
