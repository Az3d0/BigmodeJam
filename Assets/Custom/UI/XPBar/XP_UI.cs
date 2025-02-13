using DG.Tweening;
using System.Xml.Linq;
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
        GameManager.Instance.OnXPUpdated += UpdateXPBar;
    }
    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += SetMaxXp;
        LevelManager.OnLevelLoaded += SetStartXp;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= SetMaxXp;
        LevelManager.OnLevelLoaded -= SetStartXp;
    }

    private void SetMaxXp(Level level)
    {
        m_maxXP = level.xpThreshold;
        Debug.Log("xp bar max xp set to: " + m_maxXP);
    }
    private void SetStartXp(Level level)
    {
        m_currentXP = GameManager.Instance.currentXp;
        UpdateXPBar(-1);
        Debug.Log("xp bar current xp set to: " + m_currentXP);
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
        if (xpAmount != -1)
        {
            if (xpAmount < m_currentXP)
            {
                m_XPBar.color = Color.red;
                m_loseMinigame.Play();
            }
            if (xpAmount > m_currentXP)
            {
                m_XPBar.color = Color.green;
                m_winMinigame.Play();
            }
            m_currentXP = xpAmount;
        }
        Debug.Log("updating xp bar: " + m_currentXP);
        Debug.Log("max xp: " + m_maxXP);
        float ratio = (float)m_currentXP / (float)m_maxXP;
        m_XPBar.transform.DOScaleX(ratio, m_fillSpeedInSec).OnComplete(() => m_XPBar.color = m_xpBarColor);
    }
}
