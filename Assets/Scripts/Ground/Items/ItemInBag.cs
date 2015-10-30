using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ItemInBag : MonoBehaviour {

	private Item item;

	//背包里道具的数量
	public int itemNum{get;set;}

	void Start(){

//		GetComponent<MyToggle>().StartMenuEvent += UseSupply;
	}

	public Item GetItem(){
		return item;
	}

	public void SetItem(Item item){
		this.item = item;
		switch(item.itemCategory){
		case ITEM_CATEGORY.SUPPLY:GetComponent<MyToggle>().ToggleClickEvent += UseSupply;break;
		case ITEM_CATEGORY.EQUIPMENT:GetComponent<MyToggle>().ToggleClickEvent += UseEquipment;break;
		}

	}

	//使用消耗品
	void UseSupply(){
//		Debug.Log ("UseSupply");
		if(Bag.itemInBagDic.ContainsKey(item.name)){

			//获取当前item数量
			itemNum = int.Parse(GetComponentsInChildren<Text>()[1].text.Substring(1));

			//item数量-1
			itemNum --;

			//使用item效果
			item.UseItem();

			//当数量为0时，删除bagDic里相应的值
			if(itemNum == 0){

				Bag.itemInBagDic.Remove(item.name);

				GetComponentInParent<ToggleSelect>().DeleteToggle(this.gameObject);
			}
			else{
				GetComponentsInChildren<Text>()[1].text = "X" + itemNum;
			}

		}


	}

	//使用装备
	void UseEquipment(){
//		Debug.Log ("UseEquipment");
		//设置装备显示
		if(GetComponentsInChildren<Text>()[1].text == string.Empty)
			GetComponentsInChildren<Text>()[1].text = "E";
		else{
			GetComponentsInChildren<Text>()[1].text = string.Empty;
		}

		item.UseItem();

	}
	
}
