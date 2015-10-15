using UnityEngine;
using System.Collections;


public class GameStartMenu : MonoBehaviour {


	void Awake(){

		if(GetComponent<MyToggle>()){
			//给MyToggle注册事件
			GetComponent<MyToggle>().StartMenuEvent += GameStart;
		}
		else{
			Debug.LogError("no MyToggle");
		}
	}
	

    /// <summary>
    /// 点击开始游戏按钮
    /// </summary>
    public void GameStart()
    {
        //记录下一个场景的名称
        Globe.sceneName = "Level";
        //进入loading场景
        Application.LoadLevel("Loading");
    }
}
