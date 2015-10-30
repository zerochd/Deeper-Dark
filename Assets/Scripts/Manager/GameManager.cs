using UnityEngine;
using System.Collections;

public class GameManager : UnitySingleton<GameManager> {

	//不同平台下StreamingAssets的路径是不同的
	public static string PathURL;
		
    //单例GameManager变量
//    public static GameManager instance = null;

	public delegate void VoidDelegate();

	//HP更新
	public static event VoidDelegate UpdateHPEvent;

	//分数更新
	public static event VoidDelegate UpdateScroeEvent;

	//开启关闭背包
	public static event VoidDelegate SwitchBagEvent;

	//开启关闭地图
	public static event VoidDelegate SwitchMapEvent;

	//开启关闭系统选项
	public static event VoidDelegate SwitchOptionEvent;

	//游戏结束选项
	public static event VoidDelegate switchGameOverEvent;

//	bool pause = false;
	

//    void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else if (instance != this)
//            Destroy(gameObject);
//        DontDestroyOnLoad(gameObject);	
//	
//    }

	// Use this for initialization
	void Start () {
		//初始化为运行状态
		Globe.game_state = Globe.GAME_STATE.RUNNING;

		//设置assetbundle路径
		PathURL =
		#if UNITY_ANDROID
		"jar:file://" + Application.dataPath + "!/assets/";
		#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
		#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
		"file://" + Application.dataPath + "/ABs/";
		#else
		string.Empty;
		#endif

		//读取AssetBundle
		StartCoroutine(LoadAssetBundle());

	}
	
	// Update is called once per frame
	void Update () {


		//判断游戏是否结束
		IsGameOver();

		//更新HP
		UpdateHP();

		//更新分数
		UpdateScore();

		#region 功能辅助
		//打开背包(I键)
		if(Input.GetKeyDown(KeyCode.I)){
//			Debug.Log ("Globe.game_state"+Globe.game_state);

			//不在地图界面且不在系统菜单
			if(Globe.game_state != Globe.GAME_STATE.OPTION
			   &&Globe.game_state != Globe.GAME_STATE.OVER){
//				if(pause){
//					StartTime();
//				}
//				else{
//					StopTime();
//				}
				SwitchBag();
			}
		}

		//打开小地图(M键)
		if(Input.GetKeyDown(KeyCode.M)){
			//不在背包界面且不在系统菜单
			if(Globe.game_state != Globe.GAME_STATE.OPTION
			   &&Globe.game_state != Globe.GAME_STATE.OVER){
//				if(pause){
//					StartTime();
//					
//				}
//				else{
//					StopTime();
//				}
				SwitchMap();
			}	
		}

		//打开系统菜单
		if(Input.GetKeyDown(KeyCode.Escape)){
//			if(pause){
//				StartTime();
//			}
//			else{
//				StopTime();
//			}
			if(Globe.game_state != Globe.GAME_STATE.OVER){
				SwitchOption();
			}
		}
		
		#endregion
	}
	

	//开启关闭背包
	void SwitchBag(){
		if(SwitchBagEvent!=null){
			SwitchBagEvent();
		}
	}

	//开启关闭地图
	void SwitchMap(){
		if(SwitchMapEvent!=null){
			SwitchMapEvent();
		}
	}

	//开启关闭系统设置
	void SwitchOption(){
		if(SwitchOptionEvent!=null){
			SwitchOptionEvent();
		}
	}

	//开启游戏结束界面
	void SwitchGameOver(){
		if(switchGameOverEvent!=null){
			switchGameOverEvent();
		}
	}


	//更新HP
	void UpdateHP(){
		if(UpdateHPEvent!=null){
			UpdateHPEvent();
		}
	}

	//更新Score
	void UpdateScore(){
		if(UpdateScroeEvent!=null){
			UpdateScroeEvent();
		}
	}

	public void Dispose(){
		UpdateHPEvent = null;
		UpdateScroeEvent = null;
		SwitchBagEvent = null;
		SwitchMapEvent = null;
		SwitchOptionEvent = null;
		switchGameOverEvent =null;
		Destroy(this.gameObject);
	}

	//加载AssetBundle
	IEnumerator LoadAssetBundle(){
		string assetBundlePath = PathURL + "Abs";
		WWW mwww = WWW.LoadFromCacheOrDownload(assetBundlePath, 0);
		yield return mwww;
		if (!string.IsNullOrEmpty(mwww.error))
		{
			Debug.Log(mwww.error);
		}
		else
		{
			AssetBundle mab = mwww.assetBundle;
			AssetBundleManifest mainfest = (AssetBundleManifest)mab.LoadAsset("AssetBundleManifest");
			mab.Unload(false);
			string[] absName = mainfest.GetAllAssetBundles();
			//CreateFromFile用的url不同
			string url = Application.dataPath + "/ABs/";
//
//			TextManager.Instance.showMessage("url："+url);
			foreach(string name in absName){
				//CreateFromFile的path不加file://
				AssetBundle ab = AssetBundle.CreateFromFile(url+name);
				GameObject[] gos = ab.LoadAllAssets<GameObject>();
				Chest.itemsList.AddRange(gos);
				
				//关闭AssetBundle但没有摧毁创建的对象和引用,假
				ab.Unload(false);
			}		
		}
	}

	//游戏结束
	void IsGameOver(){
		if(Globe.game_state == Globe.GAME_STATE.OVER){
//			SwitchGameOver();
			Invoke("SwitchGameOver",2.5f);
//			Debug.Log ("GameOver");
		}
	}


	//停止时间
	public void StopTime(){
		Time.timeScale = 0f;
//		pause = true;
		Globe.game_state = Globe.GAME_STATE.STOP;
	}

	//开始时间
	public void StartTime(){
		Time.timeScale = 1f;
//		pause = false;
		Globe.game_state = Globe.GAME_STATE.RUNNING;
	}

}
