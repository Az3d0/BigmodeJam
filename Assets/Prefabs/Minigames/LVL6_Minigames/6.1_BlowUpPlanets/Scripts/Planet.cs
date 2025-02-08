using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    public DestroyPlanetsMinigame minigameParent;

    public void TriggerDestroy()
    {
        minigameParent.counter++;
        int random = Random.Range(0, minigameParent.m_SFX.Count);
        AudioClip randomAudio = minigameParent.m_SFX[random];
        minigameParent.audioSource.PlayOneShot(randomAudio);
        gameObject.GetComponent<Image>().sprite = minigameParent.explosion;
        DisableAfterExplosion();
        if (minigameParent.counter >= minigameParent.totalNumberOfPlanets)
        {
            WaitForLastExplosion();

        }
    }

    public void DisableAfterExplosion()
    {
        StartCoroutine(IEnumDisableAfterExplosion());
    }

    public IEnumerator IEnumDisableAfterExplosion()
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }

    public void WaitForLastExplosion()
    {
        StartCoroutine(IEnumWaitForLastExplosion());
    }

    public IEnumerator IEnumWaitForLastExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        minigameParent.win = true;
        minigameParent.TriggerGameEndParent();
    }
}
