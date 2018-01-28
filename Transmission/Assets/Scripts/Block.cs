using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject forwardSocket;
    public GameObject backwardSocket;
    
    public void PlaceAfter(Block previousBlock)
    {
        transform.position = previousBlock.backwardSocket.transform.position;
        transform.rotation = previousBlock.backwardSocket.transform.rotation;


    }

}
