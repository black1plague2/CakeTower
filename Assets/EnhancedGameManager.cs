using UnityEngine;
using System.Collections;
using TMPro;

public class EnhancedGameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject cakeSlicePrefab;
    public GameObject perfectEffectPrefab;
    public GameObject confettiPrefab;
    public GameObject[] cakeVariations;
    public GameObject[] specialEffectPrefabs;
    
    [Header("Visual Effects")]
    public ParticleSystem backgroundParticles;
    public ParticleSystem rainParticles;
    public Light mainLight;
    public Light[] accentLights;
    public Gradient skyboxGradient;
    public Material[] backgroundMaterials;
    
    [Header("Game Settings")]
    public float moveSpeed = 3f;
    public float speedIncrease = 0.15f;
    public float spawnHeight = 1.8f;
    public float perfectThreshold = 0.15f;
    public float difficultyMultiplier = 1.2f;
    
    [Header("Power-up Settings")]
    public float slowMotionDuration = 4f;
    public float slowMotionScale = 0.4f;
    public float sizeBoostMultiplier = 1.3f;
    public float scoreMultiplierValue = 2.5f;
    public float magnetDuration = 5f;
    public float shieldDuration = 8f;
    
    [Header("Gameplay UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI streakText;
    public UnityEngine.UI.Button startButton;
    public GameObject gameOverPanel;
    public GameObject mainMenuPanel;
    public GameObject gameplayPanel;
    public GameObject pausePanel;
    
    [Header("Game Over UI Elements")]
    public TextMeshProUGUI gameOverTitle;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI perfectDropsText;
    public TextMeshProUGUI bestComboText;
    public TextMeshProUGUI newRecordText;
    public TextMeshProUGUI heightAchievedText;
    public TextMeshProUGUI timePlayedText;
    public UnityEngine.UI.Button restartButton;
    public UnityEngine.UI.Button mainMenuButton;
    public UnityEngine.UI.Button shareButton;
    
    [Header("Power-up UI")]
    public TextMeshProUGUI slowMotionText;
    public TextMeshProUGUI sizeBoostText;
    public TextMeshProUGUI scoreMultiplierText;
    public TextMeshProUGUI magnetText;
    public TextMeshProUGUI shieldText;
    public UnityEngine.UI.Image powerUpIcon;
    public UnityEngine.UI.Slider powerUpCooldown;
    
    [Header("Visual UI Elements")]
    public UnityEngine.UI.Slider progressBar;
    public TextMeshProUGUI levelText;
    public GameObject[] stars;
    public UnityEngine.UI.Image backgroundImage;
    public TextMeshProUGUI instructionText;
    public UnityEngine.UI.Slider healthBar;
    public TextMeshProUGUI fpsText;
    public GameObject[] achievementPopups;
    
    [Header("Audio")]
    public AudioClip[] dropSounds;
    public AudioClip[] perfectSounds;
    public AudioClip[] comboSounds;
    public AudioClip gameOverSound;
    public AudioClip levelUpSound;
    public AudioClip[] backgroundMusic;
    public AudioClip buttonClickSound;
    public AudioClip powerUpSound;
    public AudioClip achievementSound;
    [Header("UI Animation Settings")]
    public float uiAnimationSpeed = 2f;
    public AnimationCurve uiBounceCurve;
    public AnimationCurve uiFadeCurve;
    [Header("Advanced Features")]
    public bool enableScreenEffects = true;
    public bool enableParticleEffects = true;
    public bool enableDynamicMusic = true;
    public bool enableHapticFeedback = true;
    public float targetFrameRate = 60f;
    // Add particle effects to UI elements
[Header("UI Particle Effects")]
public ParticleSystem scoreParticles;
public ParticleSystem comboParticles;
public ParticleSystem levelUpParticles;

void CreateUIParticleEffect(Vector3 worldPosition, ParticleSystem particleSystem)
{
    if (particleSystem != null)
    {
        // Convert world position to screen position
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);
        
        // Convert screen position to UI position
        RectTransform canvasRect = gameplayPanel.GetComponent<RectTransform>();
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenPos, null, out uiPos);
        
        // Create temporary particle object
        GameObject tempParticles = Instantiate(particleSystem.gameObject);
        tempParticles.transform.SetParent(gameplayPanel.transform);
        tempParticles.transform.localPosition = uiPos;
        
        Destroy(tempParticles, 3f);
    }
}

    // Enhanced Game State Variables
    private GameObject currentSlice;
    private Vector3 lastSliceSize;
    private Vector3 lastSlicePosition;
    private bool isMoving;
    private bool gameStarted;
    private bool gameOver;
    private bool gamePaused;
    
    // Enhanced Scoring System
    private int score = 0;
    private int combo = 0;
    private int perfectDrops = 0;
    private int bestCombo = 0;
    private int totalPerfectDrops = 0;
    private int level = 1;
    private int slicesStacked = 0;
    private int streak = 0;
    private int maxStreak = 0;
    private float towerHeight = 0f;
    private float gameTime = 0f;
    private int health = 3;
    private int maxHealth = 3;
    
    // Power-up System
    private bool scoreMultiplierActive = false;
    private bool slowMotionActive = false;
    private bool sizeBoostActive = false;
    private bool magnetActive = false;
    private bool shieldActive = false;
    
    // Visual Effects
    private Camera mainCamera;
    private Vector3 originalCameraPos;
    private Color originalLightColor;
    private float cameraShakeIntensity = 0f;
    
    // Audio
    private AudioSource audioSource;
    private AudioSource musicSource;
    private AudioSource ambientSource;
    
    // Enhanced Cake Themes
    private CakeTheme[] cakeThemes;
    
    // Power-up timers
    private float slowMotionTimer = 0f;
    private float sizeBoostTimer = 0f;
    private float scoreMultiplierTimer = 0f;
    private float magnetTimer = 0f;
    private float shieldTimer = 0f;
    private float powerUpSpawnTimer = 0f;
    
    // Visual enhancement variables
    private int currentMusicTrack = 0;
    private float backgroundHue = 0f;
    private bool isRainbowMode = false;
    private bool isNightMode = false;
    private float frameTime = 0f;
    private int frameCount = 0;
    
    // Achievement System
    private bool[] achievements = new bool[10];
    private string[] achievementNames = {
        "First Tower", "Perfect Starter", "Combo Master", "Sky High", "Speed Demon",
        "Perfectionist", "Tower Lord", "Unstoppable", "Legendary", "Cake God"
    };
    
    [System.Serializable]
    public class CakeTheme
    {
        public string name;
        public Color primaryColor;
        public Color secondaryColor;
        public string symbol;
        
        public CakeTheme(string n, Color p, Color s, string sym)
        {
            name = n;
            primaryColor = p;
            secondaryColor = s;
            symbol = sym;
        }
    }
    
    void Start()
    {
        // Initialize Unity-dependent variables first
        lastSliceSize = new Vector3(2.8f, 0.4f, 2.8f);
        lastSlicePosition = Vector3.zero;
        isMoving = false;
        gameStarted = false;
        gameOver = false;
        gamePaused = false;
        
        // Set target frame rate
        Application.targetFrameRate = (int)targetFrameRate;
        
        // Initialize cake themes with symbols instead of emojis
        InitializeCakeThemes();
        
        InitializeGame();
    }
    
    void InitializeCakeThemes()
    {
        cakeThemes = new CakeTheme[]
        {
            new CakeTheme("Vanilla Dream", new Color(1f, 0.95f, 0.8f), new Color(1f, 1f, 0.9f), "V"),
            new CakeTheme("Chocolate Delight", new Color(0.4f, 0.2f, 0.1f), new Color(0.6f, 0.3f, 0.15f), "C"),
            new CakeTheme("Strawberry Bliss", new Color(1f, 0.6f, 0.7f), new Color(1f, 0.8f, 0.85f), "S"),
            new CakeTheme("Lemon Zest", new Color(1f, 1f, 0.3f), new Color(1f, 1f, 0.7f), "L"),
            new CakeTheme("Blueberry Magic", new Color(0.4f, 0.4f, 0.8f), new Color(0.6f, 0.6f, 0.9f), "B"),
            new CakeTheme("Mint Fresh", new Color(0.6f, 0.9f, 0.7f), new Color(0.8f, 1f, 0.85f), "M"),
            new CakeTheme("Caramel Gold", new Color(0.8f, 0.5f, 0.2f), new Color(0.9f, 0.7f, 0.4f), "G"),
            new CakeTheme("Rainbow Surprise", Color.HSVToRGB(Random.value, 0.7f, 0.9f), Color.white, "R"),
            new CakeTheme("Cosmic Dark", new Color(0.2f, 0.1f, 0.3f), new Color(0.4f, 0.2f, 0.6f), "X"),
            new CakeTheme("Fire Cake", new Color(1f, 0.3f, 0.1f), new Color(1f, 0.6f, 0.2f), "F")
        };
    }
    
    void InitializeGame()
    {
        // Setup camera with cinematic feel
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalCameraPos = mainCamera.transform.position;
            mainCamera.fieldOfView = 60f;
        }
        
        // Setup enhanced lighting
        SetupLighting();
        
        // Setup audio with multiple sources
        SetupAudioSources();
        
        // Initialize visual effects
        InitializeVisualEffects();
        
        // Load saved data
        LoadGameData();
        
        // Initialize UI with animations
        ShowMainMenu();
        
        // Setup button listeners
        SetupButtonListeners();
        
        // Start background effects
        StartCoroutine(BackgroundColorCycle());
        StartCoroutine(ParticleEffectManager());
        StartCoroutine(FPSCounter());
        StartCoroutine(AutoSaveProgress());
    }
    
    void SetupLighting()
    {
        if (mainLight != null)
        {
            originalLightColor = mainLight.color;
            mainLight.intensity = 1.2f;
            mainLight.shadows = LightShadows.Soft;
        }
        
        // Setup accent lights for dramatic effect
        if (accentLights != null)
        {
            for (int i = 0; i < accentLights.Length; i++)
            {
                if (accentLights[i] != null)
                {
                    accentLights[i].intensity = 0.5f;
                    accentLights[i].range = 10f;
                    accentLights[i].color = Color.HSVToRGB(i * 0.3f, 0.7f, 1f);
                }
            }
        }
    }
    
    void SetupAudioSources()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        // Create dedicated music source
        GameObject musicObject = new GameObject("MusicSource");
        musicObject.transform.SetParent(transform);
        musicSource = musicObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 0.4f;
        
        // Create ambient source
        GameObject ambientObject = new GameObject("AmbientSource");
        ambientObject.transform.SetParent(transform);
        ambientSource = ambientObject.AddComponent<AudioSource>();
        ambientSource.loop = true;
        ambientSource.volume = 0.2f;
        
        // Play random background music
        if (backgroundMusic != null && backgroundMusic.Length > 0)
        {
            currentMusicTrack = Random.Range(0, backgroundMusic.Length);
            musicSource.clip = backgroundMusic[currentMusicTrack];
            musicSource.Play();
        }
    }
    
    void InitializeVisualEffects()
    {
        // Initialize background particles
        if (backgroundParticles != null)
        {
            var emission = backgroundParticles.emission;
            emission.rateOverTime = 20f;
            
            var shape = backgroundParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(20f, 20f, 20f);
        }
        
        // Initialize rain particles for atmosphere
        if (rainParticles != null)
        {
            rainParticles.Stop();
        }
        
        // Set initial background
        UpdateBackgroundTheme();
    }
    IEnumerator AnimateScoreIncrease(TextMeshProUGUI scoreText, int oldScore, int newScore)
{
    float elapsed = 0f;
    float duration = 0.5f;
    
    while (elapsed < duration)
    {
        float t = elapsed / duration;
        int displayScore = Mathf.RoundToInt(Mathf.Lerp(oldScore, newScore, t));
        scoreText.text = "SCORE: " + displayScore.ToString("N0");
        
        // Add bounce effect
        float bounce = uiBounceCurve.Evaluate(t);
        scoreText.transform.localScale = Vector3.one * (1f + bounce * 0.2f);
        
        elapsed += Time.deltaTime;
        yield return null;
    }
    
    scoreText.transform.localScale = Vector3.one;
}
    IEnumerator FPSCounter()
    {
        while (true)
        {
            frameTime += Time.deltaTime;
            frameCount++;
            
            if (frameTime >= 1f)
            {
                int fps = Mathf.RoundToInt(frameCount / frameTime);
                if (fpsText != null)
                    fpsText.text = "FPS: " + fps;
                
                frameTime = 0f;
                frameCount = 0;
            }
            
            yield return null;
        }
    }
    
    IEnumerator AutoSaveProgress()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            SaveGameData();
        }
    }
    
    IEnumerator BackgroundColorCycle()
    {
        while (true)
        {
            backgroundHue += Time.deltaTime * 0.1f;
            if (backgroundHue > 1f) backgroundHue = 0f;
            
            if (isRainbowMode)
            {
                Color newColor = Color.HSVToRGB(backgroundHue, 0.3f, 0.9f);
                if (backgroundImage != null)
                    backgroundImage.color = newColor;
                
                RenderSettings.fogColor = newColor;
                
                // Update accent lights
                if (accentLights != null)
                {
                    for (int i = 0; i < accentLights.Length; i++)
                    {
                        if (accentLights[i] != null)
                        {
                            float hue = (backgroundHue + i * 0.2f) % 1f;
                            accentLights[i].color = Color.HSVToRGB(hue, 0.8f, 1f);
                        }
                    }
                }
            }
            
            yield return null;
        }
    }
    
    IEnumerator ParticleEffectManager()
    {
        while (true)
        {
            if (combo >= 5 && backgroundParticles != null && enableParticleEffects)
            {
                var emission = backgroundParticles.emission;
                emission.rateOverTime = 50f + (combo * 5f);
                
                var velocityOverLifetime = backgroundParticles.velocityOverLifetime;
                velocityOverLifetime.enabled = true;
                velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            }
            
            // Weather effects based on performance
            if (combo >= 15 && rainParticles != null)
            {
                if (!rainParticles.isPlaying)
                    rainParticles.Play();
            }
            else if (rainParticles != null && rainParticles.isPlaying)
            {
                rainParticles.Stop();
            }
            
            yield return new WaitForSeconds(0.5f);
        }
    }

