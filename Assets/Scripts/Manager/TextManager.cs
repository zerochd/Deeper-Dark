using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

	private Text tint;

	private Text messageInfo;

	public static TextManager instance = null;

	void Awake () {

		tint = GameObject.Find("Tint").GetComponent<Text>();
		messageInfo = GameObject.Find("MessageInfo").GetComponent<Text>();

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		
	}

	public void hideTint(){
		this.tint.text = "";
		this.tint.enabled = false;
		this.tint.color = new Color(1,1,1,1);

	}

	public void hideMessage(){
		this.messageInfo.text = "";
		this.messageInfo.enabled = false;
		this.messageInfo.color = new Color(1,1,1,1);
	}


	public void showTint(string text)
	{
		hideTint ();
		this.tint.text  = text;
		this.tint.enabled = true;
		StartCoroutine(fadeOut(tint));
	}

	public void showMessage(string text){
		hideMessage();
		if(this.messageInfo.GetComponent<PrintMessage>()){
//			Debug.Log ("send");
			this.messageInfo.GetComponent<PrintMessage>().SetMessage(text);
		}
//		this.messageInfo.text = text;
		this.messageInfo.enabled = true;
		StartCoroutine(fadeOut(messageInfo));
	}

	//淡出
	IEnumerator fadeOut(Text text){
		//延迟一秒
		yield return new WaitForSeconds(1f);
		while(text.color.a > 0.01f){
			float alpha = text.color.a - 0.01f;
			text.color = new Color(1,1,1,alpha);
			yield return null;
		}
	}




}
