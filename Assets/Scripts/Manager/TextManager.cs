using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum SHOW_TYPE{
	NONE=-1,
	FADEIN=0	//淡入
}


public class TextManager : UnitySingleton<TextManager> {

	private Text tint;

	private Text messageInfo;

	private Text bossDialog;

//	public static TextManager instance = null;

	void Awake () {

		tint = GameObject.Find("Tint").GetComponent<Text>();
		messageInfo = GameObject.Find("MessageInfo").GetComponent<Text>();
		bossDialog = GameObject.Find("BossDialog").GetComponent<Text>();

//		if (instance == null)
//			instance = this;
//		else if (instance != this)
//			Destroy (gameObject);
//		DontDestroyOnLoad (gameObject);
		
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

	public void showTint(string text,SHOW_TYPE textShowType){
		switch(textShowType){
		case SHOW_TYPE.NONE:showTint(text);break;
		case SHOW_TYPE.FADEIN:{
			hideTint ();
			this.tint.text  = text;
			this.tint.enabled = true;
			StartCoroutine(fadeIn(tint));
//			iTween.ColorTo(this.tint.gameObject,Color.red,2.5f);
		}break;
		default:break;
		}
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

	public void ShowBossDialog(string text,Vector3 vec){
		this.bossDialog.text = "";
		this.bossDialog.enabled = false;
		this.bossDialog.rectTransform.position = vec;
		if(this.bossDialog.GetComponent<PrintMessage>()){
			//			Debug.Log ("send");
			this.bossDialog.GetComponent<PrintMessage>().SetMessage(text);
		}else{
		this.bossDialog.text = text;
		
		}
		this.bossDialog.enabled = true;
		Invoke("BossTextDisable",1f);
	}

	public void MoveTint(Vector3 moveEnd){
		moveEnd = this.tint.rectTransform.position + moveEnd;
		StartCoroutine(Move(this.tint,moveEnd));
	}

	public void ScaleTint(float scaleSize){
		
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

	//淡入
	IEnumerator fadeIn(Text text){
		text.color = new Color(1,1,1,0f);
		//延迟一秒
		yield return new WaitForSeconds(1f);
		while(text.color.a < 1f){
			float alpha = text.color.a + 0.01f;
			float red = Mathf.Lerp(text.color.a,Color.red.r,Time.deltaTime);
			float blue = Mathf.Lerp(text.color.b,Color.red.b,Time.deltaTime);
			float green = Mathf.Lerp(text.color.g,Color.red.g,Time.deltaTime);
			text.color = new Color(red,blue,green,alpha);

			yield return null;
		}
	}

	//淡入
	IEnumerator Move(Text text,Vector3 moveEnd){
		float sqrRemainingDistance = (text.rectTransform.position - moveEnd).sqrMagnitude;
		while (sqrRemainingDistance > float.Epsilon){
			Vector3 newPosition = Vector3.MoveTowards(text.rectTransform.position, moveEnd, 10f*Time.deltaTime);
			text.rectTransform.position = newPosition;
			sqrRemainingDistance = (text.rectTransform.position - moveEnd).sqrMagnitude;
			yield return null;
		}
	}


	void BossTextDisable(){
		this.bossDialog.text = "";
		this.bossDialog.enabled = false;
	}




}
