using UnityEngine;
using System.Collections;

public class Equip_Light : Item {

	public string name{get;set;}
	
	public string description{get;set;}

	public bool only{get;set;}

	public void start () {
		Debug.Log ("equip-light");
	}


	public Equip_Light(){
		name = "手电筒";
		description = "手电筒，能照亮附近";
		only = true;
	}

	public static Equip_Light CreateInstance(){
		Equip_Light item = new Equip_Light();
		return item;
	}

}
