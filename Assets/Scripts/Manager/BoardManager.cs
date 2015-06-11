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



    //房间设置为7个
    public int roomNum = 7;


    //地形分8列,8行
    public static int columns = 13;
    public static int rows = 7;

    //地形素材
    public GameObject wall;
    public GameObject floor;
    public GameObject door;
    
    //敌人
    public GameObject[] enemies;

    //Boss
    public GameObject Boss;

    //Transform句柄
    private Transform boardHolder;
    private Transform roomHolder;

    //房间的根节点
    private Room roomStart;
    //房间的指针节点
    private Room roomNext;

    private Dictionary<Vector2,Room> roomPosition;
    // Use this for initialization
	void Start () {
        SetupScene(1);
        
	}
	
	// Update is called once per frame
	void Update () {
 //       Debug.Log(RandomDoor());
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
        //新建名为Room的GameObject
        boardHolder = new GameObject("Room").transform;
        //创建第一个房间的引用
        roomStart = new Room(Vector2.zero);
        //随机数种子
        int seed = 15;
        //生成随机数
        System.Random ro = new System.Random(seed);
   //   int resultRandom = ro.Next(seed);
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
 
        //对随机数进行对4去模
        int doorWhich = ro.Next(seed) % 4;
        //布置房间
        InstanceRoom(parentRoom, room);

        //布置敌人
        InstanceEnemy(parentRoom, room, roomNum);

        //如果有打开的门
        while ( doorOpen > 0 && roomNum > 0 )
        {
            //进行选择门
            doorWhich = (doorWhich + 1) % 4;

            //Debug.Log("choose door:"+doorWhich);

            //指向下一个房间
            roomNext = room.getRandomRoomNext(room, doorWhich);
            
            //如果还能开门
            if (roomNext != null)
            {
                //实例化门
                InstanceDoor(parentRoom, roomNext);
                //如果已经存在房间将两个房间链接起来而不是建立新房间
                if (roomPosition.ContainsKey(roomNext.getVector()))
                {
                    //一个房间的上方向门是下一个房间的下方向门,链接两个房间的指针
                    int preDoor = (doorWhich + 2) % 4;
                    //Debug.LogWarning("preDoor:"+preDoor);
                    roomPosition[roomNext.getVector()].nextRoom[preDoor] = room;
                    roomNext = roomPosition[roomNext.getVector()];
                    break;
                }
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

    void InstanceEnemy(Transform parenteRoom, Room room,int roomNum)
    {
        //Debug.Log("I am Create Enemy");
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


//房间类
class Room
{
    Vector2 leftBottomVector;                   //记录Room的左下坐标
    Vector2 openDoorVector;                     //记录下一个打开房间的坐标
    int enemy;                                  //敌人数
    int boss;                                   //boss数量
    //public Room upRoom = null;                  //上房间
    //public Room rightRoom = null;               //右房间
    //public Room downRoom = null;                //下房间
    //public Room leftRoom = null;                //左房间
    public Room[] nextRoom;
    public bool[] door;                         //表示四个方向的门是否存在

    //不带参数的初始化
    public Room()
    {
        nextRoom = new Room[4];
        door = new bool[4];
        this.leftBottomVector = Vector2.zero;                  
    }

    //带坐标的初始化左下坐标
    public Room(Vector2 _leftBottomVector)
    {
        nextRoom = new Room[4];
        door = new bool[4];
        setVector(_leftBottomVector);
    }

    //得到房间坐标
    public Vector2 getVector()
    {
        return leftBottomVector;
    }

    //设置房间坐标
    public void setVector(Vector2 _leftBottomVector)
    {
        this.leftBottomVector = _leftBottomVector;
    }

    //得到通向下一个房间门的坐标
    public Vector2 getDoorVector()
    {
        return openDoorVector;
    }

    //设置通向下一个房间门的坐标
    public void setDoorVector(Vector2 _openDoorVector )
    {
        this.openDoorVector = _openDoorVector;
    }

    //获得横坐标
    public int getPointX()
    {
        return (int)leftBottomVector.x;
    }

    //获得纵坐标
    public int getPointY()
    {
        return (int)leftBottomVector.y;
    }





    //随机生成下一个房间
    public Room getRandomRoomNext(Room currentRoom, int doorWhich)
    {

        //生成上方向房间 
        if (!door[(int)Direction.up] && doorWhich == (int)Direction.up)
        {
            //Debug.Log("生成上房间");
            door[(int)Direction.up] = true;
            //currentRoom.upRoom = new Room(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() + BoardManager.rows-1));
            //currentRoom.upRoom.setDoorVector(new Vector2(currentRoom.getPointX()+ BoardManager.columns/2, currentRoom.getPointY() + BoardManager.rows-1));
            //currentRoom.upRoom.door[(int)Direction.down] = true;
            currentRoom.nextRoom[doorWhich] = new Room(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() + BoardManager.rows - 1));
            currentRoom.nextRoom[doorWhich].setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.columns / 2, currentRoom.getPointY() + BoardManager.rows - 1));
            currentRoom.nextRoom[doorWhich].door[(int)Direction.down] = true;
            return currentRoom.nextRoom[doorWhich];         
        }

        //生成右方向房间
        if (!door[(int)Direction.right] && doorWhich == (int)Direction.right)
        {
            //Debug.Log("生成右房间");
            door[(int)Direction.right] = true;
            //currentRoom.rightRoom = new Room(new Vector2(currentRoom.getPointX() + BoardManager.columns-1, currentRoom.getPointY()));
            //currentRoom.rightRoom.setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.columns-1, currentRoom.getPointY() + BoardManager.rows /2));
            //currentRoom.rightRoom.door[(int)Direction.left] = true;
            currentRoom.nextRoom[doorWhich] = new Room(new Vector2(currentRoom.getPointX() + BoardManager.columns - 1, currentRoom.getPointY()));
            currentRoom.nextRoom[doorWhich].setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.columns - 1, currentRoom.getPointY() + BoardManager.rows / 2));
            currentRoom.nextRoom[doorWhich].door[(int)Direction.left] = true;
            return currentRoom.nextRoom[doorWhich];
        }

        //生成下方向房间
        if (!door[(int)Direction.down] && doorWhich == (int)Direction.down)
        {
            //Debug.Log("生成下房间");
            door[(int)Direction.down] = true;
            //currentRoom.downRoom = new Room(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() - BoardManager.rows+1));
            //currentRoom.downRoom.setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.columns/2, currentRoom.getPointY()));
            //currentRoom.downRoom.door[(int)Direction.up] = true;
            currentRoom.nextRoom[doorWhich] = new Room(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() - BoardManager.rows + 1));
            currentRoom.nextRoom[doorWhich].setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.columns / 2, currentRoom.getPointY()));
            currentRoom.nextRoom[doorWhich].door[(int)Direction.up] = true;
            return currentRoom.nextRoom[doorWhich];
        }
        
        //生成左方向房间
        if (!door[(int)Direction.left] && doorWhich == (int)Direction.left)
        {
            //Debug.Log("生成左房间");
            door[(int)Direction.left] = true;
            //currentRoom.leftRoom = new Room(new Vector2(currentRoom.getPointX() - BoardManager.columns +1, currentRoom.getPointY()));
            //currentRoom.leftRoom.setDoorVector(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() + BoardManager.rows /2));
            //currentRoom.leftRoom.door[(int)Direction.right] = true;
            currentRoom.nextRoom[doorWhich] = new Room(new Vector2(currentRoom.getPointX() - BoardManager.columns + 1, currentRoom.getPointY()));
            currentRoom.nextRoom[doorWhich].setDoorVector(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() + BoardManager.rows / 2));
            currentRoom.nextRoom[doorWhich].door[(int)Direction.right] = true;
            return currentRoom.nextRoom[doorWhich];
        }
        //如果没有返回空
        return null;
    }

}


