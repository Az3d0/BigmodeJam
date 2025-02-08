using System.Collections;
using UnityEngine;

public class BigRedButtonMinigame : Minigame
{
    public int numberOfPressesToWin = 10;
    public int counter = 0;

    public void RedButtonPressed()
    {
        counter++;
        if (counter >= numberOfPressesToWin) 
        {
            StartCoroutine(DelayTriggerWin());
        }
    }

    public IEnumerator DelayTriggerWin()
    {
        yield return new WaitForSeconds(0.2f);
        win = true;
        TriggerGameEnd();
    }

}
