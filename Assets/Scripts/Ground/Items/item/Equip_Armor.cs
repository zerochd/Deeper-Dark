using UnityEngine;
using System.Collections;

public class Equip_Armor : Item {

	int e_armor = 4;

	public string name{get;set;}
	
	public string description{get;set;}
	
	public ITEM_CATEGORY itemCategory{get;set;}

	public static int num = 1;

	public void start () {
//		Debug.Log ("equip-light");
	}

	public void UseItem(){
//		Debug.Log ("use armor");
		if(Player.instance.armor == 1 ){

			Player.instance.armor = 1 + e_armor;
//			Debug.Log ("Player.instance.armor"+Player.instance.armor);
		}
		else{
			Player.instance.armor = 1;
		}
	}

	public Equip_Armor(){
		name = "护甲";
		itemCategory = ITEM_CATEGORY.EQUIPMENT;
		description = "普通的护甲，能够提升些许防御力";
		
	}

	public static Equip_Armor CreateInstance(){
		Equip_Armor item = new Equip_Armor();
		return item;
	}

	public int GetNum(){
		if(num != 0)
			return num--;
		return 0;
	}
}
