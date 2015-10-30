using UnityEngine;
using System.Collections;

public class Option : MonoBehaviour {
	
	public Transform optionPanel;
	public Transform gameOverPanel;

	// Use this for initialization
	void Start () {
		optionPanel = GameObject.Find("Canvas_HUD").transform.GetChild(2);
		gameOverPanel = GameObject.Find("Canvas_HUD").transform.GetChild(3);
		GameManager.SwitchOptionEvent +=SwitchOption;
		GameManager.switchGameOverEvent +=SwitchGameOver;
		//确保一开始option是关闭的
		if(optionPanel.gameObject.activeInHierarchy){
			Globe.game_state = Globe.GAME_STATE.RUNNING;
			optionPanel.gameObject.SetActive(false);
		}
		if(gameOverPanel.gameObject.activeInHierarchy){
			Globe.game_state = Globe.GAME_STATE.RUNNING;
			gameOverPanel.gameObject.SetActive(false);
		}
	}

	public void SwitchOption(){
		if(!optionPanel.gameObject.activeInHierarchy){
			Bag.Instance.DisableBag();
//			CameraControl.Instance.DisableCamerMap();
			MapCamera.DisableCamerMap();
			GameManager.Instance.StopTime();
			Globe.game_state = Globe.GAME_STATE.OPTION;
			optionPanel.gameObject.SetActive(true);
		}
		else{
			GameManager.Instance.StartTime();
//			Globe.game_state = Globe.GAME_STATE.RUNNING;
			optionPanel.gameObject.SetActive(false);
		}
	}

	public void SwitchGameOver(){
		if(!gameOverPanel.gameObject.activeInHierarchy){
			gameOverPanel.gameObject.SetActive(true);
		}
	}
}
