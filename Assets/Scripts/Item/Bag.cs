using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bag : MonoBehaviour {

	public static Bag instance = null;

	bool isOpened;

	public GameObject bagItem;

	public Transform bagHolder;

	public Transform bagPanel;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		
	}

	void Start(){
		isOpened = false;
		GameManager.SwitchBagEvent += SwicthBag;
	}

	//添加到背包界面里
	public void AddBag(Item t1){
		//实例化bagItem
		GameObject go = Instantiate(bagItem) as GameObject;
		go.name = t1.name;
		go.GetComponentInChildren<Text>().text = t1.name;
		//GetComponentsInChildren<Image>()得到Background和Image,取Image
		Debug.Log ("have:"+go.GetComponentsInChildren<Image>()[1].name);

		go.GetComponent<ItemInBag>().item = t1;
		go.transform.SetParent(bagHolder);
		go.transform.localScale = Vector3.one;
//		string name = go.GetComponentsInChildren<Image>()[0].name;
//		string name = go.GetComponentInChildren<Image>().gameObject.name;
//		Debug.Log ("have:"+go.GetComponentInChildren<Text>().name);
//		Debug.Log ("type:"+t1.GetType());
//		Debug.Log ("name:"+((Item)t1).name);
	}

	//添加带有logo的道具到背包界面里
	public void AddBag(Item t1,Sprite itemSprite){
		GameObject go = Instantiate(bagItem) as GameObject;
		go.name = t1.name;
		go.GetComponentInChildren<Text>().text = t1.name;
		//GetComponentsInChildren<Image>()得到Background和Image,取Image
//		Debug.Log ("have:"+go.GetComponentsInChildren<Image>()[1].name);
		go.GetComponentsInChildren<Image>()[1].sprite = itemSprite;
		go.GetComponent<ItemInBag>().item = t1;
		go.transform.SetParent(bagHolder);
		go.transform.localScale = Vector3.one;
	}

	void SwicthBag(){
		if(!bagPanel.gameObject.activeInHierarchy){
			Globe.game_state = Globe.GAME_STATE.BAGING;
			bagPanel.gameObject.SetActive(true);
		}
		else{
			Globe.game_state = Globe.GAME_STATE.RUNNING;
			bagPanel.gameObject.SetActive(false);
		}
	}





}
