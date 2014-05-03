using UnityEngine;
using System.Collections;

public class ProcessReverbZones : MonoBehaviour {
    public GameObject debugObj;
    public ReverbZoneProperties[] reverbZones;
    private byte[,] byteMatrix;
    private int xSize, ySize;
    public byte kernelSize = 5;
  

	// Use this for initialization
	void Start () {
        //if(debugObj != null) debugObj.transform.localScale = new Vector3(1, 1, 1);
        
        LevelSpawn.FinishGeneration += processReverb;
	}

    void processReverb()
    {
        xSize = LevelSpawn.levelMatrix.GetLength(0);
        ySize = LevelSpawn.levelMatrix.GetLength(1);
        byteMatrix = new byte[xSize,ySize];
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (LevelSpawn.levelMatrix[x, y] == null) byteMatrix[x, y] = 0;
                else if (LevelSpawn.levelMatrix[x, y].isWalkable) byteMatrix[x, y] = 1;
                else byteMatrix[x, y] = 0;
            }
        }
        getBlobs();
    }
    byte[,] getRooms()
    {

        byte[,] room;

        room = ImageProcessing.ApplyErrosion(byteMatrix, kernelSize);
        room = ImageProcessing.ApplyDilation(room,kernelSize);
        if (debugObj != null)
        {
            for (int x = 0; x < room.GetLength(0); x++)
            {
                for (int y = 0; y < room.GetLength(1); y++)
                {
                    if (room[x, y] == 1) Instantiate(debugObj, new Vector3(x, 2, y) * 2, Quaternion.identity);
                }
            }
        }
        return room;
       
        
    }

    byte[,] getHallWays()
    {

        byte[,] n = byteMatrix;
        byte[,] m = ImageProcessing.ApplyErrosion(n, kernelSize);
        m = ImageProcessing.ApplyDilation(m, (byte)(kernelSize / 2));

        
        return ImageProcessing.Difference(n, m);
       
    }

    void getBlobs()
    {

        byte[,] roomBlobs = getRooms();
        byte[,] pathBlobs = getHallWays();
        byte numberOfRoomBlobs;
        byte numberOfPathBlobs;
        roomBlobs = ImageProcessing.CalculateBlobs(roomBlobs,out numberOfRoomBlobs);
        pathBlobs = ImageProcessing.CalculateBlobs(pathBlobs, out numberOfPathBlobs);

        int totalReverbZones = numberOfRoomBlobs+numberOfPathBlobs;

       

        reverbZones = new ReverbZoneProperties[totalReverbZones];
       
        for (byte i = 0; i < numberOfRoomBlobs; i++)
            {
                reverbZones[i] = new ReverbZoneProperties();
                reverbZones[i].area = ImageProcessing.CalculateArea(roomBlobs, i) * 4;
                reverbZones[i].perimeter = ImageProcessing.CalculatePerimeter(roomBlobs, i) * 2;
                reverbZones[i].height = 6;
                reverbZones[i].volume = reverbZones[i].area * 6;
                reverbZones[i].decayTime = reverbZones[i].CalculateDecayTime();
                

            }
        for (int i = numberOfRoomBlobs; i < numberOfRoomBlobs + numberOfPathBlobs; i++)
        {
            byte c = (byte)(i - numberOfRoomBlobs);
            reverbZones[i] = new ReverbZoneProperties();
            reverbZones[i].area = ImageProcessing.CalculateArea(pathBlobs, c) * 4;
            reverbZones[i].perimeter = ImageProcessing.CalculatePerimeter(pathBlobs, c) * 2;
            reverbZones[i].height = 6;
            reverbZones[i].volume = reverbZones[i].area * 6;
            reverbZones[i].decayTime = reverbZones[i].CalculateDecayTime();
            
        }

         for (int x = 0; x < roomBlobs.GetLength(0); x++)
        {
            for (int y = 0; y < roomBlobs.GetLength(1); y++)
            {
                if(roomBlobs[x,y]!=0){
                    if (LevelSpawn.levelMatrix[x, y] != null)
                    {
                       
                        LevelSpawn.levelMatrix[x, y].rzp = reverbZones[roomBlobs[x, y]];
                    }
                }
            }
        }
        for (int x = 0; x < pathBlobs.GetLength(0); x++)
        {
            for (int y = 0; y < pathBlobs.GetLength(1); y++)
            {
                if (pathBlobs[x, y] != 0)
                {
                    if (LevelSpawn.levelMatrix[x, y] != null)
                    {
                        LevelSpawn.levelMatrix[x, y].rzp = reverbZones[pathBlobs[x, y] + numberOfRoomBlobs];
                    }
                }
            }
        }
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in p){
            if(player.GetComponent<DetectReverbZone>()){
                player.GetComponent<DetectReverbZone>().setRZP();
            }
        }
       
    }

    
	
	
}
public class ReverbZoneProperties
{
    public float area;
    public float perimeter;
    public float volume;
    public float height;

    public float decayTime;
    public ReverbZoneProperties(float area, float perimeter, float volume, float decayTime, float height = 6)
    {
        this.area = area; 
        this.perimeter = perimeter;
        this.volume = volume;
        this.height = height;
        this.decayTime = decayTime;
    }
    public ReverbZoneProperties(float area, float perimeter, float volume, float height=6)
    {
        this.area = area;
        this.perimeter = perimeter;
        this.volume = volume;
        this.height = height;
        this.decayTime = CalculateDecayTime();

    }
    public ReverbZoneProperties(float area, float perimeter, float height = 6)
    {
        this.area = area;
        this.perimeter = perimeter;
        this.volume = area * height;
        this.height = height;
        this.decayTime = CalculateDecayTime();
    }
    public ReverbZoneProperties()
    {
        area = 0;
        perimeter = 0;
        volume = 0;
        decayTime = 0;
        height = 6;
    }
    public float CalculateDecayTime(float area, float perimeter)
    {
        return 0.1611f * volume / ((perimeter * height+area) * 0.05f);
    }
    public float CalculateDecayTime()
    {
       // Debug.Log(0.1611f+" * "+volume + "/(( " + perimeter + " * " + height + " + " + area + ") = " + (0.1611f * volume / ((perimeter * height + 2* area) * 0.05f)));
        return 0.1611f * volume / ((perimeter * height +2* area) * 0.05f);
       
    }
}
