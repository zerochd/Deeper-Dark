using UnityEngine;
using System.Collections;

public class GameInfoMenu : MonoBehaviour {

	void Awake(){
		
		if(GetComponent<MyToggle>()){
			//给MyToggle注册事件
			GetComponent<MyToggle>().StartMenuEvent += GameInfo;
		}
		else{
			Debug.LogError("no MyToggle");
		}
	}

	public void GameInfo()
	{
		Debug.Log ("GameInfo");
	}
}
