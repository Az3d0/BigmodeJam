using System;
using System.Collections.Generic;
using UnityEngine;

public class Neighbourhood : ClickableObject
{


    int m_life = 3;
    public event Action OnLiesBelieved;

    [Header("Neighbourhood specific values")]
    [Space(10)]

    [SerializeField] private GameObject m_explosion;
    [SerializeField] private List<GameObject> m_floatingTextAssets = new List<GameObject>();
    [SerializeField] private List<AudioClip> m_dataAudioClips = new List<AudioClip>();
    [SerializeField] private AudioSource m_boom;
    private void Start()
    {
        m_explosion.SetActive(false);
    }
    public override void OnClicked()
    {
        m_life--;
        if (m_floatingTextAssets.Count == 0 ) return;

        if (m_life > 0)
        {
            int random = UnityEngine.Random.Range(0, m_floatingTextAssets.Count);
            GameObject floatingtext = Instantiate(m_floatingTextAssets[random]);
            floatingtext.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -5);

            PlayRandomSFX(m_dataAudioClips);
        }

        if (m_life == 0)
        {
            OnLiesBelieved?.Invoke();

            m_boom.Play();
            m_explosion.SetActive(true);
            //play explosion
        }


        base.OnClicked();
    }
}
