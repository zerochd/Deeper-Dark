using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //单例GameManager变量
    public static GameManager instance = null;

	public delegate void VoidDelegate();

	//HP更新
	public static event VoidDelegate UpdateHPEvent;

	//分数更新
	public static event VoidDelegate UpdateScroeEvent;

	//开启关闭背包
	public static event VoidDelegate SwitchBagEvent;

	//开启关闭地图
	public static event VoidDelegate SwitchMapEvent;

	bool pause = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);	
	
    }

	// Use this for initialization
	void Start () {
		Globe.game_state = Globe.GAME_STATE.RUNNING;
//		SwitchBag();
	}
	
	// Update is called once per frame
	void Update () {

		//更新HP
		UpdateHP();

		//更新分数
		UpdateScore();

		#region 功能辅助
		//打开背包(I键)
		if(Input.GetKeyDown(KeyCode.I)){
			//不在地图界面
			if(Globe.game_state != Globe.GAME_STATE.MAPING){
				if(pause){
					StartTime();
				}
				else{
					StopTime();
				}
				SwitchBag();
			}
		}

		//打开小地图(M键)
		if(Input.GetKeyDown(KeyCode.M)){
			//不在背包界面
			if(Globe.game_state != Globe.GAME_STATE.BAGING){
				if(pause){
					StartTime();
					
				}
				else{
					StopTime();
				}
				SwitchMap();
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

	void SwitchMap(){
		if(SwitchMapEvent!=null){
			SwitchMapEvent();
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

	//停止时间
	void StopTime(){
		Time.timeScale = 0f;
		pause = true;
		Globe.game_state = Globe.GAME_STATE.STOP;
	}

	//开始时间
	void StartTime(){
		Time.timeScale = 1f;
		pause = false;
		Globe.game_state = Globe.GAME_STATE.RUNNING;
	}

}
