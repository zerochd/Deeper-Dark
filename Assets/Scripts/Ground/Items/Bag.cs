using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Bag : UnitySingleton<Bag> {

//	public static Bag instance = null;

//	bool isOpened;

	public GameObject bagItem;

	public Transform bagHolder;

	public Transform bagPanel;

//	List<string> itemInBagList;

//	Dictionary<Item,int> itemInBagDic;

	public static Dictionary<string,Text> itemInBagDic;
	
//	void Awake () {
//		if (instance == null)
//			instance = this;
//		else if (instance != this)
//			Destroy (gameObject);
//		DontDestroyOnLoad (gameObject);
//
//	}

	void Start(){
//		isOpened = false;
		GameManager.SwitchBagEvent += SwicthBag;
//		itemInBagList = new List<string>();
		itemInBagDic = new Dictionary<string, Text>();

		////确保一开始bag是关闭的
		if(bagPanel.gameObject.activeInHierarchy){
			Globe.game_state = Globe.GAME_STATE.RUNNING;
			bagPanel.gameObject.SetActive(false);
		}

	}

	//添加到背包界面里
	public void AddBag(Item t1){
		//实例化bagItem
		GameObject go = Instantiate(bagItem) as GameObject;
		go.name = t1.name;
		go.GetComponentInChildren<Text>().text = t1.name;
		go.transform.SetParent(bagHolder);
		go.transform.localScale = Vector3.one;
	}

	//添加带有logo的道具到背包界面里
	public void AddBag(Item t1,Sprite itemSprite){

		//判断当前列表是否已经有该道具
		if(itemInBagDic.ContainsKey(t1.name)){
			if(t1.GetNum() != 0){
				//设置道具数量+1
				try{
					Text numText;
					if(itemInBagDic.TryGetValue(t1.name,out numText)){
						int nowNum;
	
						if(int.TryParse(numText.text.Substring(1),out nowNum)){
							nowNum++;
						}

						numText.text = "X"+nowNum;
					}
				
				}
				catch(UnityException exception){
					Debug.LogError("exception"+exception.StackTrace);
				}
			}
			else{
				return ;
			}
		}
		else{

			GameObject go = Instantiate(bagItem) as GameObject;

			//往字典里添加item的名字跟它的数量
			itemInBagDic.Add(t1.name,go.GetComponentsInChildren<Text>()[1]);

			//如果该item为supply,第一次添加显示后缀为X1，如果是equipment将不显示
			if(t1.itemCategory == ITEM_CATEGORY.SUPPLY){
				go.GetComponentsInChildren<Text>()[1].text = "X1";
			}

			t1.GetNum();
			go.name = t1.name;
			go.GetComponentInChildren<Text>().text = t1.name;
			go.GetComponentsInChildren<Image>()[1].sprite = itemSprite;
		
			//将prefab的Item类传递给UI里itemToggle
			go.GetComponent<ItemInBag>().SetItem(t1);
			go.GetComponent<ItemInBag>().itemNum = 1;
			go.transform.SetParent(bagHolder);
			go.transform.localScale = Vector3.one;
		}
	}

	//开关背包
	public void SwicthBag(){
		if(!bagPanel.gameObject.activeInHierarchy){
//			CameraControl.Instance.DisableCamerMap();
			MapCamera.DisableCamerMap();
			bagPanel.gameObject.SetActive(true);
			GameManager.Instance.StopTime();
			Globe.game_state = Globe.GAME_STATE.BAGING;
		}
		else{
			bagPanel.gameObject.SetActive(false);
			GameManager.Instance.StartTime();
			Globe.game_state = Globe.GAME_STATE.RUNNING;
		}
	}

	public void DisableBag(){
		bagPanel.gameObject.SetActive(false);
	}


}
