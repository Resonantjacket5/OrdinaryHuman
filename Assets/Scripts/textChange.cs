using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class textChange : MonoBehaviour {
	public List<string> text = new List<string>();

	[TextArea(15,10)]
	public string toshow;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0-10.52f, 4.25f, 0);
		text =  toshow.Split ('#').ToList();
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
