using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;	//Allows us to use Lists.


//枚举值用来表示上右下左
enum Direction : int
{
    up      = 0, 
    right   = 1, 
    down    = 2, 
    left    = 3,
};

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
                                     
    public int roomMaxNum = 10;                             //房间总数
    public int seed = 25;                                   //生成随机房间的随机数种子

    //地形分13列,7行
    public static int columns = 13;
    public static int rows = 7;

    //地形素材
    public GameObject wall;
    public GameObject floor;
    public GameObject door;
    
    //敌人类型
    public GameObject[] enemies;
    //Boss类型
    public GameObject Boss;

    private Transform boardHolder;                      //Transform句柄
    private Transform roomHolder;                       //Transform句柄
    private Transform enemyHolder;                      //Transform句柄
                                          
    private Room roomStart;                             //房间的根节点
    private Room roomNext;                              //下一个房间节点
    private Dictionary<Vector2,Room> roomPosition;
    private int roomID=0;                                 //房间编号
    private int roomNum;                                 //设置房间数
    // Use this for initialization
	void Start () {
        SetupScene(1);
        
	}
	
	// Update is called once per frame
	void Update () {
 //       Debug.Log(RandomDoor());
	}

    //返回第一个房间的句柄
    public Room getRoomFirst()
    {
        return roomStart;
    }

    //初始化房间列表

    void InitialiseList()
    {
        roomPosition = new Dictionary<Vector2, Room>();
        roomPosition.Clear();
    }

    //地形加载
    void BoardSetup(int level)
    {
        roomNum = roomMaxNum;
        //新建名为Room的GameObject
        boardHolder = new GameObject("Room").transform;
        //新建名为Enemy的GameObject
        enemyHolder = new GameObject("Enemy").transform;
        //创建第一个房间的引用
        roomStart = new Room();
        //生成随机数
        System.Random ro = new System.Random(seed);
        //int resultRandom = ro.Next(seed);
        //初始化房间列表
        InitialiseList();
        //初始化减1房间
        roomNum--;
        //开始创建随机房间,并人为的给第一个房间的开门数为3
        CreateRandomRoom(boardHolder, roomStart, seed, ro, 3);    
    }

    //创建随机房间
    /*方法原理:通过采用递归的方式来建立随机房间，先绘制房间，然后用开门数来限制循环，
     *在循环中按照顺时针的方向来选择下一个房间的方向，同时判断该方向的下一个房间是否
     *已经存在，如果已经存在就将两个房间的节点链接起来，并跳出循环,如果不存在，则房间
     *数跟开门数-1，同时进入下一个房间建立随机房间。
    */
    void CreateRandomRoom(Transform parentRoom, Room room, int seed,System.Random ro,int doorOpen)
    {
        
        //Debug.Log("RoomNum:" + roomNum);
        //Debug.Log("I have doorOpen:" + doorOpen);
        //Debug.Log("room Vector2:(" + room.getPointX() + " " + room.getPointY() + ")");
        //将坐标跟房间建立联系
        roomPosition.Add(new Vector2(room.getPointX(),room.getPointY()),room);
 
        //用随机数对4去模，取得初始选择的门
        int doorWhich = ro.Next(seed) % 4;

        //布置房间
        InstanceRoom(parentRoom, room);

  //      Debug.Log("Enemy: "+room.enemy);
        //布置敌人，通过将种子跟roomNum相加来修改seed的值(roomNum每次循环-1)
        InstanceEnemy(parentRoom,room,seed+roomNum);

        //如果有打开的门
        while ( doorOpen > 0 && roomNum > 0 )
        {
            //进行选择门
            doorWhich = (doorWhich + 1) % 4;

            //Debug.Log("choose door:"+doorWhich);

            //指向下一个房间
            roomNext = room.getRandomRoomNext(room, doorWhich,roomID);
            
            //如果还能开门
            if (roomNext != null)
            {
                //实例化门
                InstanceDoor(parentRoom, roomNext);
                //如果已经存在房间将两个房间链接起来而不是建立新房间
                if (roomPosition.ContainsKey(roomNext.getVector()))
                {
                    Debug.LogWarning("房间重叠");
                    //一个房间的上方向门是下一个房间的下方向门,链接两个房间的句柄
                    int preDoor = (doorWhich + 2) % 4;
                    //Debug.LogWarning("preDoor:"+preDoor);
                    roomPosition[roomNext.getVector()].nextRoom[preDoor] = room;
                    roomNext = roomPosition[roomNext.getVector()];
                    break;
                }
                roomID++;

                roomNext.setRoomID(roomID);
                roomNext.setRoomNextEnemyNum(roomNext, roomMaxNum);
                //房间数跟开门数-1
                roomNum--;
                doorOpen--;
                //随机下一个开门数并限制在房间数内
                int nextDoorOpen = Mathf.Clamp(ro.Next(seed + doorOpen)%3,0,roomNum);
                //创建Room链接父子节点
                roomHolder = new GameObject("Room").transform;
                roomHolder.SetParent(parentRoom);
                
                //继续创建随机房间
                CreateRandomRoom(roomHolder, roomNext, seed, ro, nextDoorOpen);
                
            }
            //else
            //{
            //    Debug.Log("Try again");
            //}

        }//while ( doorOpen > 0 && roomNum > 0 )
 
    }

    //实例化房间细节
    void InstanceRoom(Transform parentRoom, Room room)
    {
        for (int x = room.getPointX(); x < room.getPointX() + columns; x++)
        {
            for (int y = room.getPointY(); y < room.getPointY() + rows; y++)
            {
                //先铺地板
                GameObject toInstantiate = floor;

                //给房间边缘边缘铺墙
                if (x == room.getPointX() || x == room.getPointX() + columns - 1 || y == room.getPointY() || y == room.getPointY() + rows - 1)
                {
                        toInstantiate = wall;
                }
                //实例化toInstantiate在x,y坐标处并转为GameObject类型
                GameObject instance =
                        Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;


                //设置父节点为Room
                instance.transform.SetParent(parentRoom);

            }//for (int y = room.getPointY(); y < room.getPointY() + rows; y++)
        }//for (int x = room.getPointX(); x < room.getPointX() + columns; x++)
    }

    void InstanceEnemy(Transform parenteRoom,Room room,int seed)
    {
        System.Random random = new System.Random(seed);
       
        while (room.enemy > 0)
        {
            int xRandom = random.Next(columns-4) + 2;
            int yRandom = random.Next(rows-4) + 2;
            //Debug.Log("x :" + xRandom + " y: " + yRandom);
            Vector3 enemySpawnPosition = new Vector3(room.getPointX() + xRandom, room.getPointY() + yRandom, 0);
            GameObject enemyInstance = Instantiate(getRandomEnemy(), enemySpawnPosition, Quaternion.identity) as GameObject;
            //初始化为不活动
        //    enemyInstance.SetActive(false);
            enemyInstance.transform.SetParent(enemyHolder);
            //限制循环
            room.enemy--;
        }
        while (room.boss > 0)
        {
            Vector3 bossSpawnPosition = new Vector3(room.getPointX() + columns/2, room.getPointY() + rows/2, 0);
            GameObject bossInstance = Instantiate(Boss, bossSpawnPosition, Quaternion.identity) as GameObject;
            bossInstance.transform.SetParent(enemyHolder);
            room.boss--;

        }
        //Debug.Log("I am Create Enemy");
    }

    //获得随机敌人
    GameObject getRandomEnemy()
    {
        GameObject enemy = enemies[UnityEngine.Random.Range(0, enemies.Length)];
        return enemy;
    }


    //实例化门
    void InstanceDoor(Transform parentRoom, Room room)
    {

   //   Debug.Log("roomNextDoor Vector2:"+room.getDoorVector());
        GameObject toInstantiate = door;
   
        GameObject instance = Instantiate(toInstantiate, new Vector3(room.getDoorVector().x, room.getDoorVector().y, 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(parentRoom);
        
        //如果门在左侧或者右侧则旋转门的方向
        if(room.getDoorVector().x==room.getPointX()||room.getDoorVector().x==room.getPointX() + BoardManager.columns - 1){
            instance.transform.Rotate(new Vector3(0,0,90f));
        }
    }

    //场景建立
    public void SetupScene(int level)
    {
        BoardSetup(level);
    }

   
}





