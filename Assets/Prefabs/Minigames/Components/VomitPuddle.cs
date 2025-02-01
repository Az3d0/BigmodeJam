using UnityEngine;

public class VomitPuddle : MonoBehaviour
{
    private SpriteRenderer vomit;

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
        }
    }
}
