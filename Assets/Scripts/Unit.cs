using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public bool selected;
    public int tilesSpeed;
    public bool hasMoved;
    public float moveSpeed;
    public int playerNumber;

    public int attackRange = 1;
    public List<Unit> enemiesInRange = new List<Unit>();
    public bool hasAttacked;
    public GameObject weaponIcon;

    public int health, attackDamage, defenceDamage, armor;

    public DamageIcon damageIcon;
    public GameObject deathEffect;

    public Text kingHealth;
    public bool isKing = false;

    public AudioClip attackSound, dieSound, selectSound;

    GameMaster gm;
    Animator camAnimator;
    AudioSource audioSource;
    

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        camAnimator = Camera.main.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        UpdateKingHealth();
    }

    public void Move(Vector2 tilePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(tilePos));
    }

    public void UpdateKingHealth()
    {
        if (isKing)
        {
            kingHealth.text = health.ToString();
        }
    }

    IEnumerator StartMovement(Vector2 tilePos)
    {
        while (transform.position.x != tilePos.x)
        {

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePos.x, transform.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.position.y != tilePos.y)
        {

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, tilePos.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;
        ResetWeaponIcons();
        GetEnemies();
        gm.MoveStatsPanel(this);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gm.ToggleStatsPanel(this);
        }
    }

    private void OnMouseDown()
    {
        ResetWeaponIcons();
        if (selected)
        {
            selected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();
        }
        else
        {
            if (playerNumber == gm.playerTurn)
            {

                if (gm.selectedUnit)
                {
                    gm.selectedUnit.selected = false;
                }
                selected = true;
                gm.selectedUnit = this;
                gm.ResetTiles();
                GetEnemies();
                GetWalkableTiles();
            }
        }

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), .15f);
        Unit unit = col.GetComponent<Unit>();
        if (gm.selectedUnit)
        {
            if (gm.selectedUnit.enemiesInRange.Contains(unit) && !gm.selectedUnit.hasAttacked)
            {
                gm.selectedUnit.Attack(unit);
            }
        }

    }

    public void Attack(Unit enemy)
    {

        camAnimator.SetTrigger("shake");
        audioSource.clip = attackSound;
        audioSource.Play();

        hasAttacked = true;
        int enemyDamage = attackDamage - enemy.armor;
        int myDamage = enemy.defenceDamage - armor;

        if (enemyDamage >= 1)
        {
            DamageIcon damageIconInstance = Instantiate(damageIcon, enemy.transform.position, Quaternion.identity);
            damageIconInstance.Setup(enemyDamage);
            enemy.health -= enemyDamage;
            enemy.UpdateKingHealth();
        }


        if (transform.tag == "Ranged" && enemy.tag != "Ranged")
        {
            if (Mathf.Abs(transform.position.x - enemy.transform.position.x) + Mathf.Abs(transform.position.y - enemy.transform.position.y) <= 1)
            {
                TakeDamage(myDamage);
            }
        }
        else
        {
            TakeDamage(myDamage);
        }


        if (enemy.health <= 0)
        {
            Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
            Destroy(enemy.gameObject);
            GetWalkableTiles();
            gm.RemoveStatsPanel(enemy);
        }
        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gm.ResetTiles();
            gm.RemoveStatsPanel(this);
            Destroy(this.gameObject);
        }
        gm.UpdateStatsPanel();
    }

    private void TakeDamage(int myDamage)
    {
        if (myDamage >= 1)
        {
            DamageIcon damageIconInstance = Instantiate(damageIcon, transform.position, Quaternion.identity);
            damageIconInstance.Setup(myDamage);
            health -= myDamage;
            UpdateKingHealth();
        }
    }

    public void ResetWeaponIcons()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.weaponIcon.SetActive(false);
        }
    }

    void GetWalkableTiles()
    {
        if (hasMoved)
        {
            return;
        }
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if ((Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y)) <= tilesSpeed)
            {
                if (tile.IsClear())
                {
                    tile.Highlight();
                }
            }
        }
    }

    void GetEnemies()
    {
        enemiesInRange.Clear();

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if ((Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y)) <= attackRange)
            {
                if (unit.playerNumber != gm.playerTurn && hasAttacked == false)
                {
                    enemiesInRange.Add(unit);
                    unit.weaponIcon.SetActive(true);
                }
            }
        }

    }
}
