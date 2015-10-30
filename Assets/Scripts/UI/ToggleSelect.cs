using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// Toggle select.
/// </summary>
public class ToggleSelect : MonoBehaviour {

	//获取Toggles
	[HideInInspector]
	private MyToggle[] toggles;

	//当前Toggle的下标
	[HideInInspector]
	private int currentToggleIndex;

	//选择音效
	public AudioClip selectClip;

	void Awake(){
			
	}

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
		//每次开启都从第一个开始
		for(int i = 0;i<toggles.Length;i++){
			toggles[i].isOn = false;
		}
		if(toggles.Length!=0){
			toggles[0].isOn = true;
			currentToggleIndex = 0;
		}
	}

//	void OnDisable(){
//		toggles = GetComponentsInChildren<MyToggle>();
//		for(int i = 0;i<toggles.Length;i++){
//			toggles[i].isOn = false;
//		}
//	}
	
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

		//确定键调用各自toggle的方法(空格或回车)
		if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return)){
			//当该组件绑定到Menu GameObject时,使用菜单
//			if(this.name.Equals("Menu")){
//				toggles[currentToggleIndex].StartMenu();
//			}
//			//当该组件绑定到道具界面时，使用道具
//			else if(this.name.Equals("ItemList")){
//				toggles[currentToggleIndex].StartMenu();
//			}
			toggles[currentToggleIndex].ToggleClick();
		}

	
	}

	//播放选择音效
	public void PlayerSelectSound(){
		if(selectClip!=null){
//			AudioSource.PlayClipAtPoint(selectClip,Vector3.zero);
			SoundManager.Instance.PlaySingle(SOUND_CHANNEL.BACKGROUND,selectClip);
		}
//		Debug.Log ("sound");
	}

	//删除Toggle
	public void DeleteToggle(GameObject toggle){
		if(toggles.Length >= 2){
			if(currentToggleIndex != toggles.Length-1)
				currentToggleIndex++;
			else
				currentToggleIndex = 0;
			toggles[currentToggleIndex].isOn = true;
		}
		//必须用DestroyImmediate
		DestroyImmediate(toggle);
		toggles = GetComponentsInChildren<MyToggle>();
		if(currentToggleIndex != 0)
			currentToggleIndex--;

	}
}
