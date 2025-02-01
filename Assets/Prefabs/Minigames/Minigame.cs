using System;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public static event Action<bool> OnGameEnded;


    //update this in child classes depending on specific win requirements
    public bool win;

    protected void TriggerGameEnd()
    {
        Debug.Log($"{gameObject.name} - Minigame won? {win}");

        OnGameEnded?.Invoke(win); 
    }

    protected virtual void OnDestroy()
    {
        TriggerGameEnd();
    }

}
