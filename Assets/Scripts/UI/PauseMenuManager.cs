using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backButton;

    private PlayerController _localPlayerController;
    private InputSystem _controls;
    private bool _isPaused = false;
    
    // В Awake создаем новую систему ввода специально для этого скрипта.
    private void Awake()
    {
        _controls = new InputSystem();
        _controls.UI.Pause.performed += _ => TogglePauseMenu();
        _controls.UI.Enable();

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (optionsButton != null)
            optionsButton.onClick.AddListener(OpenOptions);

        if (backButton != null)
            backButton.onClick.AddListener(CloseOptions);
        
        //if (exitButton != null)
        //    exitButton.onClick.AddListener(ExitGame);

        // Изначально скрываем UI панели
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
    }
    
    // В OnEnable/OnDisable управляем своей системой ввода.
    private void OnEnable()
    {
        _controls.UI.Enable();
    }
    
    private void OnDisable()
    {
        _controls.UI.Disable();
    }
    
    // Этот метод будет вызываться, чтобы найти и сохранить ссылку на локального игрока.
    private void FindLocalPlayer()
    {
        if (_localPlayerController != null) return;
        
        // Ищем в сцене все NetworkIdentity
        foreach (var playerIdentity in FindObjectsOfType<NetworkIdentity>())
        {
            // Если это локальный игрок, получаем его PlayerController.
            if (playerIdentity.isLocalPlayer)
            {
                _localPlayerController = playerIdentity.GetComponent<PlayerController>();
                if (_localPlayerController != null)
                {
                    Debug.Log("Локальный игрок найден.");
                    return;
                }
            }
        }
    }

    private void TogglePauseMenu()
    {
        FindLocalPlayer(); // Всегда пытаемся найти локального игрока перед использованием.
        if (_localPlayerController == null)
        {
            Debug.LogWarning("Локальный игрок не найден. Не могу поставить на паузу.");
            return;
        }

        if (_isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    private void PauseGame()
    {
        _isPaused = true;
        
        // Отключаем управление игроком, если он найден.
        _localPlayerController?.SetInputActive(false);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        Debug.Log("Игра на паузе.");
    }
    
    private void ResumeGame()
    {
        _isPaused = false;
        
        // Включаем управление игроком.
        _localPlayerController?.SetInputActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Debug.Log("Игра возобновлена.");
    }
    
    public void OpenOptions()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
        Debug.Log("Options Opened");
    }

    public void CloseOptions()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        Debug.Log("Options Closed");
    }
    
    /*private void ExitGame()
    {
        // Логика выхода остаётся прежней.
        if (NetworkManager.singleton.isServer)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }*/
}