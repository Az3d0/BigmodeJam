using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public string sceneName;
    public int xpThreshold;
    public Sprite playerSprite;
    public Sprite employeeCardSprite;

    public Level(string name, int xpThreshold)
    {
        this.sceneName = name;
        this.xpThreshold = xpThreshold;
    }


}
