using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    List<GameObject> chunks;
    void Start()
    {
        // for(int i = 0; i < 1; i++) {

        // }

        for(int x = 0; x < 3; x++){
            for(int z = 0; z < 3; z++) {
                GameObject chunk = Instantiate(chunkPrefab, new Vector3(x * (GridMetrics.PointsPerChunk - 1), 0, z * (GridMetrics.PointsPerChunk - 1)), Quaternion.identity, this.transform);
            }
        }
        //chunk.refernceNoiseGenerator(noiseGenerator);
        // chunk.gameObject.SetActive(true);

    }

    // Update is called once per frame
    public void onSettingsUpdated() {
        //print("we update!");
    }
}
