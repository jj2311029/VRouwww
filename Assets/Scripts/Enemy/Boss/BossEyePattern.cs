using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyePattern : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject eyePrefab;
    public List<Vector2> eyeSpawnPoint;
    public void SpawnEye(GameObject bossScript)
    {
        int num = Random.Range(0,eyeSpawnPoint.Count);
        Vector2 spawnPoint = eyeSpawnPoint[num];


        GameObject eye=Instantiate(eyePrefab,spawnPoint,Quaternion.identity,bossScript.transform);
        eye.tag = "Boss";
        Destroy(eye, 6f); 
    }

}
