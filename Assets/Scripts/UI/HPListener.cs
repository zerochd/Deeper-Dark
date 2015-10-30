using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPListener : MonoBehaviour {

	public Player player;

	Text HPText;

	int hp;

	int preHP;

	string suffix = "%";
	
	// Use this for initialization
	void Start () {

		HPText = GetComponent<Text>();

		preHP = player.GetHP();

		//完成界面更新HP事件
		GameManager.UpdateHPEvent += delegate() {
			hp = player.GetHP();

			//hp逐渐变化
//			if(Mathf.Abs(preHP-hp) >=  float.Epsilon){
//				preHP += Mathf.Sign(hp-preHP) * Time.unscaledDeltaTime * 20f;
//				HPText.text = (int)preHP+suffix;
//			}
			StartCoroutine(HpSmoothChange());

		};
	}

	//HP平滑变化
	IEnumerator HpSmoothChange(){
		//扣血显示红色，加血显示绿色
		if(preHP < hp){
			HPText.color = Color.green;
		}
		else{
			HPText.color = Color.red;
		}
		if(preHP != hp){
			preHP += (hp - preHP)/Mathf.Abs(preHP - hp);
//			Debug.Log ("preHP:"+preHP);
			HPText.text = preHP+suffix;
			yield return new WaitForSeconds(0.1f);
		}
		else{
			HPText.color = Color.white;
		}
	}
}
