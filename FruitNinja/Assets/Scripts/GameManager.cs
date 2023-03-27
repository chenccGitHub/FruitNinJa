using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    private int score;
    private Blade blade;
    private Spawner spawner;
    public Image fadeImage;
    public GameObject startGameObj;
    private Button startGameBtn;
    public Button restartBtn;
    public GameObject restartObj;
    private Image restartImage;
    public GameObject quitObj;
    private Button quitbtn;
    private Image quitImage;
    public int fallFruitNum = 4;
    public Text fallFruitText;
    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        blade.enabled = false;
        spawner.enabled = false;
        startGameBtn = startGameObj.GetComponentInChildren<Button>();
        quitbtn = quitObj.GetComponent<Button>();
        quitImage = quitObj.GetComponent<Image>();
        restartImage = restartObj.GetComponent<Image>();

    }
    private void Start()
    {
        startGameBtn.onClick.AddListener(() => 
        {
            StartCoroutine(StartGameSequence());
        });
        restartBtn.onClick.AddListener(() =>
        {
            StartCoroutine(RestartSequence());

        });
        quitbtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    private void NewGame()
    {
        Time.timeScale = 1f;

        ClearScene();

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();
        fallFruitNum = 4;
        fallFruitText.text = fallFruitNum.ToString();
    }
    public void InCreaseScore(int amount)
    {
        score+= amount;
        scoreText.text = score.ToString();
    }
    public void InFallFruitNum()
    {
        if (fallFruitNum < 1)
        {
            Explode();
            return;
        }
        fallFruitNum--;
        fallFruitText.text = fallFruitNum.ToString();
        

    }
    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }
    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;
        StartCoroutine(ExplodeSequence());
    }
    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;
        // Fade to white
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);
            restartImage.color = Color.Lerp(Color.clear, Color.white, t);
            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        ClearScene();
        restartObj.SetActive(true);
        fadeImage.enabled = true;
        //yield return new WaitForSecondsRealtime(1f);

        //NewGame();

        //elapsed = 0f;

        //// Fade back in
        //while (elapsed < duration)
        //{
        //    float t = Mathf.Clamp01(elapsed / duration);
        //    fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

        //    elapsed += Time.unscaledDeltaTime;

        //    yield return null;
        //}
    }
    private IEnumerator RestartSequence()
    {
        float elapsed = 0f;
        float duration = 0.3f;

        // Fade to white
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);
            restartImage.color = Color.Lerp(Color.white, Color.clear, t);
            quitImage.color = Color.Lerp(Color.clear, Color.white, t);
            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        startGameObj.SetActive(true);
        quitObj.SetActive(true);
        fadeImage.enabled = false;
        transform.GetComponent<AudioSource>().Play();
    }
    private IEnumerator StartGameSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        // Fade to white
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            quitImage.color = Color.Lerp(Color.white, Color.clear, t);
            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        startGameObj.SetActive(false);
        quitObj.SetActive(false);
        restartObj.SetActive(false);
        NewGame();
        transform.GetComponent<AudioSource>().Stop();


    }
}
