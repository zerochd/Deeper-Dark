using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;	//Allows us to use Lists.
using Random = UnityEngine.Random; //Tell Random to use the Unity Engine random number generator.

public class BoardManager : MonoBehaviour {

    //允许实例化该类的对象能够在inspector中设置数值
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //房间设置为5个
    public int roomNum = 5;


    //地形分8列,8行
    public int columns = 8;
    public int row = 8;

    public GameObject door;
    public GameObject[] enemyTiles;

    private Transform boardHolder;
    private Transform[] room;
    // Use this for initialization
	void Start () {
        SetupScene(1);
        
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(RandomDoor());
	}

    //地形加载
    void BoardSetup(int level)
    {
        //新建名为Board的GameObject
        boardHolder = new GameObject("Board").transform;
        //新建roomNum长度的room数组
        room = new Transform[roomNum];
        //循环roomNum次
        for (int i = 0; i < roomNum; i++)
        {
            //新建名为Room的GameObject
            room[i] = new GameObject("Room").transform;
            //设置room的父节点为boardHolder
            room[i].SetParent(boardHolder);
        }

    }

    int RandomDoor()
    {
        return (int)(Mathf.Clamp(Random.Range(0f, 4f), 0, roomNum));
    }

    public void SetupScene(int level)
    {
        BoardSetup(level);
    }
}
