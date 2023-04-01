using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Instantiate(Prefab, transform.position,transform.rotation);
        }
    }
}
