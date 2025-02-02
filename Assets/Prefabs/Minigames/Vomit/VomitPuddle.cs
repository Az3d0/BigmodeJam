using System;
using System.Collections.Generic;
using UnityEngine;

public class VomitPuddle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer vomit;

    public event Action OnCleaned;
    [SerializeField] private List<AudioSource> m_SFX = new List<AudioSource>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mop" && Input.mousePositionDelta != Vector3.zero)
        {
            var tempColor = vomit.color;
            tempColor.a -= 0.1f;
            vomit.color = tempColor;
            if (vomit.color.a <= 0f)
            {
                OnCleaned?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mop" && Input.mousePositionDelta != Vector3.zero)
        {
            int random = UnityEngine.Random.Range(0, m_SFX.Count);
            m_SFX[random].Play();
        }
    }
}
