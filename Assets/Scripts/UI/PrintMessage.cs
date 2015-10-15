using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PrintMessage : MonoBehaviour {

    //通过添加该组件至Text组件实现固定高度(MAX_ROW_COUNT)滚动显示文本


    private ArrayList messages = new ArrayList();
    private bool isPrinting = false;
    private Text subScreenGUIText;              //获得UI的Text组件
    const int MAX_ROW_COUNT = 6;                //总行数


    private static int ADDITION_NUM = 1;        //一次显示的文字数
    private static string CURSOR_STR = "_";     //末尾的文字

	// Use this for initialization
	void Start () {
        subScreenGUIText = GetComponent<Text>();
        //Debug.Log(subScreenGUIText.text);

//        subScreenGUIText.text = "\n\n\n\n\n\n";

        //初始化Message
//        SetMessage("STAND BY ALERT.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");
//        SetMessage("ENEMY FLEETS ARE APPROACHING.");

	}
	
	// Update is called once per frame
	void Update () {
        //判断messages里面是否有值
        if (messages.Count > 0)
        {
            //判断是否有在输入
            if (!isPrinting)
            {
                isPrinting = true;
                //出队列
                string tmp = messages[0] as string;
                messages.RemoveAt(0);
                //开始打字
                StartCoroutine(PlayMessage(tmp));
            }
        }

	}

    //添加信息（先进先出）
    public void SetMessage(string message)
    {
        //进队列
        messages.Add(message);
    }

    //开始打字
    IEnumerator PlayMessage(string message)
    {
        char[] charactors = new char[256];

        //如果单个信息长度大于255的话，舍弃之后的数据
        if (message.Length > 255)
        {
            message = message.Substring(0, 254);
        }

        //将数据转储为字符数组
        charactors = message.ToCharArray();

        //获得屏幕上的当前文字
        string subScreenText = subScreenGUIText.text;

		//临时删除
//        subScreenText += "\n";

        //一次表示的文字=固定值加行数
        int adddtionNum = ADDITION_NUM + messages.Count;

        for (int i = 0; i < charactors.Length; i += adddtionNum)
        {
            //删除末尾的光标字符
            if (subScreenText.EndsWith(CURSOR_STR))
            {
                subScreenText = subScreenText.Remove(subScreenText.Length - 1);
            }

            for (int j = 0; j < adddtionNum; j++)
            {
                if (i + j >= charactors.Length)
                {
                    break;
                }
                subScreenText += charactors[i + j];
            }

            //追加末尾光标
            subScreenText += CURSOR_STR;

            //根据"\n"分割字符串
            string[] lines = subScreenText.Split("\n"[0]);
            for (int m = 0; m < lines.Length; m++)
            {
//                Debug.Log(i+"i:"+lines[m]);
            }

//            Debug.Log("line.Legth:" + lines.Length);

            //当超过MAX_ROW_COUNT，文字向上滚动
            if (lines.Length > MAX_ROW_COUNT)
            {
                subScreenText = "";
                for (int j = lines.Length - MAX_ROW_COUNT; j < lines.Length; j++)
                {
                    subScreenText += lines[j];
                    //添加换行
                    if (j < lines.Length - 1)
                    {
                        subScreenText += "\n";
                    }
                }
            }
            subScreenGUIText.text = subScreenText;

            //等待0.001f
            yield return new WaitForSeconds(0.001f);
        }
        //将最后光标去掉
        if (subScreenText.EndsWith(CURSOR_STR))
        {

            subScreenText = subScreenText.Remove(subScreenText.Length - 1);

            subScreenGUIText.text = subScreenText;
        }

        //表示处理结束
        isPrinting = false;
    }
}
