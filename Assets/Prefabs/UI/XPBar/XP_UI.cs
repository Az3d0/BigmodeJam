using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class XP_UI : MonoBehaviour
{
    [SerializeField] private Image m_XPBar;
    private Color m_xpBarColor;
    [SerializeField] private float m_fillSpeedInSec = 0.5f; 
    private int m_maxXP;
    private int m_currentXP;

    private void Awake()
    {
        m_xpBarColor = m_XPBar.color;
    }
    private void Start()
    {
        GameManager.Instance.OnXPUpdated += UpdateXPBar;
        ResetXPBar();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnXPUpdated -= UpdateXPBar;
    }
    private void ResetXPBar()
    {
        m_maxXP = GameManager.Instance.XPRequiredToGoNextLevel;
        m_XPBar.transform.localScale = new Vector3(0, 1, 1);
    }
    private void UpdateXPBar(int xpAmount)
    {
        if(xpAmount < m_currentXP)
        {
            m_XPBar.color = Color.red;
        }
        if(xpAmount > m_currentXP)
        {
            m_XPBar.color = Color.green;
        }
        m_currentXP = xpAmount;
        float ratio = (float)m_currentXP / (float)m_maxXP;
        m_XPBar.transform.DOScaleX(ratio, m_fillSpeedInSec).OnComplete(() => m_XPBar.color = m_xpBarColor);
    }
}
