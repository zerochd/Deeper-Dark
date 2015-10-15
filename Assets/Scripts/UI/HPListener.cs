using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPListener : MonoBehaviour {

	public Player player;

	Text HPText;

	int hp;

	string suffix = "%";
	
	// Use this for initialization
	void Start () {

		HPText = GetComponent<Text>();

		//完成界面更新HP事件
		GameManager.UpdateHPEvent += delegate() {
			hp = player.GetHP();
			HPText.text = hp+suffix;
		};

	}
	
//	// Update is called once per frame
//	void Update () {
////		hp = player.GetHP();
////		HPText.text = hp+suffix;
//	}







}
