using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] groups; // A list of all the group game objects with the group script attached

    public GameObject[] shadows; // A list of all the group game objects with the shadow script attached

    public static Vector3 pos; // This is the position of the bottom of the board where pieces spawn( used to spawn shadows)

    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector3(transform.position.x, 0);

        SpawnNext();
    }

    public void SpawnNext()
    {
        int num = Random.Range(0, groups.Length);
        Instantiate(groups[num], transform.position, Quaternion.identity);
        Instantiate(shadows[num], pos, Quaternion.identity);
    }
}
