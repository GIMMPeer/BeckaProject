using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//singleton in each room to manage and string together each event that needs to happen
//named BeckaRoomManager because RoomManager was taken up by OVR
public class BeckaRoomManager : MonoBehaviour
{
    public static BeckaRoomManager Singleton;

    //holds all tasks required to complete room
    public RoomTaskHolder[] m_AllRoomTasks;

    //holds all tasks required to complete room
    public PaintingTask[] m_AllPaintingTasks;

    //event triggered when all tasks completed
    public UnityEvent m_AllTasksCompleted;

    public bool m_AllowNonSequentialOrder = true;

    //index that keeps track of which task player is on
    private int m_RoomTaskIndex = 0;

	// Use this for initialization
	void Awake ()
    {
        Singleton = this;
	}

    private void Start()
    {
        if (m_AllRoomTasks.Length > 0) //As long as we have a task in the room, begin the task
        {
            if (m_AllowNonSequentialOrder)
            {
                foreach (RoomTaskHolder taskHolder in m_AllRoomTasks)
                {
                    taskHolder.Task.StartTask();
                }
            }

            else
            {
                m_AllRoomTasks[0].Task.StartTask();
            }

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) //Dev Cheat. Finishes painting without user manually painting. To Be removed on release -->
        {
            GameManager.m_Singleton.SetRoomPaintedStatus(true);
            RoomContainer rc = GameManager.m_Singleton.GetCurrentRoomContainer();
            GameManager.m_Singleton.SetNextRoom(rc.m_NextRoom);

        } // <-- to be removed on release
    }

    public void StartNextTask()
    {
        m_RoomTaskIndex++;

        if (AreAllRoomTasksCompleted())
        {
            Debug.Log("All tasks done");
            //move to next scene

            if (AreAllPaintingTasksCompleted())
            {
                //set that all paintings in room have been completed
                //queue up next room in gamemanager
                GameManager.m_Singleton.SetRoomPaintedStatus(true);
                RoomContainer rc = GameManager.m_Singleton.GetCurrentRoomContainer();
                GameManager.m_Singleton.SetNextRoom(rc.m_NextRoom);
            }

            m_AllTasksCompleted.Invoke();
            return;
        }

        RoomTask newRoomTask = m_AllRoomTasks[m_RoomTaskIndex].Task;

        if (m_AllRoomTasks[m_RoomTaskIndex] == null || m_AllRoomTasks.Length == 0)
        {
            Debug.LogError("RoomTask is null or length of room events is 0");
            return;
        }

        newRoomTask.StartTask();
    }

    //check if sent in task is current task in sequence
    public bool IsCurrentTask(RoomTask RoomTask)
    {
        if (m_AllowNonSequentialOrder) return true; //if order doesn't matter current task means nothing

        return m_AllRoomTasks[m_RoomTaskIndex].Task.Equals(RoomTask);
    }

    private bool AreAllRoomTasksCompleted()
    {
        if (m_RoomTaskIndex >= m_AllRoomTasks.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool AreAllPaintingTasksCompleted()
    {
        foreach (PaintingTask task in m_AllPaintingTasks)
        {
            Debug.Log("Task State: " + task.IsTaskComplete());
            //if any one task is not complete all of them are not complete
            if (!task.IsTaskComplete())
            {
                return false;
            }
        }

        return true;
    }
}

//for editor so each task can have a name, description, and task object attached to it
[System.Serializable]
public class RoomTaskHolder
{
    public string Name;
    public string Description;
    public RoomTask Task;
}
