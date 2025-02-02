using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LayoffMinigame : PointNClickMinigame
{
    [SerializeField] private GameObject m_employeeAsset;
    [SerializeField] private List<GameObject> m_employeeSpots = new List<GameObject>();

    [SerializeField] private GameObject m_mouseFollowerAsset;
    private SpriteRenderer m_mouseFollowerSpriteRenderer;
    [SerializeField] private Sprite m_handNormalSprite;
    [SerializeField] private Sprite m_handFlickedSprite;
    private GameObject m_mouseFollower;

    private int m_employeeNum;
    private int m_firedEmployeeNum;
    protected override void Awake()
    {
        base.Awake();

        OnObjectHit += SetClickObject;
        m_mouseFollower = Instantiate(m_mouseFollowerAsset);
        if (m_mouseFollower.transform.GetChild(0).TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            m_mouseFollowerSpriteRenderer = spriteRenderer;
        }
        else
        {
            Debug.Log("No spriteRenderer attached");
        }
        m_mouseFollower.transform.parent = transform;
        m_mouseFollower.transform.position = gameObject.transform.position;
    }

    protected override void OnCLick(InputAction.CallbackContext context)
    {
        base.OnCLick(context);
        m_mouseFollowerSpriteRenderer.sprite = m_handFlickedSprite;
    }
    protected override void OnRelease(InputAction.CallbackContext context)
    {
        base.OnRelease(context);
        m_mouseFollowerSpriteRenderer.sprite = m_handNormalSprite;
    }

    private void FixedUpdate()
    {
        Vector3 mousePosition = new Vector3 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -5);
        m_mouseFollower.transform.localPosition = mousePosition;
    }

    public override void Start()
    {
        GenerateEmployees();
        base.Start();
    }
    protected override void OnDestroy()
    {
        OnObjectHit -= SetClickObject;
        base.OnDestroy();
    }

    private void GenerateEmployees()
    {
        m_employeeNum = Random.Range(3, m_employeeSpots.Count);

        for (int i = 0; i < m_employeeNum; i++)
        {
            Instantiate(m_employeeAsset, m_employeeSpots[i].transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Employee employee))
        {
            m_firedEmployeeNum++;
            if(m_firedEmployeeNum == m_employeeNum)
            {
                win = true;
                TriggerGameEnd();
            }
        }
    }
}
