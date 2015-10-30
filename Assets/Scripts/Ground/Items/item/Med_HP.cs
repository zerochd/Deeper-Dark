using UnityEngine;
using System.Collections;

class Med_HP : MonoBehaviour, Item{

	//生命值
	int hp;

	//药瓶使用声音
	private  AudioClip useMedSound;

	public string name{get;set;}

	public string description{get;set;}

	public ITEM_CATEGORY itemCategory{get;set;}

	public static int num = 10;

	// Use this for initialization
	public void start () {
		Debug.Log ("med-hp-small");
	}

	public Med_HP(){
		hp = 10;
		name = "小药瓶";
		itemCategory = ITEM_CATEGORY.SUPPLY;
		description = "一枚普通的绿药,能恢复少量的生命值";
	}

	public Med_HP(ITEM_TYPE med){
		switch(med){
			case ITEM_TYPE.MED_HP_BIG:{
				hp = 30;
				name = "大药瓶";
				itemCategory = ITEM_CATEGORY.SUPPLY;
				description = "一枚很大的绿药,能恢复很多的生命值";

			}
			break;
			case ITEM_TYPE.MED_HP_SMALL:{
				hp = 10;
				name = "小药瓶";
				itemCategory = ITEM_CATEGORY.SUPPLY;
				description = "一枚普通的绿药,能恢复少量的生命值";
			}break;
			default:Debug.LogError ("no this type in med");break;
		}
	}

	public Med_HP(ITEM_TYPE med,AudioClip useMedSound){
		switch(med){
		case ITEM_TYPE.MED_HP_BIG:{
			hp = 30;
			name = "大药瓶";
			itemCategory = ITEM_CATEGORY.SUPPLY;
			description = "一枚很大的绿药,能恢复很多的生命值";
			
		}
			break;
		case ITEM_TYPE.MED_HP_SMALL:{
			hp = 10;
			name = "小药瓶";
			itemCategory = ITEM_CATEGORY.SUPPLY;
			description = "一枚普通的绿药,能恢复少量的生命值";
		}break;
		default:Debug.LogError ("no this type in med");break;
		}
		this.useMedSound = useMedSound;
	}

	public int GetNum(){
		if(num != 0)
			return num--;
		return 0;
	}
	

	public static Med_HP CreateInstance(){
		Med_HP item = new Med_HP();
		return item;
	}

	public static Med_HP CreateInstance(ITEM_TYPE med){
		Med_HP item = new Med_HP(med);
		return item;
	}

	public static Med_HP CreateInstance(ITEM_TYPE med,AudioClip useMedSound){
		Med_HP item = new Med_HP(med,useMedSound);
		return item;
	}

	public void UseItem(){
		Player.instance.SetHP(-hp);
		if(useMedSound!=null){
			SoundManager.Instance.PlaySingle(SOUND_CHANNEL.BACKGROUND,useMedSound);
		}
	}


}
