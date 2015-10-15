
//事件行为基类
abstract class EventActor{

	public virtual void start(EventManager evman)
	{	
	}

	public virtual void execute(EventManager evman)
	{
	}

	public virtual void onGUI(EventManager evman)
	{
	}

	public virtual bool isDone()
	{
		return true;
	}

	public virtual bool isWaitClick(EventManager evman)
	{
		return true;
	}

}