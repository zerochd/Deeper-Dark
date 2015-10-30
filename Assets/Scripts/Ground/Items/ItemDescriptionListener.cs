using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ItemDescriptionListener : MonoBehaviour {

//	void Awake(){
//		MyToggle.showDescriptionEvent += showDescription;
//	}

	void Start(){
		MyToggle.showDescriptionEvent += showDescription;
	}

	void showDescription(Item item){
		GetComponent<Text>().text = "\n"+item.description;
	}
}
