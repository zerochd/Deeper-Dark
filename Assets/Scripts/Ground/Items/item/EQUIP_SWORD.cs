using UnityEngine;
using System.Collections;

public class EQUIP_SWORD : Item {

	int e_damage = 2;

	public string name{get;set;}
	
	public string description{get;set;}
	
	public ITEM_CATEGORY itemCategory{get;set;}

	// Use this for initialization
	public void start () {
		Debug.Log ("med-EQUIP_SWORD-small");
	}
	
	public EQUIP_SWORD(){
		name = "短剑";
		itemCategory = ITEM_CATEGORY.EQUIPMENT;
		description = "普通的短剑，能造成额外伤害";
		
	}

	public int GetNum(){
		return 0;
	}
	
	public static EQUIP_SWORD CreateInstance(){
		EQUIP_SWORD item = new EQUIP_SWORD();
		return item;
	}
	
	public void UseItem(){
		if(Player.instance.GetDamage() == 1 ){
			Player.instance.SetDamage(1+e_damage);
		}
		else{
			Player.instance.SetDamage(1);
		}
	}

}
