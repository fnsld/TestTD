using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text textHP;
    [SerializeField] private Text textGold;
    [SerializeField] private TextMeshProUGUI enemiesDefeatedText;
    [SerializeField] private GameObject loseOverlay;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        loseOverlay.SetActive(false);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        
        Bus.OnEnemyStepTarget.AddListener((x) => UpdateHP());
        Bus.OnGoldChanged.AddListener(UpdateGold);
        Bus.OnGameOver.AddListener(OnGameOver);
        
        UpdateGold();
        UpdateHP();
    }

    private void OnGameOver()
    {
        loseOverlay.SetActive(true);
        enemiesDefeatedText.text = $"Enemies defeated:\n<size=100>{Bus.EnemiesDead}</size>";
    }
    
    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene(0);
    }
    
    private void UpdateGold() => textGold.text = "Gold: " + Bus.PlayerGold;
    
    private void UpdateHP() => textHP.text = "HP: " + Mathf.Max(0, Bus.PlayerHealth);
}
