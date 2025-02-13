using UnityEngine;

public class Collectable : MonoBehaviour
{
    public void EmitFeedback(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("EmittingFeeback");
        }
        else
        {
            Debug.Log("StopEmmitingFeedback");
        }
    }
}
