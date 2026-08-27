using TMPro;
using UnityEngine;

public class Card : MonoBehaviour {
    public int serverNumber;
    public bool IsGood;
    public string Text;

    [SerializeField]
    private TextMeshProUGUI text;
    
    private void Awake() {
        text.text = Text;
    }
    
    public void Select() {
        CardsManager.Instance.SelectCard(this);
    }
}
