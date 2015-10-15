using UnityEngine;
using System.Collections;

public class GameExitMenu : MonoBehaviour {

	void Awake(){
		
		if(GetComponent<MyToggle>()){
			//给MyToggle注册事件
			GetComponent<MyToggle>().StartMenuEvent += GameExit;
		}
		else{
			Debug.LogError("no MyToggle");
		}
	}
	
	public void GameExit()
	{
		Debug.Log ("GameExit");
//		Application.Quit();
	}
}
