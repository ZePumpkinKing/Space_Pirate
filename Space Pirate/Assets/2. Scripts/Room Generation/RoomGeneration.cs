using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    private RoomLists roomLists;

    private void Awake()
    {
        roomLists = GetComponent<RoomLists>();
    }


}
