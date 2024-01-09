using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform player;
    //public float distance = 3f;

    void Update()
    {
        transform.position = player.transform.position;
    }

}
