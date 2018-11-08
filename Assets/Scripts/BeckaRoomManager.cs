﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton in each room to manage and string together each event that needs to happen
public class BeckaRoomManager : MonoBehaviour
{
    public static BeckaRoomManager Singleton;

    //TODO find a way to make tasks more easy to read, currently they are just the name of the objects the tasks are attached to
    public RoomTask[] m_AllRoomTasks;

    private int m_RoomTaskIndex = 0;
	// Use this for initialization
	void Awake ()
    {
        Singleton = this;
	}

    private void Start()
    {
        m_AllRoomTasks[0].StartTask();
    }

    public void StartNextEvent()
    {
        m_RoomTaskIndex++;

        Debug.Log("TaskNumber: " + m_RoomTaskIndex);
        if (AllRoomTasksCompleted())
        {
            Debug.Log("All tasks done");
            //move to next scene
            return;
        }

        RoomTask newRoomTask = m_AllRoomTasks[m_RoomTaskIndex];

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
        return m_AllRoomTasks[m_RoomTaskIndex].Equals(RoomTask);
    }

    private bool AllRoomTasksCompleted()
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
}
