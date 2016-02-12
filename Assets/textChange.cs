using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class textChange : MonoBehaviour {
	public List<string> text = new List<string>();


	// Use this for initialization
	void Start () {
		text.Add ("This is an average man.");
		text.Add ("He goes to work every \n morning.");
		text.Add ("And then he works all \n day.");
		text.Add ("Nothing bad ever \n happens.");
		text.Add ("Nothing good ever \n happens.");
		text.Add ("His life is ordernary.");
		text.Add ("Lets try to keep it \n that way.");
		text.Add ("\n        -LEVEL ONE-");
		text.Add ("\n Average Man usually \n isn't hit by cars.");
		text.Add ("\n Use W,A,S, and D \n to control traffic.");
		StartCoroutine (ChangeText (text));
	}

	// Update is called once per frame
	void Update () {
	}

	IEnumerator ChangeText(List<string> text) {
		TextMesh tMesh = GetComponent<TextMesh> ();
		foreach (string str in text) {
			tMesh.text = str;
			yield return new WaitForSeconds (3);
		}
		UnityEditor.SceneManagement.EditorSceneManager.LoadScene("MainScene");

	}


}
