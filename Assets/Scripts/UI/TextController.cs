using Management;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI myText; 

    void Start()
    {
        
    }

    void Update()
    {
        // 动态更新数字
        myText.text = $"生命： {GameManager.Instance.playerHp} / 3";
    }
}