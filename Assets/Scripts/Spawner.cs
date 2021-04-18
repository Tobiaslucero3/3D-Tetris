using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] groups; // A list of all the group game objects with the group script attached

    // Start is called before the first frame update
    void Start()
    {
        SpawnNext();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnNext()
    {
        Instantiate(groups[Random.Range(0, groups.Length)], transform.position, Quaternion.identity);
    }
}
