using Newtonsoft.Json;
using UnityEngine;
using Zork.Common;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LocationText;
    [SerializeField]
    private TextMeshProUGUI ScoreText;
    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private UnityInputService Input;
    [SerializeField]
    private UnityOutputService Output;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);

        _game.Run(Input, Output);

    }

    private Game _game;
}
