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

    public static bool isCreate = false;                    //设置房间没有创建                 

    //地形分13列,7行
    public static int COLUMNS = 13;
    public static int ROWS = 7;

    //地形素材
    public GameObject wall;
    public GameObject floor;
    public GameObject door;
    
    //敌人类型
    public GameObject[] enemies;
    //Boss类型
    public GameObject Boss;
	//道具类型
	public GameObject[] chests;
	//陷阱类型
	public GameObject[] traps;
	//云类型
	public GameObject cloud;

    private Transform boardHolder;                      //board的Transform句柄
    private Transform roomHolder;                       //room的Transform句柄
    private Transform enemyHolder;                      //enemy的Transform句柄
	private Transform ChestHolder;						//item的Transform句柄
	private Transform trapHolder;						//trap的Transform句柄
	private Transform cloudHolder;						//cloud的Transorm句柄

                                          
    private Room roomStart;                             //房间的根节点
    private Room roomNext;                              //下一个房间节点
    private Dictionary<Vector2,Room> roomPosition;
    private int roomID=0;                                //房间编号
    private int roomNum;                                 //设置房间数
		
	public  static Vector3 xy_min = Vector3.zero;								 //最小的xy3维								
	public  static Vector3 xy_max = new Vector3(COLUMNS - 1,ROWS - 1,-10);		 //最大的xy3维

	private List<Vector3> gridPositions = new List<Vector3>();	//保存房间格子数(放置物)


	delegate void InstanceSomethingDelegate(Transform parentRoom, Room room);
	event InstanceSomethingDelegate instanceSomethingEvent;

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
    void InitialiseRoomPositionList()
    {
        roomPosition = new Dictionary<Vector2, Room>();
        roomPosition.Clear();
    }

	//初始化可放置格子，排除掉靠近门的四个格子
	void InitialiseGridPositionsList()
	{
		gridPositions.Clear();
		for (int x = 2; x < COLUMNS - 2; x++) 
		{
			for(int y = 2; y < ROWS - 2;y++){
				gridPositions.Add(new Vector3(x,y,0f));
			}
		}
	}


    //地形加载
    void BoardSetup(int level)
    {
        roomNum = roomMaxNum;
        //新建名为Room的GameObject
        boardHolder = new GameObject("Room").transform;
        //新建名为Enemy的GameObject
        enemyHolder = new GameObject("Enemy").transform;
		//新建名为Item的GameObject
		ChestHolder = new GameObject("Chest").transform;
		//新建名为Trap的GameObject
		trapHolder = new GameObject("Trap").transform;
		//新建名为cloud的GameObject
		cloudHolder = new GameObject("Cloud").transform;

		//布置一些东西里添加(房间,敌人,物件)
		instanceSomethingEvent += InstanceEnemy;
		instanceSomethingEvent += InstanceObejct;
		instanceSomethingEvent += InstanceRoom;

        //创建第一个房间的引用
        roomStart = new Room();
        //生成随机数
        System.Random ro = new System.Random(seed);
        //int resultRandom = ro.Next(seed);
        //初始化房间列表
        InitialiseRoomPositionList();
        //初始化减1房间
        roomNum--;
        //开始创建随机房间,并人为的给第一个房间的开门数为3
        CreateRandomRoom(boardHolder, roomStart, seed, ro, 3);

//		Debug.Log ("min_xy:"+xy_min +"max_xy"+xy_max);

		//初始化mapCamera的x,y值跟size值
		Vector3 boardCenter = (BoardManager.xy_max + BoardManager.xy_min) / 2 - Vector3.forward * 5;
		int size_x = (int)((BoardManager.xy_max.x-BoardManager.xy_min.x)/(BoardManager.COLUMNS - 1));
		int size_y = (int)((BoardManager.xy_max.y-BoardManager.xy_min.y)/(BoardManager.ROWS - 1));
		//3.2f是main_Camera的size值
		float size_final = (size_x > size_y ? size_x  : size_y) * 3.2f;
		Camera mapCamera = GameObject.Find("MapCamera").GetComponent<Camera>();
		mapCamera.orthographicSize = size_final;
		mapCamera.transform.position = boardCenter;
		mapCamera.enabled = false;

        isCreate = true;
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
        
		//求出整个board最小跟最大的两个Vector2
		if(xy_min.x >= room.getPointX()){
			xy_min.x = room.getPointX();
		}
		else{
			xy_max.x = room.getPointX() + COLUMNS -1;
		}

		if(xy_min.y >= room.getPointY()){
			xy_min.y = room.getPointY();
		}
		else{
			xy_max.y = room.getPointY() + ROWS - 1;
		}

		//将坐标跟房间建立联系
        roomPosition.Add(new Vector2(room.getPointX(),room.getPointY()),room);
 
        //用随机数对4去模，取得初始选择的门
        int doorWhich = ro.Next(seed) % 4;
	
		//初始化gridPositions
		InitialiseGridPositionsList();

		//布置一些东西
		InstanceSomething(parentRoom,room);

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
				roomNext.setRoomNextObjectNum(roomNext, roomMaxNum);

                //房间数跟开门数-1
                roomNum--;
                doorOpen--;
                //随机下一个开门数并限制在房间数内,设置最小值为1,避免出现下一个不开门的情况，
                //设置最大值为roomNum，避免出现多开门的情况
                int nextDoorOpen = Mathf.Clamp(ro.Next(seed + doorOpen)%3,1,roomNum);
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

	//事件调用
	void InstanceSomething(Transform parentRoom, Room room)
	{
		if(instanceSomethingEvent != null ){
			instanceSomethingEvent(parentRoom,room);
		}
	}


    //实例化房间细节
    void InstanceRoom(Transform parentRoom, Room room)
    {
        for (int x = room.getPointX(); x < room.getPointX() + COLUMNS; x++)
        {
            for (int y = room.getPointY(); y < room.getPointY() + ROWS; y++)
            {
                //先铺地板
                GameObject toInstantiate = floor;


                //给房间边缘边缘铺墙
                if (x == room.getPointX() || x == room.getPointX() + COLUMNS - 1 || y == room.getPointY() || y == room.getPointY() + ROWS - 1)
                {

                    toInstantiate = wall;
                }
                //实例化toInstantiate在x,y坐标处并转为GameObject类型
                GameObject instance =
                        Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //设置父节点为Room
                instance.transform.SetParent(parentRoom);

				if(cloud == null)
					continue;

				//生成房间上方的云
				if( x == room.getPointX() + (COLUMNS - 1)/2 && y == room.getPointY() + (ROWS - 1)/2)
				{
					GameObject cloudInstance = Instantiate(cloud,new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
					cloudInstance.transform.SetParent(cloudHolder);
				}

            }//for (int y = room.getPointY(); y < room.getPointY() + ROWS; y++)
        }//for (int x = room.getPointX(); x < room.getPointX() + COLUMNS; x++)
    }

	//实例化敌人
    void InstanceEnemy(Transform parenteRoom,Room room)
    {
        while (room.enemy > 0)
        {
			Vector3 randomGrid = RandomPosition();
			Vector3 enemySpawnPosition = new Vector3(room.getPointX() + randomGrid.x, room.getPointY() + randomGrid.y, 0);
            GameObject enemyInstance = Instantiate(GetRandomEnemy(), enemySpawnPosition, Quaternion.identity) as GameObject;
            enemyInstance.transform.SetParent(enemyHolder);
            //限制循环
            room.enemy--;
        }
        while (room.boss > 0)
        {
            Vector3 bossSpawnPosition = new Vector3(room.getPointX() + COLUMNS/2, room.getPointY() + ROWS/2, 0);
            GameObject bossInstance = Instantiate(Boss, bossSpawnPosition, Quaternion.identity) as GameObject;
            bossInstance.transform.SetParent(enemyHolder);
            room.boss--;

        }
        //Debug.Log("I am Create Enemy");
    }

	void InstanceObejct(Transform parenteRoom,Room room)
	{
		while(room.chest > 0){
			Vector3 randomGrid = RandomPosition();
			Vector3 chestSpawnPosition = new Vector3(room.getPointX() + randomGrid.x, room.getPointY() + randomGrid.y, 0);
			GameObject itemInstance = Instantiate(GetRandomChest(), chestSpawnPosition, Quaternion.identity) as GameObject;
			itemInstance.transform.SetParent(ChestHolder);
			room.chest--;
		}
		while(room.trap > 0){
			Vector3 randomGrid = RandomPosition();
			Vector3 trapSpawnPosition = new Vector3(room.getPointX() + randomGrid.x, room.getPointY() + randomGrid.y, 0);
			GameObject trapInstance = Instantiate(GetRandomTrap(), trapSpawnPosition, Quaternion.identity) as GameObject;
			trapInstance.transform.SetParent(trapHolder);
			room.trap--;
		}
	}


    //获得随机敌人
    GameObject GetRandomEnemy()
    {
		if(enemies.Length == 0)
			Debug.LogError("no enemy");
        GameObject enemy = enemies[UnityEngine.Random.Range(0, enemies.Length)];
        return enemy;
    }

	//获得随机道具
	GameObject GetRandomChest()
	{
		if(chests.Length == 0)
			Debug.LogError("no chest");
		GameObject item = chests[UnityEngine.Random.Range(0, chests.Length)];
		return item;
	}

	//获得随机陷阱
	GameObject GetRandomTrap()
	{
		if(traps.Length == 0)
			Debug.LogError("no trap");
		GameObject trap = traps[UnityEngine.Random.Range(0, traps.Length)];
		return trap;
	}



	//随机gridPosition中的位置
	Vector3 RandomPosition()
	{
		int randomIndex = UnityEngine.Random.Range (0,gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

    //实例化门
    void InstanceDoor(Transform parentRoom, Room room)
    {

   //   Debug.Log("roomNextDoor Vector2:"+room.getDoorVector());
        GameObject toInstantiate = door;
   
        GameObject instance = Instantiate(toInstantiate, new Vector3(room.getDoorVector().x, room.getDoorVector().y, 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(parentRoom);
        
        //如果门在左侧或者右侧则旋转门的方向
        if(room.getDoorVector().x==room.getPointX()||room.getDoorVector().x==room.getPointX() + BoardManager.COLUMNS - 1){
            instance.transform.Rotate(new Vector3(0,0,90f));
        }
    }

    //场景建立
    public void SetupScene(int level)
    {
        BoardSetup(level);

		TextManager.Instance.showTint("未知洞穴");

    }

   
}





