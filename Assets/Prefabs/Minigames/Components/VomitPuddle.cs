using System;
using UnityEngine;

public class VomitPuddle : MonoBehaviour
{
    private SpriteRenderer vomit;

    public event Action OnCleaned;
    private void Awake()
    {
        vomit = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Wipe");
        if(collision.gameObject.tag == "Mop" && Input.mousePositionDelta != Vector3.zero)
        {
            var tempColor = vomit.color;
            tempColor.a -= 0.1f;
            vomit.color = tempColor;
            if(vomit.color.a <= 0f)
            {
                OnCleaned?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
