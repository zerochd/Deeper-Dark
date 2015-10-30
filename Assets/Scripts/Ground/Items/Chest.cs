using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chest : MonoBehaviour {
	
	private bool isOpened = false;

	//宝箱打开音效
	public AudioClip chestisOpenedClip;

	//宝箱打开图像
	public Sprite treasureisOpened;

	//宝箱里的道具
//	public GameObject[] items;

	//宝箱里的道具列表
	public static List<GameObject> itemsList = new List<GameObject>();


	void Start(){
//		Debug.Log ("count:"+items.Length);

	}

	void Update(){
//		Debug.Log ("now:"+Chest.itemsList.Count);
	}

	void OnTriggerEnter2D(Collider2D other){
	
		//检测到攻击判定且未被打开,转换sprite,播放音效,设置为打开状态
		if(other.name.Equals("attackPoint") && isOpened == false){
			GetComponent<SpriteRenderer>().sprite = treasureisOpened;
//			SoundManager.Instance.PlaySingle("background",chestisOpenedClip);
			isOpened = true;
			int randomIndex;
			GameObject item = GetRandomItem(out randomIndex);
			GameObject go = Instantiate(item, this.transform.position, this.transform.rotation) as GameObject;

			//保证装备道具的唯一性
			if(go.GetComponent<ItemAdapter>().item.itemCategory.Equals(ITEM_CATEGORY.EQUIPMENT)){
				itemsList.Remove(item);
			}
			go.name = item.name;
			if(!go.GetComponent<ItemAdapter>()){
				Debug.LogError("no ItemAdapter");
			}

		}
	}

	GameObject GetRandomItem(out int randomIndex){
//		if(items.Length == 0){
//			Debug.LogError("no Item");
//		}
//		GameObject item = items[UnityEngine.Random.Range(0, items.Length)];
		if(itemsList.Count == 0){
			Debug.LogWarning("no Item");
		}
		randomIndex = UnityEngine.Random.Range(0, itemsList.Count);
		GameObject item = itemsList.ToArray()[randomIndex];

		return item;
	}




	
	

}
