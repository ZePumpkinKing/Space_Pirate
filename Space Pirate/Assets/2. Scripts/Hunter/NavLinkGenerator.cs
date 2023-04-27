using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavLinkGenerator : MonoBehaviour
{
    [SerializeField] NavMeshSurface[] surfaces;

    void Start()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}
