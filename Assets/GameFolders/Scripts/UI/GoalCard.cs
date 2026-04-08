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

    private void Update()
    {
        _backFace.SetActive(Vector3.Dot(Vector3.forward, transform.forward) < 0);
    }

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

        LeanTween.scale(transform.GetChild(0).gameObject, Vector3.one * 1.1f, 0.08f)
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
