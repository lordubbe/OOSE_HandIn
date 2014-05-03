using UnityEngine;
using System.Collections;

public class DetectReverbZone : MonoBehaviour {

    ReverbZoneProperties rzp;
    bool startSearching = false;
    public AudioReverbZone arz;
   
    public void setRZP()
    {
        startSearching = true;
        int x = (int)(transform.position.x / 2);
        int y = (int)(transform.position.y / 2);
        int z = (int)(transform.position.z / 2);
        rzp = LevelSpawn.levelMatrix[x, z].rzp;

        arz.decayTime = rzp.decayTime;
    }
    private void Update()
    {
        if (startSearching)
        {
            int x = (int)(transform.position.x / 2);
            int y = (int)(transform.position.y / 2);
            int z = (int)(transform.position.z / 2);
            if(LevelSpawn.levelMatrix[x,z] !=null)
            if(LevelSpawn.levelMatrix[x,z].rzp !=null)
            if (LevelSpawn.levelMatrix[x,z].rzp.decayTime != rzp.decayTime)
            {
                rzp = LevelSpawn.levelMatrix[x, z].rzp;
                arz.decayTime = rzp.decayTime;

                Debug.Log("new decay time:" + arz.decayTime);
            }

        }
    }
}
