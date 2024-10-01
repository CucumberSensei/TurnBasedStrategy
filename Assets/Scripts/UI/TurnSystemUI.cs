using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnSignal;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        UpdateTurnNumber();
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        UpdateEnemyTurnSignal();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnChange(object sender, System.EventArgs e)
    {
        UpdateTurnNumber();
        UpdateEnemyTurnSignal();
        UpdateEndTurnButtonVisibility();
    }

    public void UpdateTurnNumber()
    {
        turnNumberText.text = "TURN: " + TurnSystem.Instance.GetTurnNumber();
    }

    public void UpdateEnemyTurnSignal()
    {
        enemyTurnSignal.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

}
