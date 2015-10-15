using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// Toggle select.
/// </summary>
public class ToggleSelect : MonoBehaviour {

	//获取Toggles
	MyToggle[] toggles;

	//当前Toggle的下标
	int currentToggleIndex;

	public AudioClip selectClip;

	// Use this for initialization
	void Start () {
		toggles = GetComponentsInChildren<MyToggle>();
		if(toggles.Length!=0){
			toggles[0].isOn = true;
			currentToggleIndex = 0;
		}
	}

	void OnEnable(){
		toggles = GetComponentsInChildren<MyToggle>();
		if(toggles.Length!=0){
			toggles[0].isOn = true;
			currentToggleIndex = 0;
		}
	}

	void OnDisable(){
		for(int i = 0;i<toggles.Length;i++){
			toggles[i].isOn = false;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(toggles.Length == 0){
			return;
		}

		//向上滚动(上箭头,W)
		if(Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)){
			if(currentToggleIndex == 0){
				currentToggleIndex = toggles.Length -1;
			}
			else{
				currentToggleIndex --;
			}
			//设置toggle的isOn状态为true
			toggles[currentToggleIndex].isOn = true;
			//播放音效
			PlayerSelectSound();
		}


		//向下滚动(下箭头,S)
		if(Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.S)){
			if(currentToggleIndex == toggles.Length -1){
				currentToggleIndex = 0;
			}
			else{
				currentToggleIndex ++;
			}
			//设置toggle的isOn状态为true
			toggles[currentToggleIndex].isOn = true;
			//播放音效
			PlayerSelectSound();
		}

		//确定键调用各自toggle的方法
		if(Input.GetKeyDown(KeyCode.Space)){
			//使用菜单
			toggles[currentToggleIndex].StartMenu();
		}

	
	}

	//播放选择音效
	public void PlayerSelectSound(){
		if(selectClip!=null)
			AudioSource.PlayClipAtPoint(selectClip,Vector3.zero);
//		Debug.Log ("sound");
	}
}
