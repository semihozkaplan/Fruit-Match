using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalCard : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _goalAmountTxt;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private GameObject _tickIcon;
    [SerializeField] private GameObject _backFace;
    [SerializeField] private Animator _animator;

    public void InitializeCard(int firstAmount, Sprite itemIcon)
    {
        _goalAmountTxt.text = firstAmount.ToString();
        _itemIcon.sprite = itemIcon;
        _tickIcon.SetActive(false);
    }

    public void UpdateGoalText(int amount)
    {
        _goalAmountTxt.text = amount.ToString();
        CardAnim();
    }

    private void CardAnim()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;

        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.08f)
            .setLoopPingPong(1);
    }

    public void CompleteGoal()
    {
        //gameObject.SetActive(false);

        _animator.Play("CompleteAnim");
        _goalAmountTxt.text = "";
        _tickIcon.SetActive(true);
    }
}
