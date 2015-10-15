using UnityEngine;
using System.Collections;

class Med_HP_Small : Item {

	int hp = 10;

	public string name{get;set;}

	public string description{get;set;}

	public bool only{get;set;}

	// Use this for initialization
	public void start () {
		Debug.Log ("med-hp-small");
	}

	public Med_HP_Small(){
		name = "小药瓶";
		description = "一枚普通的绿药";
		only = false;
	}
	
	public static Med_HP_Small CreateInstance(){
		Med_HP_Small item = new Med_HP_Small();
		return item;
	}


}
