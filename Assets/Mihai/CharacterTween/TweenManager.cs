using UnityEngine;
using System.Collections;

public class mTween
{


    public static void moveTo(GameObject gameObj, float speed, Vector3 finish, CharacterController cc, float progress = 0)
    {
        gameObj.AddComponent<CharacterTween>();
        gameObj.GetComponent<CharacterTween>().startTween(speed, finish, cc, progress);

    }

    public static void moveTo(GameObject gameObj, float speed, Vector3[] path, CharacterController cc, float progress = 0)
    {
        gameObj.AddComponent<CharacterTween>();
        gameObj.GetComponent<CharacterTween>().startTween(speed, path, cc, progress);

    }
    public static void Stop(GameObject gameObj)
    {
        if (gameObj.GetComponent<CharacterTween>()!=null)
        {
           gameObj.GetComponent<CharacterTween>().Stop();
        }
    }
}
