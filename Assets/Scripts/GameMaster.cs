using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public Unit selectedUnit;
    public int playerTurn = 1;
    public GameObject selectedUnitCircle;

    public Image playerTurnIndicator;
    public Sprite player1Indicator, player2Indicator;
    public int player1Gold = 100, player2Gold = 100;
    public Text player1GoldText, player2GoldText;
    public BarrackItem purchasedItem;

    public GameObject statsPanel;
    public Vector2 statsPanelOffset;
    public Unit viewedUnit;

    public Text healthText, armorText, attackText, defenseText;

    public GameObject darkKing, lightKing;
    public GameObject lightVictoryPanel, darkVictoryPanel;

    public AudioSource camMainMusic;


    private void Start()
    {
        GetGoldIncome(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }

        if (selectedUnit)
        {
            selectedUnitCircle.SetActive(true);
            selectedUnitCircle.transform.position = selectedUnit.transform.position;
        }
        else
        {
            selectedUnitCircle.SetActive(false);
        }


        if (lightKing == null)
        {
            darkVictoryPanel.SetActive(true);
            StopMainMusic();
        }
        else if (darkKing == null)
        {
            lightVictoryPanel.SetActive(true);
            StopMainMusic();
        }
    }

    public void ToggleStatsPanel(Unit unit)
    {
        if (!unit.Equals(viewedUnit))
        {
            statsPanel.SetActive(true);
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelOffset;
            viewedUnit = unit;
            UpdateStatsPanel();
        }
        else
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }
    }
    public void UpdateStatsPanel()
    {
        if (viewedUnit)
        {
            healthText.text = viewedUnit.health.ToString();
            attackText.text = viewedUnit.attackDamage.ToString();
            armorText.text = viewedUnit.armor.ToString();
            defenseText.text = viewedUnit.defenceDamage.ToString();
        }
    }

    public void MoveStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelOffset;
        }
    }

    public void RemoveStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }

    }

    public void UpdateGoldText()
    {
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
    }

    void GetGoldIncome(int playerTurn)
    {
        foreach (Village village in FindObjectsOfType<Village>())
        {
            if (village.playerNumber == playerTurn)
            {
                if (playerTurn == 1)
                {
                    player1Gold += village.goldPerTurn;
                }
                else
                {
                    player2Gold += village.goldPerTurn;
                }
            }
        }
        UpdateGoldText();
    }

    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void EndTurn()
    {
        if (playerTurn == 2)
        {
            playerTurn = 1;
            playerTurnIndicator.sprite = player1Indicator;
        }
        else if (playerTurn == 1)
        {
            playerTurn = 2;
            playerTurnIndicator.sprite = player2Indicator;
        }

        GetGoldIncome(playerTurn);

        if (selectedUnit)
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }
        ResetTiles();

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
            unit.weaponIcon.SetActive(false);
            unit.hasAttacked = false;
        }

        GetComponent<Barrack>().CloseMenus();

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void StopMainMusic()
    {
        camMainMusic.Stop();
    }
}
