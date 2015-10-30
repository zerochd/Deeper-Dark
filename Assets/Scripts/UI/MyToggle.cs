using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyToggle : Toggle {

	public delegate void ToggleClickDelegate();
	public delegate void ShowDescriptionDelegate(Item item);
	
	public event ToggleClickDelegate ToggleClickEvent;
	public static event ShowDescriptionDelegate showDescriptionEvent;

	void Awake(){
		showDescriptionEvent = null;
	}

	void Start () {
		//获取当前toggle所在的group
		group = GetComponentInParent<ToggleGroup>();
	}
	
	public void Switch(){

		if(isOn){
			targetGraphic.color = new Color(1f,0.7f,0.7f,0.7f);
			if(GetComponent<ItemInBag>()){
				ShowDescription(GetComponent<ItemInBag>().GetItem());
			}
		}
		else{
			targetGraphic.color = new Color(1f,1f,1f,1f);
		}
	}

	//运行单击功能，根据绑定的其他脚本来注册该事件
	public void ToggleClick(){
		if(ToggleClickEvent!=null){
			ToggleClickEvent();
		}
	}

	public void ShowDescription(Item item){
		if(showDescriptionEvent != null){
			showDescriptionEvent(item);
		}
	}

}
