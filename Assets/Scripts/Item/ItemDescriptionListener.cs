using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ItemDescriptionListener : MonoBehaviour {

	void Awake(){
		MyToggle.showDescriptionEvent += showDescription;
	}

	void showDescription(Item item){
		GetComponent<Text>().text = item.description;
	}
}
