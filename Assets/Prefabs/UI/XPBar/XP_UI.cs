using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class XP_UI : MonoBehaviour
{
    [SerializeField] private Image m_XPBar;
    private Color m_xpBarColor;
    [SerializeField] private float m_fillSpeedInSec = 0.5f;
    [SerializeField] private AudioSource m_winMinigame;
    [SerializeField] private AudioSource m_loseMinigame;
    private int m_maxXP;
    private int m_currentXP;

    private void Awake()
    {
        ResetXPBar();
    }
    private void Start()
    {
        m_maxXP = GameManager.Instance.XPRequiredToGoNextLevel;
        GameManager.Instance.OnXPUpdated += UpdateXPBar;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnXPUpdated -= UpdateXPBar;
    }
    private void ResetXPBar()
    {
        m_xpBarColor = m_XPBar.color;
        m_XPBar.transform.localScale = new Vector3(0, 1, 1);
    }
    private void UpdateXPBar(int xpAmount)
    {
        if(xpAmount < m_currentXP)
        {
            m_XPBar.color = Color.red;
            m_loseMinigame.Play();
        }
        if(xpAmount > m_currentXP)
        {
            m_XPBar.color = Color.green;
            m_winMinigame.Play();
        }
        m_currentXP = xpAmount;
        float ratio = (float)m_currentXP / (float)m_maxXP;
        m_XPBar.transform.DOScaleX(ratio, m_fillSpeedInSec).OnComplete(() => m_XPBar.color = m_xpBarColor);
    }
}
