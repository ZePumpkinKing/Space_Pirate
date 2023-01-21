using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    //public int openingDir;

    private RoomLists rooms;
    public enum direction {Up, Down, Left, Right};
    // 0 --> needs bottom door
    // 1 needs top door
    // 2 needs right door
    // 3 needs left door

    public direction openingDir;
    private int rand;
    public bool spawned;

    private void Awake()
    {
        rooms = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomLists>();
    }
    private void Start()
    {
        Invoke("GenerateRooms", .1f);
    }


    private void GenerateRooms()
    {
        if (!spawned)
        {
            switch (openingDir)
            {
                case direction.Up:
                    {
                        SpawnRoom(rooms.bottomRooms);
                        break;
                    }
                case direction.Left:
                    {
                        SpawnRoom(rooms.rightRooms);
                        break;
                    }
                case direction.Right:
                    {
                        SpawnRoom(rooms.leftRooms);
                        break;
                    }
                case direction.Down:
                    {
                        SpawnRoom(rooms.topRooms);
                        break;
                    }
            }
            
        }

    }
    private void SpawnRoom(GameObject[] typeOfRoom)
    {
        rand = Random.Range(0, typeOfRoom.Length);
        Instantiate(typeOfRoom[rand], transform.position, typeOfRoom[rand].transform.rotation);
        spawned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnNode"))
        {
            Debug.Log("Destroyed");
            Destroy(gameObject);
        }
    }

}
