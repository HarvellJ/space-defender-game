using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject defensePoint;

    public GameObject spawnManager;
    public GameObject smallHostileEnemyPrefab;
    public GameObject mediumHostileEnemyPrefab;
    public GameObject largeHostileEnemyPrefab;

    public GameObject smallNonHostileEnemyPrefab;
    public GameObject mediumNonHostileEnemyPrefab;
    public GameObject largeNonHostileEnemyPrefab;

    public Text waveText;
    public Text defensePointHealthText;
    public Text playerHealthText;
    public TextMesh gameOverHeaderText;
    public TextMesh restartGameText;
    public TextMesh quitGameText;
    public TextMesh wavesSurvivedText;

    public SimpleHealthBar playerHealthBar;
    public SimpleHealthBar defensePointHealthBar;

    private float timeOfBackgroundSpawn;
    private float timeOfLastAsteroidSpawn;
    private float backgroundSpawnCooldown = 5;
    private float asteroidSpawnCooldown = 7;
    private GameObject backgroundEnemyTargetPoint; // the point that the background spawned enemies will fly towards
    private float gameTime;

    bool gameOverMenuShown;
    int currentWave;
    List<EnemyTypeEnum> waveEnemies;
    GameObject[] waveEnemyGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        waveEnemies = new List<EnemyTypeEnum>();
        currentWave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        // Check game isn't over
        this.CheckGameOver();

        // Handle waves of eneimes
        this.UpdateEnemyWaves();

        // Update the dynamic content in the world
        this.UpdateWorldLayers();

        this.UpdateGameOverlay();
    }

    public bool AreEnemiesAlive()
    {
        waveEnemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        // print(waveEnemyGameObjects.Length);
        if (waveEnemyGameObjects.Length == 0)
            return false;
        else
            return true;
    }

    public void UpdateGameOverlay()
    {
 
        if (!CheckGameOver())
        {
            this.waveText.text = "Wave: " + currentWave;
            // Update health bars
            this.playerHealthBar.UpdateBar(player.GetComponent<GameCharacter>().Hp, player.GetComponent<GameCharacter>().MaxHp);
            if (GameObject.Find("DefensePoint"))
                this.defensePointHealthBar.UpdateBar(defensePoint.GetComponent<DestroyableStaticObject>().Hp, defensePoint.GetComponent<DestroyableStaticObject>().MaxHp);
            else
                this.defensePointHealthBar.UpdateBar(0, 100);

        }
        else // activate the game over menu
        {
            if (!gameOverMenuShown)
            {
                // hide game overlay
                waveText.gameObject.SetActive(false);
                defensePointHealthText.gameObject.SetActive(false);
                playerHealthText.gameObject.SetActive(false);
                playerHealthBar.gameObject.SetActive(false);
                defensePointHealthBar.gameObject.SetActive(false);
                // show game over menu
                wavesSurvivedText.text += currentWave;
                gameOverHeaderText.gameObject.SetActive(true);
                restartGameText.gameObject.SetActive(true);
                quitGameText.gameObject.SetActive(true);
                wavesSurvivedText.gameObject.SetActive(true);
                gameOverMenuShown = true;
            }
        }
    }

    public List<EnemyTypeEnum> GetNextWave()
    {
        currentWave++;
        if (currentWave % 1 == 0)
        {
            waveEnemies.Add(EnemyTypeEnum.small);
        }
        if (currentWave % 4 == 0)
        {
            if (currentWave - 4 == 0)
                waveEnemies = new List<EnemyTypeEnum>();

            waveEnemies.Add(EnemyTypeEnum.medium);
        }
        if (currentWave % 10 == 0)
        {
            if (currentWave - 10 == 0)
                waveEnemies = new List<EnemyTypeEnum>();

            waveEnemies.Add(EnemyTypeEnum.large);
        }

        return waveEnemies;
    }

    private void UpdateWorldLayers()
    {
        this.UpdateBackgroundNPCs();
        this.UpdateAsteroids();
    }

    private void UpdateBackgroundNPCs()
    {
        // get the point for NPCs to fly toward if it is null
        if (backgroundEnemyTargetPoint == null)
            backgroundEnemyTargetPoint = GameObject.Find("DistantEnemyFinish");

        // spawn some enemies
        if (gameTime > timeOfBackgroundSpawn + backgroundSpawnCooldown)
        {
            timeOfBackgroundSpawn = gameTime;
            GameObject flyInFromPoint = spawnManager.GetComponent<SpawnManager>().GetDistantSpawnPoint();
            System.Random r = new System.Random();
            int numberOfBackgroundEnemies = r.Next(6);
            for (int i = 0; i < numberOfBackgroundEnemies; i++)
            {
                Instantiate(this.GetRandomNonHostileEnemyPrefab(), flyInFromPoint.transform.position, backgroundEnemyTargetPoint.transform.rotation).GetComponent<EnemyController>().TargetLocation = backgroundEnemyTargetPoint;
            }

            timeOfBackgroundSpawn = gameTime;
        }

    }

    private void UpdateAsteroids()
    {
        // TODO - Implement asteroids
    }

    private GameObject GetRandomNonHostileEnemyPrefab()
    {
        System.Random random = new System.Random();

        int enemyType = random.Next(3);
        if (enemyType == 0)
            return smallNonHostileEnemyPrefab;
        else if (enemyType == 1)
            return mediumNonHostileEnemyPrefab;
        else
            return largeNonHostileEnemyPrefab;
    }

    private bool CheckGameOver()
    {
        if (!this.player.GetComponent<GameCharacter>().IsAlive || !GameObject.Find("DefensePoint"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateEnemyWaves()
    {
        if (!this.AreEnemiesAlive())
        {
            waveEnemies = GetNextWave();
            foreach (EnemyTypeEnum enemy in waveEnemies)
            {
                GameObject nextSpawnPoint = spawnManager.GetComponent<SpawnManager>().GetSpawnPoint();

                Vector3 flyInFromPoint = nextSpawnPoint.transform.position; // This is used to create the effect of the ship flying in from off the screen to the spawn
                if (flyInFromPoint.z > 0)
                    flyInFromPoint.z += 20;
                else
                    flyInFromPoint.z -= 20;

                if (enemy == EnemyTypeEnum.small)
                {
                    Instantiate(smallHostileEnemyPrefab, flyInFromPoint, nextSpawnPoint.transform.rotation).GetComponent<EnemyController>().TargetLocation = nextSpawnPoint; ;
                }
                else if (enemy == EnemyTypeEnum.medium)
                {
                    Instantiate(mediumHostileEnemyPrefab, flyInFromPoint, nextSpawnPoint.transform.rotation).GetComponent<EnemyController>().TargetLocation = nextSpawnPoint; ;
                }
                else if (enemy == EnemyTypeEnum.large)
                {
                    Instantiate(largeHostileEnemyPrefab, flyInFromPoint, nextSpawnPoint.transform.rotation).GetComponent<EnemyController>().TargetLocation = nextSpawnPoint; ;
                }

            }
        }
    }
}
