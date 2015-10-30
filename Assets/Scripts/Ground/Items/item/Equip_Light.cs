using UnityEngine;
using System.Collections;

public class Equip_Light : Item {

	public string name{get;set;}
	
	public string description{get;set;}

	public ITEM_CATEGORY itemCategory{get;set;}

//	public static int num = 1;

	public void start () {
		Debug.Log ("equip-light");
	}


	public Equip_Light(){
		name = "手电筒";
		itemCategory = ITEM_CATEGORY.EQUIPMENT;
		description = "手电筒，能照亮附近";
	}

	public int GetNum(){
//		if(num != 0)
//			return num--;
		return 0;
	}

	public static Equip_Light CreateInstance(){
		Equip_Light item = new Equip_Light();
		return item;
	}

	public void UseItem(){
		Debug.Log ("Equip light");

		if(Player.instance.equipment.childCount < 4){
			if(!Player.instance.equipment.GetComponentInChildren<Light>()){
//				Transform equipLight = new Transform();
//				equipLight.localPosition = new Vector3(0f,0f,-1f);
//				equipLight.gameObject.AddComponent<Light>().color = new Color(;
			}
			else{
				Light light = Player.instance.equipment.GetComponentInChildren<Light>();
				if(!light.enabled){
					light.enabled = true;
				}else{
					light.enabled = false;
				}
			}
		}
	}

}
