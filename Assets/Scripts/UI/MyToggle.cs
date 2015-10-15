using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyToggle : Toggle {

	public delegate void StartMenuDelegate();
	public delegate void ShowDescriptionDelegate(Item item);
	
	public event StartMenuDelegate StartMenuEvent;
	public static event ShowDescriptionDelegate showDescriptionEvent;


	void Start () {
		//获取当前toggle所在的group
		group = GetComponentInParent<ToggleGroup>();
	}
	
	public void Switch(){

		if(isOn){
			targetGraphic.color = new Color(1f,0.7f,0.7f,0.7f);
			ShowDescription(GetComponent<ItemInBag>().item);
		}
		else{
			targetGraphic.color = new Color(1f,1f,1f,1f);
		}
	}

	//运行菜单功能，根据绑定的其他脚本来注册该事件
	public void StartMenu(){
		if(StartMenuEvent!=null){
			StartMenuEvent();
		}
	}

	public void ShowDescription(Item item){
		if(showDescriptionEvent != null){
			showDescriptionEvent(item);
		}
	}

}
