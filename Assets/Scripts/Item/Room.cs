using UnityEngine;
using System.Collections;

public class Room{

    Vector2 leftBottomVector;                   //记录Room的左下坐标
    Vector2 openDoorVector;                     //记录下一个打开房间的坐标
    public int enemy;                                  //敌人数
    public int boss;                                   //boss数量

    private int roomID;
    public Room[] nextRoom;                       //房间数组放上下左右四个房间的句柄
    public bool[] door;                           //表示四个方向的门是否存在

    //不带参数的初始化
    public Room()
    {
        nextRoom = new Room[4];
        door = new bool[4];
        this.leftBottomVector = Vector2.zero;
        roomID = 0;       
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
    public void setDoorVector(Vector2 _openDoorVector)
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

    public void setRoomID(int _roomID)
    {
        this.roomID = _roomID;
    }

    //随机生成下一个房间
    /*步骤：1.判断开门的方向，如果该方向的门还没开，如果真则进入步骤2,如果假则进入步骤4
     * 2.将该方向门bool设置真，实例化一个新的房间，并设房间坐标，跟所开的门的坐标,并将新的房间的反方向的门bool设置真，进入步骤3
     * 3.设置下一个房间的敌人配置策略,返回下一个房间
     * 4.返回null
    */
    public Room getRandomRoomNext(Room currentRoom, int doorWhich,int roomID)
    {

        //生成上方向房间 
        if (!door[(int)Direction.up] && doorWhich == (int)Direction.up)
        {
            //Debug.Log("生成上房间");
            door[(int)Direction.up] = true;
            //currentRoom.upRoom = new Room(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() + BoardManager.ROWS-1));
            //currentRoom.upRoom.setDoorVector(new Vector2(currentRoom.getPointX()+ BoardManager.COLUMNS/2, currentRoom.getPointY() + BoardManager.ROWS-1));
            //currentRoom.upRoom.door[(int)Direction.down] = true;
            currentRoom.nextRoom[doorWhich] = new Room(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() + BoardManager.ROWS - 1));
            currentRoom.nextRoom[doorWhich].setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.COLUMNS / 2, currentRoom.getPointY() + BoardManager.ROWS - 1));
            currentRoom.nextRoom[doorWhich].door[(int)Direction.down] = true;
            return currentRoom.nextRoom[doorWhich];         
        }

        //生成右方向房间
        if (!door[(int)Direction.right] && doorWhich == (int)Direction.right)
        {
            //Debug.Log("生成右房间");
            door[(int)Direction.right] = true;
            //currentRoom.rightRoom = new Room(new Vector2(currentRoom.getPointX() + BoardManager.COLUMNS-1, currentRoom.getPointY()));
            //currentRoom.rightRoom.setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.COLUMNS-1, currentRoom.getPointY() + BoardManager.ROWS /2));
            //currentRoom.rightRoom.door[(int)Direction.left] = true;
            currentRoom.nextRoom[doorWhich] = new Room(new Vector2(currentRoom.getPointX() + BoardManager.COLUMNS - 1, currentRoom.getPointY()));
            currentRoom.nextRoom[doorWhich].setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.COLUMNS - 1, currentRoom.getPointY() + BoardManager.ROWS / 2));
            currentRoom.nextRoom[doorWhich].door[(int)Direction.left] = true;
            return currentRoom.nextRoom[doorWhich];
        }

        //生成下方向房间
        if (!door[(int)Direction.down] && doorWhich == (int)Direction.down)
        {
            //Debug.Log("生成下房间");
            door[(int)Direction.down] = true;
            //currentRoom.downRoom = new Room(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() - BoardManager.ROWS+1));
            //currentRoom.downRoom.setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.COLUMNS/2, currentRoom.getPointY()));
            //currentRoom.downRoom.door[(int)Direction.up] = true;
            currentRoom.nextRoom[doorWhich] = new Room(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() - BoardManager.ROWS + 1));
            currentRoom.nextRoom[doorWhich].setDoorVector(new Vector2(currentRoom.getPointX() + BoardManager.COLUMNS / 2, currentRoom.getPointY()));
            currentRoom.nextRoom[doorWhich].door[(int)Direction.up] = true;
            return currentRoom.nextRoom[doorWhich];
        }
        
        //生成左方向房间
        if (!door[(int)Direction.left] && doorWhich == (int)Direction.left)
        {
            //Debug.Log("生成左房间");
            door[(int)Direction.left] = true;
            //currentRoom.leftRoom = new Room(new Vector2(currentRoom.getPointX() - BoardManager.COLUMNS +1, currentRoom.getPointY()));
            //currentRoom.leftRoom.setDoorVector(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() + BoardManager.ROWS /2));
            //currentRoom.leftRoom.door[(int)Direction.right] = true;

            currentRoom.nextRoom[doorWhich] = new Room(new Vector2(currentRoom.getPointX() - BoardManager.COLUMNS + 1, currentRoom.getPointY()));
            currentRoom.nextRoom[doorWhich].setDoorVector(new Vector2(currentRoom.getPointX(), currentRoom.getPointY() + BoardManager.ROWS / 2));
            currentRoom.nextRoom[doorWhich].door[(int)Direction.right] = true;
            return currentRoom.nextRoom[doorWhich];
        }
        //如果没有返回空
        return null;
    }

    //设置下一个房间的敌人数量策略
    //当房间ID等于某个关联最大房间数的值时，添加boss
    //反之添加敌人
    public void setRoomNextEnemyNum(Room nextRoom,int roomMaxNum)
    {
        Debug.Log("roomID: " +nextRoom.roomID);
        
        if (nextRoom.roomID == roomMaxNum / 2 + 1)
        {
            nextRoom.boss = 1;
        }
        else
        {
            nextRoom.enemy = UnityEngine.Random.Range(1, 5);
            nextRoom.boss = 0;
        }
    }
}
