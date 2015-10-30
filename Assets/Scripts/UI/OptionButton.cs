using UnityEngine;
using System.Collections;

public class OptionButton : MonoBehaviour {

	public OPTION_BUTTON_TYPE optionButtonType = OPTION_BUTTON_TYPE.NONE;

	//选项按钮类型
	public enum OPTION_BUTTON_TYPE{
		NONE = -1,
		GAMESTART = 0,
		GAMEINFO,
		GAMEEXIT,
		GAMERESUME,
		GAMERETURN,
		GAMERESTART
	}

	void Awake(){
//		if(GetComponent<MyToggle>()){
//
//			//给MyToggle注册事件
//			switch(optionButtonType){
//			case OPTION_BUTTON_TYPE.GAMESTART:GetComponent<MyToggle>().ToggleClickEvent += GameStart;break;
//			default:Debug.LogError("no this type");break;
//			}
//
//		}
//		else{
//			Debug.LogError("no MyToggle");
//		}
	}

	void Start(){
		if(GetComponent<MyToggle>()){
			
			//给MyToggle注册事件
			switch(optionButtonType){
			case OPTION_BUTTON_TYPE.GAMESTART:GetComponent<MyToggle>().ToggleClickEvent += GameStart;break;
			case OPTION_BUTTON_TYPE.GAMEINFO:GetComponent<MyToggle>().ToggleClickEvent += GameInfo;break;
			case OPTION_BUTTON_TYPE.GAMEEXIT:GetComponent<MyToggle>().ToggleClickEvent += GameExit;break;
			case OPTION_BUTTON_TYPE.GAMERESUME:GetComponent<MyToggle>().ToggleClickEvent+= GameResume;break;
			case OPTION_BUTTON_TYPE.GAMERETURN:GetComponent<MyToggle>().ToggleClickEvent+= GameReturn;break;
			case OPTION_BUTTON_TYPE.GAMERESTART:GetComponent<MyToggle>().ToggleClickEvent+= GameRestart;break;
			default:Debug.Log("no this type");break;
			}
			
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

	/// <summary>
	/// 点击介绍游戏
	/// </summary>
	public void GameInfo()
	{
		Debug.Log ("GameInfo");
	}

	/// <summary>
	/// 点击退出游戏
	/// </summary>
	public void GameExit()
	{
		Debug.Log ("GameExit");
		//		Application.Quit();
	}

	/// <summary>
	/// 游戏恢复 	
	/// </summary>
	public void GameResume(){
		GameManager.Instance.StartTime();
//		Option.Instance.SwitchOption();
		GameObject.Find("OptionPanel").gameObject.SetActive(false);
	}

	/// <summary>
	/// 回到主选单
	/// </summary>
	public void GameReturn(){
		Debug.Log ("GameReturn");
		Globe.sceneName = "Menu";
		GameManager.Instance.Dispose();
		CameraControl.Instance.ResetCameraPosition();
		Globe.MeetBoss = false;
		GameManager.Instance.StartTime();
		foreach(GameObject go in Globe.instanceGameObjects){
			Destroy(go);
		}
		Globe.instanceGameObjects.Clear();
		Application.LoadLevel("Menu");

	}

	/// <summary>
	/// 重新开始游戏 	
	/// </summary>
	public void GameRestart(){
		//记录下一个场景的名称
		Globe.sceneName = "Level";
//		Destroy(GameManager.instance.gameObject);
		GameManager.Instance.Dispose();
		CameraControl.Instance.ResetCameraPosition();
		Globe.MeetBoss = false;
		GameManager.Instance.StartTime();
		//进入loading场景
//		Application.LoadLevel("Loading");
		Application.LoadLevel("Level");
	}

}