IEnumerator AnimateProgressBar(float startValue, float endValue)
{
    float elapsed = 0f;
    float duration = 0.3f;
    
    while (elapsed < duration)
    {
        float t = elapsed / duration;
        progressBar.value = Mathf.Lerp(startValue, endValue, t);
        elapsed += Time.deltaTime;
        yield return null;
    }
    
    progressBar.value = endValue;
}

    void LoadGameData()
    {
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        bestCombo = PlayerPrefs.GetInt("BestCombo", 0);
        totalPerfectDrops = PlayerPrefs.GetInt("TotalPerfectDrops", 0);
        maxStreak = PlayerPrefs.GetInt("MaxStreak", 0);
        
        // Load achievements
        for (int i = 0; i < achievements.Length; i++)
        {
            achievements[i] = PlayerPrefs.GetInt("Achievement_" + i, 0) == 1;
        }
        
        if (highScoreText != null)
            highScoreText.text = "High Score: " + savedHighScore.ToString("N0");
    }
    
    void SaveGameData()
    {
        PlayerPrefs.SetInt("TotalPerfectDrops", totalPerfectDrops);
        PlayerPrefs.SetInt("BestCombo", bestCombo);
        PlayerPrefs.SetInt("MaxStreak", maxStreak);
        
        // Save achievements
        for (int i = 0; i < achievements.Length; i++)
        {
            PlayerPrefs.SetInt("Achievement_" + i, achievements[i] ? 1 : 0);
        }
        
        PlayerPrefs.Save();
    }
    
    void SetupButtonListeners()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() => { PlayButtonSound(); StartGame(); });
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() => { PlayButtonSound(); RestartGame(); });
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(() => { PlayButtonSound(); ShowMainMenu(); });
        }
        
        if (shareButton != null)
        {
            shareButton.onClick.RemoveAllListeners();
            shareButton.onClick.AddListener(() => { PlayButtonSound(); ShareScore(); });
        }
    }
    
    void PlayButtonSound()
    {
        if (buttonClickSound != null)
            PlaySFX(buttonClickSound, 0.7f, 1f);
    }
    
    void ShareScore()
    {
        // Enhanced share functionality
        string shareText = "I just scored " + score.ToString("N0") + " points in Cake Tower! Can you beat my score?";
        Debug.Log("Sharing: " + shareText);
        
        // Copy to clipboard for easy sharing
        GUIUtility.systemCopyBuffer = shareText;
        
        #if UNITY_ANDROID || UNITY_IOS
        // Mobile sharing could be implemented here
        #endif
    }
    
    void Update()
    {
        if (!gameStarted || gameOver || gamePaused) 
        {
            if (Input.GetKeyDown(KeyCode.Escape) && gameStarted && !gameOver)
            {
                TogglePause();
            }
            return;
        }
        
        gameTime += Time.deltaTime;
        
        bool dropInput = Input.GetKeyDown(KeyCode.Space) || 
                        Input.GetMouseButtonDown(0) || 
                        Input.GetKeyDown(KeyCode.Return);
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            return;
        }
        
        if (dropInput)
        {
            if (isMoving)
            {
                DropSlice();
            }
            else if (currentSlice == null)
            {
                SpawnNewSlice();
            }
        }
        
        if (isMoving && currentSlice != null)
        {
            MoveSlice();
        }
        
        UpdatePowerUpTimers();
        UpdateDynamicLighting();
        UpdateProgressBar();
        UpdateCameraShake();
        UpdatePowerUpSpawning();
    }
    
    void TogglePause()
    {
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0f : 1f;
        
        if (pausePanel != null)
            pausePanel.SetActive(gamePaused);
    }
    
    void UpdateCameraShake()
    {
        if (cameraShakeIntensity > 0f && mainCamera != null)
        {
            Vector3 shakeOffset = new Vector3(
                Random.Range(-1f, 1f) * cameraShakeIntensity,
                Random.Range(-1f, 1f) * cameraShakeIntensity,
                0f
            );
            
            mainCamera.transform.position = originalCameraPos + shakeOffset;
            cameraShakeIntensity = Mathf.Lerp(cameraShakeIntensity, 0f, Time.deltaTime * 5f);
        }
    }
    
    void UpdatePowerUpSpawning()
    {
        powerUpSpawnTimer += Time.deltaTime;
        
        if (powerUpSpawnTimer >= 15f && combo >= 3)
        {
            SpawnRandomPowerUp();
            powerUpSpawnTimer = 0f;
        }
    }
    
    void SpawnRandomPowerUp()
    {
        int powerUpType = Random.Range(0, 5);
        
        switch (powerUpType)
        {
            case 0: ActivateSlowMotion(); break;
            case 1: ActivateSizeBoost(); break;
            case 2: ActivateScoreMultiplier(); break;
            case 3: ActivateMagnet(); break;
            case 4: ActivateShield(); break;
        }
        
        if (powerUpSound != null)
            PlaySFX(powerUpSound, 0.8f, Random.Range(0.9f, 1.1f));
    }
    
    void UpdateDynamicLighting()
    {
        if (mainLight != null)
        {
            if (combo >= 20)
            {
                mainLight.color = Color.Lerp(originalLightColor, Color.magenta, Mathf.Sin(Time.time * 4f) * 0.5f + 0.5f);
                mainLight.intensity = 1.5f + Mathf.Sin(Time.time * 6f) * 0.3f;
            }
            else if (combo >= 10)
            {
                mainLight.color = Color.Lerp(originalLightColor, Color.yellow, Mathf.Sin(Time.time * 3f) * 0.5f + 0.5f);
                mainLight.intensity = 1.3f;
            }
            else if (combo >= 5)
            {
                mainLight.color = Color.Lerp(originalLightColor, Color.cyan, 0.3f);
                mainLight.intensity = 1.2f;
            }
            else
            {
                mainLight.color = Color.Lerp(mainLight.color, originalLightColor, Time.deltaTime);
                mainLight.intensity = Mathf.Lerp(mainLight.intensity, 1.2f, Time.deltaTime);
            }
        }
    }
    
    void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            int nextLevelTarget = level * 10;
            float progress = (float)slicesStacked / nextLevelTarget;
            progressBar.value = progress;
        }
        
        if (healthBar != null)
        {
            healthBar.value = (float)health / maxHealth;
            // FIXED: Added null check for fillRect
            if (healthBar.fillRect != null)
            {
                UnityEngine.UI.Image fillImage = healthBar.fillRect.GetComponent<UnityEngine.UI.Image>();
                if (fillImage != null)
                    fillImage.color = health > 1 ? Color.green : Color.red;
            }
        }
    }
    
    void UpdatePowerUpTimers()
    {
        if (slowMotionActive)
        {
            slowMotionTimer -= Time.unscaledDeltaTime;
            if (slowMotionTimer <= 0)
            {
                slowMotionActive = false;
                Time.timeScale = 1f;
                if (slowMotionText != null)
                    slowMotionText.gameObject.SetActive(false);
            }
            else if (slowMotionText != null)
            {
                slowMotionText.text = "<color=#4169E1><b>SLOW TIME: " + Mathf.Ceil(slowMotionTimer).ToString() + "</b></color>";
                Time.timeScale = slowMotionScale;
            }
        }
        
        if (sizeBoostActive)
        {
            sizeBoostTimer -= Time.deltaTime;
            if (sizeBoostTimer <= 0)
            {
                sizeBoostActive = false;
                if (sizeBoostText != null)
                    sizeBoostText.gameObject.SetActive(false);
            }
            else if (sizeBoostText != null)
            {
                sizeBoostText.text = "<color=#32CD32><b>BIG SIZE: " + Mathf.Ceil(sizeBoostTimer).ToString() + "</b></color>";
            }
        }
        
        if (scoreMultiplierActive)
        {
            scoreMultiplierTimer -= Time.deltaTime;
            if (scoreMultiplierTimer <= 0)
            {
                scoreMultiplierActive = false;
                if (scoreMultiplierText != null)
                    scoreMultiplierText.gameObject.SetActive(false);
            }
            else if (scoreMultiplierText != null)
            {
                scoreMultiplierText.text = "<color=#FF1493><b>BONUS x" + scoreMultiplierValue + ": " + Mathf.Ceil(scoreMultiplierTimer).ToString() + "</b></color>";
            }
        }
        
        if (magnetActive)
        {
            magnetTimer -= Time.deltaTime;
            if (magnetTimer <= 0)
            {
                magnetActive = false;
                if (magnetText != null)
                    magnetText.gameObject.SetActive(false);
            }
            else if (magnetText != null)
            {
                magnetText.text = "<color=#FF8C00><b>MAGNET: " + Mathf.Ceil(magnetTimer).ToString() + "</b></color>";
            }
        }
        
        if (shieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                shieldActive = false;
                if (shieldText != null)
                    shieldText.gameObject.SetActive(false);
            }
            else if (shieldText != null)
            {
                shieldText.text = "<color=#00FFFF><b>SHIELD: " + Mathf.Ceil(shieldTimer).ToString() + "</b></color>";
            }
        }
    }
    
    public void ShowMainMenu()
    {
        gameStarted = false;
        gameOver = false;
        gamePaused = false;
        Time.timeScale = 1f;
        
        StartCoroutine(AnimateUITransition(mainMenuPanel, true));
        StartCoroutine(AnimateUITransition(gameOverPanel, false));
        StartCoroutine(AnimateUITransition(gameplayPanel, false));
        StartCoroutine(AnimateUITransition(pausePanel, false));
        
        if (startButton != null)
            startButton.gameObject.SetActive(true);
        
        ClearAllSlices();
        
        if (mainCamera != null)
            StartCoroutine(SmoothCameraTransition(new Vector3(0, 5, -8), 1f));
        
        LoadGameData();
        
        isRainbowMode = false;
        isNightMode = false;
        UpdateBackgroundTheme();
    }
    
    IEnumerator AnimateUITransition(GameObject panel, bool show)
    {
        if (panel == null) yield break;
        
        if (show)
        {
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero;
            
            float elapsed = 0f;
            while (elapsed < 0.3f)
            {
                float scale = Mathf.Lerp(0f, 1f, elapsed / 0.3f);
                panel.transform.localScale = Vector3.one * scale;
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            panel.transform.localScale = Vector3.one;
        }
        else
        {
            float elapsed = 0f;
            Vector3 startScale = panel.transform.localScale;
            
            while (elapsed < 0.2f)
            {
                float scale = Mathf.Lerp(1f, 0f, elapsed / 0.2f);
                panel.transform.localScale = startScale * scale;
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            panel.SetActive(false);
        }
    }
    
    public void StartGame()
    {
        gameStarted = true;
        gameOver = false;
        gamePaused = false;
        Time.timeScale = 1f;
        
        score = 0;
        combo = 0;
        perfectDrops = 0;
        level = 1;
        slicesStacked = 0;
        towerHeight = 0f;
        gameTime = 0f;
        streak = 0;
        health = maxHealth;
        moveSpeed = 3f;
        cameraShakeIntensity = 0f;
        
        ResetAllPowerUps();
        
        StartCoroutine(AnimateUITransition(mainMenuPanel, false));
        StartCoroutine(AnimateUITransition(gameplayPanel, true));
        StartCoroutine(AnimateUITransition(gameOverPanel, false));
        
        if (startButton != null)
            startButton.gameObject.SetActive(false);
        
        HideAllPowerUpTexts();
        ClearAllSlices();
        
        lastSliceSize = new Vector3(2.8f, 0.4f, 2.8f);
        lastSlicePosition = new Vector3(0, 0.25f, 0);
        originalCameraPos = new Vector3(0, 5, -8);
        
        UpdateUI();
        
        if (instructionText != null)
        {
            instructionText.text = "TAP TO DROP THE CAKE SLICE!\nAIM FOR PERFECT ALIGNMENT!";
            StartCoroutine(FadeOutInstruction());
        }
        
        SpawnNewSlice();
        
        if (PlayerPrefs.GetInt("HighScore", 0) > 1000)
            isRainbowMode = true;
        if (PlayerPrefs.GetInt("HighScore", 0) > 5000)
            isNightMode = true;
    }
    
    void ResetAllPowerUps()
    {
        slowMotionActive = false;
        sizeBoostActive = false;
        scoreMultiplierActive = false;
        magnetActive = false;
        shieldActive = false;
        
        slowMotionTimer = 0f;
        sizeBoostTimer = 0f;
        scoreMultiplierTimer = 0f;
        magnetTimer = 0f;
        shieldTimer = 0f;
        powerUpSpawnTimer = 0f;
    }
    
    IEnumerator FadeOutInstruction()
    {
        yield return new WaitForSeconds(3f);
        
        if (instructionText != null)
        {
            float elapsed = 0f;
            Color startColor = instructionText.color;
            
            while (elapsed < 1f)
            {
                Color newColor = startColor;
                newColor.a = Mathf.Lerp(1f, 0f, elapsed);
                instructionText.color = newColor;
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            instructionText.gameObject.SetActive(false);
        }
    }
    
    void HideAllPowerUpTexts()
    {
        if (slowMotionText != null) slowMotionText.gameObject.SetActive(false);
        if (sizeBoostText != null) sizeBoostText.gameObject.SetActive(false);
        if (scoreMultiplierText != null) scoreMultiplierText.gameObject.SetActive(false);
        if (magnetText != null) magnetText.gameObject.SetActive(false);
        if (shieldText != null) shieldText.gameObject.SetActive(false);
    }
    
    public void RestartGame()
    {
        StartGame();
    }
    
    void ClearAllSlices()
    {
        GameObject[] existingSlices = GameObject.FindGameObjectsWithTag("CakeSlice");
        if (existingSlices != null)
        {
            foreach (GameObject slice in existingSlices)
            {
                if (slice != null)
                    Destroy(slice);
            }
        }
        
        if (currentSlice != null)
        {
            Destroy(currentSlice);
            currentSlice = null;
        }
    }
    
    void SpawnNewSlice()
    {
        if (gameOver) return;
        
        Vector3 spawnPos = new Vector3(
            Random.Range(-5f, 5f), 
            lastSlicePosition.y + spawnHeight, 
            Random.Range(-1f, 1f)
        );
        
        GameObject prefabToUse = cakeSlicePrefab;
        if (cakeVariations != null && cakeVariations.Length > 0 && level > 3)
        {
            prefabToUse = cakeVariations[Random.Range(0, cakeVariations.Length)];
        }
        
        currentSlice = Instantiate(prefabToUse, spawnPos, Quaternion.identity);
        currentSlice.tag = "CakeSlice";
        
        Vector3 sliceSize = sizeBoostActive ? lastSliceSize * sizeBoostMultiplier : lastSliceSize;
        currentSlice.transform.localScale = sliceSize;
        
        ApplyCakeTheme(currentSlice);
        
        if (combo >= 3)
        {
            AddGlowEffect(currentSlice);
        }
        
        if (combo >= 10)
        {
            AddRainbowEffect(currentSlice);
        }
        
        if (combo >= 20)
        {
            AddCosmicEffect(currentSlice);
        }
        
        SetupSlicePhysics(currentSlice);
        
        isMoving = true;
        
        PlayRandomSFX(dropSounds, 0.3f, Random.Range(1.2f, 1.4f));
    }
    
    void ApplyCakeTheme(GameObject slice)
    {
        Renderer renderer = slice.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(renderer.material);
            
            CakeTheme selectedTheme;
            if (combo >= 25)
            {
                selectedTheme = cakeThemes[8]; // Cosmic Dark
            }
            else if (combo >= 15)
            {
                selectedTheme = cakeThemes[9]; // Fire Cake
            }
            else if (combo >= 10)
            {
                selectedTheme = new CakeTheme("Rainbow", 
                    Color.HSVToRGB(Random.value, 0.8f, 1f), 
                    Color.HSVToRGB(Random.value, 0.6f, 1f), "R");
            }
            else
            {
                selectedTheme = cakeThemes[Random.Range(0, 8)];
            }
            
            mat.color = selectedTheme.primaryColor;
            mat.SetFloat("_Glossiness", 0.8f);
            mat.SetFloat("_Metallic", 0.1f);
            
            if (combo >= 5)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", selectedTheme.secondaryColor * (0.3f + combo * 0.05f));
            }
            
            renderer.material = mat;
            
            AddThemeSymbol(slice, selectedTheme.symbol);
        }
    }
    
    void AddThemeSymbol(GameObject slice, string symbol)
    {
        GameObject symbolObj = new GameObject("ThemeSymbol");
        symbolObj.transform.SetParent(slice.transform);
        symbolObj.transform.localPosition = Vector3.up * 0.3f;
        symbolObj.transform.localRotation = Quaternion.Euler(90, 0, 0);
        
        TextMeshPro textMesh = symbolObj.AddComponent<TextMeshPro>();
        textMesh.text = symbol;
        textMesh.fontSize = 2f;
        textMesh.color = Color.white;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.fontStyle = FontStyles.Bold;
    }
    
    void SetupSlicePhysics(GameObject slice)
    {
        Rigidbody rb = slice.GetComponent<Rigidbody>();
        if (rb == null)
            rb = slice.AddComponent<Rigidbody>();
        
        rb.isKinematic = true;
        rb.mass = 1f;
        // FIXED: Changed linearDamping to drag (Unity 2022+ compatibility)
        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.5f;
    }
    
    void MoveSlice()
    {
        if (currentSlice == null) return;
        
        float effectiveSpeed = slowMotionActive ? moveSpeed * slowMotionScale : moveSpeed;
        
        float movement;
        if (level >= 10)
        {
            movement = Mathf.Sin(Time.time * effectiveSpeed) * 4f + 
                      Mathf.Sin(Time.time * effectiveSpeed * 1.5f) * 2f +
                      Mathf.Cos(Time.time * effectiveSpeed * 0.7f) * 1f;
        }
        else if (level >= 5)
        {
            movement = Mathf.Sin(Time.time * effectiveSpeed) * 4f + 
                      Mathf.Sin(Time.time * effectiveSpeed * 2f) * 1f;
        }
        else
        {
            movement = Mathf.Sin(Time.time * effectiveSpeed) * 4f;
        }
        
        if (magnetActive)
        {
            float targetX = lastSlicePosition.x;
            movement = Mathf.Lerp(movement, targetX, 0.3f);
        }
        
        Vector3 pos = currentSlice.transform.position;
        pos.x = movement;
        currentSlice.transform.position = pos;
        
        currentSlice.transform.Rotate(0, 30f * Time.deltaTime, 0);
    }
    
    void DropSlice()
    {
        if (currentSlice == null) return;
        
        isMoving = false;
        
        Rigidbody rb = currentSlice.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
        
        PlayRandomSFX(dropSounds, 0.7f, Random.Range(0.9f, 1.1f));
        
        StartCoroutine(ProcessSliceLanding());
    }
    
    IEnumerator ProcessSliceLanding()
    {
        yield return new WaitForSeconds(1f);
        
        if (currentSlice == null) yield break;
        
        Vector3 currentPos = currentSlice.transform.position;
        Vector3 offset = new Vector3(
            currentPos.x - lastSlicePosition.x,
            0,
            currentPos.z - lastSlicePosition.z
        );
        
        float offsetMagnitude = offset.magnitude;
        bool isPerfectDrop = offsetMagnitude <= perfectThreshold;
        
        Vector3 newSize = lastSliceSize;
        if (!isPerfectDrop && !shieldActive)
        {
            float sizePenalty = offsetMagnitude * 0.8f;
            newSize.x = Mathf.Max(0.2f, lastSliceSize.x - sizePenalty);
            newSize.z = Mathf.Max(0.2f, lastSliceSize.z - sizePenalty);
            
            health--;
            streak = 0;
        }
        else if (isPerfectDrop)
        {
            streak++;
            if (streak > maxStreak)
                maxStreak = streak;
        }
        
        if ((newSize.x < 0.6f || newSize.z < 0.6f) && health <= 0)
        {
            EndGame();
            yield break;
        }
        
        currentSlice.transform.localScale = newSize;
        currentSlice.transform.position = new Vector3(
            lastSlicePosition.x, 
            currentPos.y, 
            lastSlicePosition.z
        );
        
        Rigidbody rb = currentSlice.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
        
        lastSliceSize = newSize;
        lastSlicePosition = currentSlice.transform.position;
        slicesStacked++;
        towerHeight = lastSlicePosition.y;
        
        ProcessEnhancedScoring(isPerfectDrop, offsetMagnitude);
        
        if (isPerfectDrop)
        {
            CreateEnhancedPerfectEffect();
            AddCameraShake(0.15f);
            PlayRandomSFX(perfectSounds, 0.8f, 1.5f);
            
            StartCoroutine(ScreenFlash(Color.yellow, 0.1f));
        }
        else
        {
            PlayRandomSFX(dropSounds, 0.6f, 1f);
            
            if (!shieldActive)
            {
                AddCameraShake(0.3f);
                StartCoroutine(ScreenFlash(Color.red, 0.2f));
            }
        }
        
        CheckLevelUp();
        CheckAchievements();
        MoveCameraUp();
        
        moveSpeed += speedIncrease * difficultyMultiplier;
        
        currentSlice = null;
        
        yield return new WaitForSeconds(0.4f);
        
        if (!gameOver)
            SpawnNewSlice();
    }
    
    void ProcessEnhancedScoring(bool isPerfect, float accuracy)
    {
        int basePoints = 15;
        int bonusPoints = 0;
        
        if (isPerfect)
        {
            combo++;
            perfectDrops++;
            bonusPoints = combo * 8;
            
            if (combo >= 30) bonusPoints += 1000;
            else if (combo >= 25) bonusPoints += 750;
            else if (combo >= 20) bonusPoints += 500;
            else if (combo >= 15) bonusPoints += 300;
            else if (combo >= 10) bonusPoints += 150;
            else if (combo >= 5) bonusPoints += 50;
            
            bonusPoints += streak * 5;
        }
        else
        {
            combo = 0;
        }
        
        int levelMultiplier = 1 + (level - 1) / 2;
        int totalPoints = (basePoints + bonusPoints) * levelMultiplier;
        
        if (scoreMultiplierActive)
            totalPoints = Mathf.RoundToInt(totalPoints * scoreMultiplierValue);
        
        score += totalPoints;
        
        if (combo > bestCombo)
            bestCombo = combo;
        
        UpdateUI();
        ShowFloatingScore(totalPoints, isPerfect);
        
        if (combo >= 5 && comboSounds != null && comboSounds.Length > 0)
        {
            int soundIndex = Mathf.Min(combo / 5 - 1, comboSounds.Length - 1);
            PlaySFX(comboSounds[soundIndex], 0.8f, 1f);
        }
    }
    
    void ShowFloatingScore(int points, bool isPerfect)
    {
        GameObject floatingText = new GameObject("FloatingScore");
        floatingText.transform.position = currentSlice.transform.position + Vector3.up * 2f;
        
        TextMeshPro textMesh = floatingText.AddComponent<TextMeshPro>();
        textMesh.text = "+" + points.ToString();
        textMesh.fontSize = isPerfect ? 8f : 6f;
        textMesh.color = isPerfect ? Color.yellow : Color.white;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.fontStyle = FontStyles.Bold;
        
        StartCoroutine(AnimateFloatingText(floatingText));
    }
    
    IEnumerator AnimateFloatingText(GameObject textObj)
    {
        Vector3 startPos = textObj.transform.position;
        Vector3 endPos = startPos + Vector3.up * 3f;
        
        float elapsed = 0f;
        while (elapsed < 1.5f)
        {
            if (textObj != null)
            {
                textObj.transform.position = Vector3.Lerp(startPos, endPos, elapsed / 1.5f);
                
                TextMeshPro textMesh = textObj.GetComponent<TextMeshPro>();
                if (textMesh != null)
                {
                    Color color = textMesh.color;
                    color.a = Mathf.Lerp(1f, 0f, elapsed / 1.5f);
                    textMesh.color = color;
                }
            }
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        if (textObj != null)
            Destroy(textObj);
    }
    
    void CheckLevelUp()
    {
        int requiredSlices = level * 8;
        if (slicesStacked >= requiredSlices)
        {
            level++;
            
            if (levelUpSound != null)
                PlaySFX(levelUpSound, 0.8f, 1f);
            
            StartCoroutine(LevelUpEffect());
            
            if (level == 3) isRainbowMode = true;
            if (level == 5) ActivateScoreMultiplier();
            if (level == 7) isNightMode = true;
            if (level == 10) ActivateMagnet();
        }
    }
    
    void CheckAchievements()
    {
        if (!achievements[0] && slicesStacked >= 5)
            UnlockAchievement(0);
        
        if (!achievements[1] && combo >= 3)
            UnlockAchievement(1);
        
        if (!achievements[2] && combo >= 10)
            UnlockAchievement(2);
        
        if (!achievements[3] && towerHeight >= 20f)
            UnlockAchievement(3);
        
        if (!achievements[4] && moveSpeed >= 8f)
            UnlockAchievement(4);
        
        if (!achievements[5] && perfectDrops >= 20)
            UnlockAchievement(5);
        
        if (!achievements[6] && score >= 5000)
            UnlockAchievement(6);
        
        if (!achievements[7] && combo >= 25)
            UnlockAchievement(7);
        
        if (!achievements[8] && score >= 10000)
            UnlockAchievement(8);
        
        if (!achievements[9] && combo >= 50)
            UnlockAchievement(9);
    }
    
    void UnlockAchievement(int achievementIndex)
    {
        achievements[achievementIndex] = true;
        
        if (achievementSound != null)
            PlaySFX(achievementSound, 0.8f, 1f);
        
        StartCoroutine(ShowAchievementPopup(achievementNames[achievementIndex]));
    }
    
    IEnumerator ShowAchievementPopup(string achievementName)
    {
        if (achievementPopups != null && achievementPopups.Length > 0)
        {
            GameObject popup = achievementPopups[0];
            if (popup != null)
            {
                popup.SetActive(true);
                
                TextMeshProUGUI achievementText = popup.GetComponentInChildren<TextMeshProUGUI>();
                if (achievementText != null)
                    achievementText.text = "ACHIEVEMENT UNLOCKED!\n" + achievementName;
                
                yield return new WaitForSeconds(3f);
                popup.SetActive(false);
            }
        }
    }
    
    IEnumerator LevelUpEffect()
    {
        yield return StartCoroutine(ScreenFlash(Color.cyan, 0.3f));
        
        if (levelText != null)
        {
            levelText.text = "LEVEL " + level + " REACHED!";
            levelText.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(2f);
            
            levelText.gameObject.SetActive(false);
        }
    }
    
    IEnumerator ScreenFlash(Color flashColor, float duration)
    {
        if (gameplayPanel == null) yield break;
        
        GameObject flashPanel = new GameObject("ScreenFlash");
        flashPanel.transform.SetParent(gameplayPanel.transform);
        
        UnityEngine.UI.Image flashImage = flashPanel.AddComponent<UnityEngine.UI.Image>();
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        
        RectTransform rectTransform = flashPanel.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Sin((elapsed / duration) * Mathf.PI) * 0.3f;
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        if (flashPanel != null)
            Destroy(flashPanel);
    }
    
    void CreateEnhancedPerfectEffect()
    {
        if (perfectEffectPrefab != null && currentSlice != null)
        {
            Vector3 effectPos = currentSlice.transform.position + Vector3.up * 0.5f;
            GameObject effect = Instantiate(perfectEffectPrefab, effectPos, Quaternion.identity);
            Destroy(effect, 3f);
        }
        
        if (combo >= 10 && confettiPrefab != null)
        {
            Vector3 confettiPos = currentSlice.transform.position + Vector3.up * 2f;
            GameObject confetti = Instantiate(confettiPrefab, confettiPos, Quaternion.identity);
            Destroy(confetti, 5f);
        }
        
        if (combo >= 25 && specialEffectPrefabs != null && specialEffectPrefabs.Length > 0)
        {
            Vector3 specialPos = currentSlice.transform.position;
            GameObject specialEffect = Instantiate(specialEffectPrefabs[Random.Range(0, specialEffectPrefabs.Length)], specialPos, Quaternion.identity);
            Destroy(specialEffect, 10f);
        }
    }
    
    void AddGlowEffect(GameObject slice)
    {
        GameObject glow = Instantiate(slice);
        glow.transform.SetParent(slice.transform);
        glow.transform.localPosition = Vector3.zero;
        glow.transform.localScale = Vector3.one * 1.15f;
        
        Renderer glowRenderer = glow.GetComponent<Renderer>();
        if (glowRenderer != null)
        {
            Material glowMat = new Material(glowRenderer.material);
            glowMat.color = new Color(1f, 1f, 0f, 0.4f);
            glowMat.EnableKeyword("_EMISSION");
            glowMat.SetColor("_EmissionColor", Color.yellow * 0.5f);
            glowRenderer.material = glowMat;
        }
        
        Collider glowCollider = glow.GetComponent<Collider>();
        if (glowCollider != null) Destroy(glowCollider);
        
        Rigidbody glowRb = glow.GetComponent<Rigidbody>();
        if (glowRb != null) Destroy(glowRb);
    }
    
    void AddRainbowEffect(GameObject slice)
    {
        StartCoroutine(RainbowColorCycle(slice));
    }
    
    void AddCosmicEffect(GameObject slice)
    {
        StartCoroutine(CosmicEffect(slice));
    }
    
    IEnumerator RainbowColorCycle(GameObject slice)
    {
        Renderer renderer = slice.GetComponent<Renderer>();
        if (renderer == null) yield break;
        
        float hue = 0f;
        while (slice != null && combo >= 10)
        {
            hue += Time.deltaTime * 2f;
            if (hue > 1f) hue = 0f;
            
            Color rainbowColor = Color.HSVToRGB(hue, 0.8f, 1f);
            renderer.material.color = rainbowColor;
            renderer.material.SetColor("_EmissionColor", rainbowColor * 0.3f);
            
            yield return null;
        }
    }
    
    IEnumerator CosmicEffect(GameObject slice)
    {
        Renderer renderer = slice.GetComponent<Renderer>();
        if (renderer == null) yield break;
        
        float time = 0f;
        while (slice != null && combo >= 20)
        {
            time += Time.deltaTime;
            
            float pulse = Mathf.Sin(time * 5f) * 0.5f + 0.5f;
            Color cosmicColor = Color.Lerp(
                new Color(0.2f, 0.1f, 0.3f), 
                new Color(0.8f, 0.4f, 1f), 
                pulse
            );
            
            renderer.material.color = cosmicColor;
            renderer.material.SetColor("_EmissionColor", cosmicColor * pulse);
            
            yield return null;
        }
    }
    
    void AddCameraShake(float intensity)
    {
        cameraShakeIntensity = Mathf.Max(cameraShakeIntensity, intensity);
    }
    
    void MoveCameraUp()
    {
        if (mainCamera == null) return;
        
        originalCameraPos = new Vector3(
            originalCameraPos.x,
            lastSlicePosition.y + 5f,
            originalCameraPos.z
        );
        
        StartCoroutine(SmoothCameraMove());
    }
    
    IEnumerator SmoothCameraMove()
    {
        if (mainCamera == null) yield break;
        
        Vector3 startPos = mainCamera.transform.position;
        float elapsed = 0f;
        float duration = 0.6f;
        
        while (elapsed < duration)
        {
            Vector3 targetPos = originalCameraPos;
            if (cameraShakeIntensity <= 0.01f)
            {
                mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        if (cameraShakeIntensity <= 0.01f)
            mainCamera.transform.position = originalCameraPos;
    }
    
    IEnumerator SmoothCameraTransition(Vector3 targetPos, float duration)
    {
        if (mainCamera == null) yield break;
        
        Vector3 startPos = mainCamera.transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        mainCamera.transform.position = targetPos;
        originalCameraPos = targetPos;
    }
    
    void PlaySFX(AudioClip clip, float volume, float pitch)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
        }
    }
    
    void PlayRandomSFX(AudioClip[] clips, float volume, float pitch)
    {
        if (clips != null && clips.Length > 0)
        {
            AudioClip randomClip = clips[Random.Range(0, clips.Length)];
            PlaySFX(randomClip, volume, pitch);
        }
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + score.ToString("N0");
            
        if (comboText != null)
        {
            if (combo > 0)
            {
                string comboPrefix = combo >= 25 ? "COSMIC" : combo >= 15 ? "FIRE" : combo >= 10 ? "MEGA" : combo >= 5 ? "SUPER" : "NICE";
                comboText.text = comboPrefix + " COMBO x" + combo;
                comboText.color = combo >= 25 ? Color.magenta : combo >= 15 ? Color.red : combo >= 10 ? new Color(1f, 0.5f, 0f) : combo >= 5 ? Color.yellow : Color.cyan;
                comboText.gameObject.SetActive(true);
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }
        }
        
        if (heightText != null)
            heightText.text = "HEIGHT: " + towerHeight.ToString("F1") + "m";
        
        if (streakText != null)
        {
            if (streak > 0)
            {
                streakText.text = "STREAK: " + streak;
                streakText.gameObject.SetActive(true);
            }
            else
            {
                streakText.gameObject.SetActive(false);
            }
        }
        
        if (levelText != null && gameStarted)
            levelText.text = "LEVEL " + level;
    }
    
    void UpdateBackgroundTheme()
    {
        if (backgroundImage != null)
        {
            if (isRainbowMode)
            {
                backgroundImage.color = Color.HSVToRGB(backgroundHue, 0.3f, 0.9f);
            }
            else if (isNightMode)
            {
                backgroundImage.color = new Color(0.05f, 0.05f, 0.15f);
            }
            else
            {
                Color baseColor = level >= 10 ? new Color(0.1f, 0.1f, 0.1f) : 
                                 level >= 5 ? new Color(0.1f, 0.1f, 0.3f) : 
                                 new Color(0.2f, 0.3f, 0.5f);
                backgroundImage.color = baseColor;
            }
        }
        
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.01f;
    }
    
    void EndGame()
    {
        gameOver = true;
        Time.timeScale = 1f;
        
        totalPerfectDrops += perfectDrops;
        
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        bool isNewRecord = score > savedHighScore;
        
        if (isNewRecord)
        {
            PlayerPrefs.SetInt("HighScore", score);
            savedHighScore = score;
        }
        
        PlayerPrefs.SetInt("TotalPerfectDrops", totalPerfectDrops);
        PlayerPrefs.SetInt("BestCombo", Mathf.Max(PlayerPrefs.GetInt("BestCombo", 0), bestCombo));
        PlayerPrefs.SetInt("MaxStreak", maxStreak);
        PlayerPrefs.Save();
        
        UpdateGameOverUI(isNewRecord, savedHighScore);
        
        StartCoroutine(AnimateUITransition(gameplayPanel, false));
        StartCoroutine(AnimateUITransition(gameOverPanel, true));
        
        PlaySFX(gameOverSound, 0.8f, 0.8f);
        
        StartCoroutine(AnimateGameOverUI());
    }

void UpdateGameOverUI(bool isNewRecord, int savedHighScore)
{
    if (gameOverTitle != null)
    {
        gameOverTitle.text = "<color=#FF4444><size=80><b>GAME OVER</b></size></color>";
    }
    
    if (finalScoreText != null)
    {
        finalScoreText.text = "<color=#FFFFFF><b>Final Score</b></color>\n<color=#FFD700><size=60>" + score.ToString("N0") + "</size></color>";
    }
    
    if (highScoreText != null)
    {
        // FIXED: Removed emoji, replaced with text
        highScoreText.text = "<color=#FFD700><b>High Score: " + savedHighScore.ToString("N0") + "</b></color>";
    }
    
    if (perfectDropsText != null)
    {
        perfectDropsText.text = "<color=#87CEEB>Perfect Drops: <b>" + perfectDrops + "</b></color>";
    }
    
    if (bestComboText != null)
    {
        bestComboText.text = "<color=#FFFF00>Best Combo: <b>" + bestCombo + "x</b></color>";
    }
    
    if (heightAchievedText != null)
    {
        // FIXED: Removed emoji
        heightAchievedText.text = "<color=#90EE90>Tower Height: <b>" + towerHeight.ToString("F1") + "m</b></color>";
    }
    
    if (newRecordText != null)
    {
        if (isNewRecord)
        {
            // FIXED: Removed emojis
            newRecordText.text = "<color=#FF69B4><size=48><b>NEW HIGH SCORE!</b></size></color>";
            newRecordText.gameObject.SetActive(true);
        }
        else
        {
            newRecordText.gameObject.SetActive(false);
        }
    }
}

    
    void UpdateStarRating()
    {
        if (stars == null || stars.Length == 0) return;
        
        int starCount = 0;
        if (score >= 5000) starCount = 3;
        else if (score >= 2000) starCount = 2;
        else if (score >= 1000) starCount = 1;
        
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
                stars[i].SetActive(i < starCount);
        }
    }
    
    IEnumerator AnimateGameOverUI()
    {
        // Start with elements scaled down
        TextMeshProUGUI[] elementsToAnimate = {
            gameOverTitle,
            finalScoreText,
            highScoreText,
            perfectDropsText,
            bestComboText,
            heightAchievedText,
            timePlayedText
        };
        
        foreach (TextMeshProUGUI element in elementsToAnimate)
        {
            if (element != null)
                element.transform.localScale = Vector3.zero;
        }
        
        // Animate each element with delay
        foreach (TextMeshProUGUI element in elementsToAnimate)
        {
            if (element != null)
            {
                StartCoroutine(ScaleUpElement(element.transform, 0.4f));
                yield return new WaitForSeconds(0.2f);
            }
        }
        
        // Animate stars
        if (stars != null)
        {
            foreach (GameObject star in stars)
            {
                if (star != null && star.activeInHierarchy)
                {
                    StartCoroutine(ScaleUpElement(star.transform, 0.3f));
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        
        // Show new record text with special animation
        if (newRecordText != null && newRecordText.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(PulseElement(newRecordText.transform, 3f));
        }
    }
    
    IEnumerator ScaleUpElement(Transform element, float duration)
    {
        if (element == null) yield break;
        
        float elapsed = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            element.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        element.localScale = endScale;
    }
    
    IEnumerator PulseElement(Transform element, float duration)
    {
        if (element == null) yield break;
        
        float elapsed = 0f;
        Vector3 originalScale = element.localScale;
        
        while (elapsed < duration)
        {
            float scale = 1f + Mathf.Sin(elapsed * 6f) * 0.2f;
            element.localScale = originalScale * scale;
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        element.localScale = originalScale;
    }
    
    // Enhanced Power-up activation methods
    public void ActivateSlowMotion()
    {
        slowMotionActive = true;
        slowMotionTimer = slowMotionDuration;
        if (slowMotionText != null)
        {
            slowMotionText.gameObject.SetActive(true);
            slowMotionText.text = "<color=#4169E1><b>SLOW TIME: " + Mathf.Ceil(slowMotionTimer).ToString() + "</b></color>";
        }
    }
    
    public void ActivateSizeBoost()
    {
        sizeBoostActive = true;
        sizeBoostTimer = 6f;
        if (sizeBoostText != null)
        {
            sizeBoostText.gameObject.SetActive(true);
            sizeBoostText.text = "<color=#32CD32><b>BIG SIZE: " + Mathf.Ceil(sizeBoostTimer).ToString() + "</b></color>";
        }
    }
    
    public void ActivateScoreMultiplier()
    {
        scoreMultiplierActive = true;
        scoreMultiplierTimer = 12f;
        if (scoreMultiplierText != null)
        {
            scoreMultiplierText.gameObject.SetActive(true);
            scoreMultiplierText.text = "<color=#FF1493><b>BONUS x" + scoreMultiplierValue + ": " + Mathf.Ceil(scoreMultiplierTimer).ToString() + "</b></color>";
        }
    }
    
    public void ActivateMagnet()
    {
        magnetActive = true;
        magnetTimer = magnetDuration;
        if (magnetText != null)
        {
            magnetText.gameObject.SetActive(true);
            magnetText.text = "<color=#FF8C00><b>MAGNET: " + Mathf.Ceil(magnetTimer).ToString() + "</b></color>";
        }
    }
    
    public void ActivateShield()
    {
        shieldActive = true;
        shieldTimer = shieldDuration;
        if (shieldText != null)
        {
            shieldText.gameObject.SetActive(true);
            shieldText.text = "<color=#00FFFF><b>SHIELD: " + Mathf.Ceil(shieldTimer).ToString() + "</b></color>";
        }
    }
    
    // Additional missing method
    // void ShareScore()
    // {
    //     // Simple share functionality - can be expanded
    //     string shareText = "I just scored " + score.ToString("N0") + " points in Cake Tower! Can you beat my score?";
    //     Debug.Log("Sharing: " + shareText);
        
    //     // For mobile platforms, you could integrate with native sharing
    //     #if UNITY_ANDROID || UNITY_IOS
    //     // Native sharing implementation would go here
    //     #endif
    // }
}

