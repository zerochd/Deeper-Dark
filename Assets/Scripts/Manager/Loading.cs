using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

	//注意这里返回值一定是 IEnumerator
    AsyncOperation async;
    public Text loading_text;
    //读取场景的进度
    int progress = 0;

    void Awake()
    {
        loading_text = GameObject.Find("Loading").GetComponent<Text>();
    }

    void Start()
    {
        //开启协同
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene(){

        //异步读取场景。
        //Globe.loadName 就是A场景中需要读取的C场景名称。
        async = Application.LoadLevelAsync(Globe.sceneName);
        //读取完毕后返回， 系统会自动进入C场景
        yield return async;
    }


    void Update()
    {
        progress = (int)(async.progress * 100);
        loading_text.text = "loading:" + progress + "%";
        Debug.Log("progress:" + progress);

    }
}
