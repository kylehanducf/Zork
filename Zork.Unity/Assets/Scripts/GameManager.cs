using Newtonsoft.Json;
using UnityEngine;
using Zork.Common;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LocationText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private UnityInputService InputService;

    [SerializeField]
    private UnityOutputService OutputService;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Player.LocationChanged += Player_LocationChanged;
        _game.Player.ScoreChanged += Player_ScoreChanged;
        _game.Player.MovesChanged += Player_MovesChanged;
        _game.Run(InputService, OutputService);

    }

    public void Player_LocationChanged(object sender, Room location)
    {
        LocationText.text = location.Name;
    }

    public void Player_ScoreChanged(object sender, int score)
    {
        ScoreText.text = $"Score: {score}";
    }

    public void Player_MovesChanged(object sender, int moves)
    {
        MovesText.text = $"Moves: {moves}";
    }

    public void Start()
    {
        InputService.SetFocus();
        LocationText.text = _game.Player.CurrentRoom.Name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputService.ProcessInput();
            InputService.SetFocus();
        }

        if (_game.IsRunning == false)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private Game _game;
}
