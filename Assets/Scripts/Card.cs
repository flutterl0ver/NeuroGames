using UnityEngine;

public class Card : MonoBehaviour {
    public int serverNumber;

    public void Select() {
        CardsManager.Instance.SelectCard(this);
    }
}
