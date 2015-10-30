using UnityEngine;
using System.Collections;

public interface Item {
	
	void start ();

	//道具名字
	string name{get;set;}

	//道具描述
	string description{get;set;}

	//获得道具剩余数量
	int GetNum();

	//道具分类
	ITEM_CATEGORY itemCategory{get;set;}

	//道具使用
	void UseItem();

}

//道具分类
public enum ITEM_CATEGORY{
	NONE = -1,
	SUPPLY = 0,		//消耗品
	EQUIPMENT 		//装备
}


