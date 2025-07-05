using TMPro;
using UnityEngine;

public class GameDataSync : MonoBehaviour
{
    private Main main;
    public GameObject HeartBackground;
    public TextMeshProUGUI HealthRepresent;
    public TextMeshProUGUI MoneyRepresent;
    private Vector3 HBoriginScale;
    public void Start()
    {
        main = FindFirstObjectByType<Main>();
        HBoriginScale = HeartBackground.transform.localScale;
    }
    public void Update()
    {
        //HB scale
        float ratio = main.gameData.HP / main.gameData.maxHP;
        HeartBackground.transform.localScale = new Vector3(HBoriginScale.x, HBoriginScale.y * ratio, HBoriginScale.z);
        //HP represent
        HealthRepresent.text = "" + (int)main.gameData.HP;
        //Money represent
        MoneyRepresent.text = "" + main.gameData.money;
    }
}
