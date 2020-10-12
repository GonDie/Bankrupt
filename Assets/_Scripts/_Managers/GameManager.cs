using Bankrupt.Managers;
using Bankrupt.Player;
using Bankrupt.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public delegate void SimpleEvent();
    public delegate void StringEvent(string s);
    public delegate void StringIntEvent(string s, int i);

    public event SimpleEvent OnEndGame;
    public event StringIntEvent OnPayRent;
    public event StringEvent OnPlayerLose;

    public int maxGames = 300;
    public int maxTurns = 1000;

    public List<Player> players;

    public Text gamesElapsedText, turnsElapsedText, logText;

    int _gamesElapsed, _turnsElapsed;

    Dictionary<string, float> _log = new Dictionary<string, float>()
    {
        { "Timeout", 0f },
        { "AverageTurns", 0f },
        { "impulsivo", 0f },
        { "exigente", 0f },
        { "cauteloso", 0f },
        { "aleatorio", 0f }
    };

    protected override void Start()
    {
        base.Start();
        
        BoardManager.Instance.BuildBoard(ReadConfigFile());

        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        gamesElapsedText.text = (_gamesElapsed + 1).ToString();
        turnsElapsedText.text = (_turnsElapsed + 1).ToString();

        logText.text = "<b>Timeouts:</b> " + _log["Timeout"] + "\n";
        logText.text += "<b>Média de Turnos:</b> " + (_log["AverageTurns"] / _gamesElapsed) + "\n\n";
        logText.text += "<b>Player Impulsivo:</b> " + _log["impulsivo"] + " (" + ((_log["impulsivo"] / _gamesElapsed) * 100f).ToString("00") + "%)\n";
        logText.text += "<b>Player Exigente:</b> " + _log["exigente"] + " (" + ((_log["exigente"] / _gamesElapsed) * 100f).ToString("00") + "%)\n";
        logText.text += "<b>Player Cauteloso:</b> " + _log["cauteloso"] + " (" + ((_log["cauteloso"] / _gamesElapsed) * 100f).ToString("00") + "%)\n";
        logText.text += "<b>Player Aleatório:</b> " + _log["aleatorio"] + " (" + ((_log["aleatorio"] / _gamesElapsed) * 100f).ToString("00") + "%)\n";
    }

    IEnumerator GameLoop()
    {
        players = players.OrderBy(x => UnityEngine.Random.value).ToList();

        yield return new WaitForSeconds(1f);

        int rand = 0;

        while(_turnsElapsed < maxTurns)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].gameObject.activeSelf)
                {
                    rand = UnityEngine.Random.Range(1, 7);
                    players[i].MoveToTile(rand);

                    yield return new WaitForSeconds(0.5f * rand);
                }
            }

            _turnsElapsed++;
        }

        // Timeout
        _log["Timeout"] += 1f;

        List<Player> playerOrder = new List<Player>(players);
        playerOrder = playerOrder.OrderByDescending(x => x.Coins).ToList();

        _log[playerOrder[0].playerName] += 1;

        GameReset();
    }

    public void PlayerLose(string playerName)
    {
        if (OnPlayerLose != null)
            OnPlayerLose(playerName);

        List<Player> playersLeft = players.FindAll(x => x.gameObject.activeSelf);

        if(playersLeft.Count == 1)
        {
            // Abate
            _log[playersLeft[0].playerName] += 1;

            GameReset();
        }
    }

    void GameReset()
    {
        StopAllCoroutines();

        if (OnEndGame != null)
            OnEndGame();

        _log["AverageTurns"] += _turnsElapsed;

        _turnsElapsed = 0;

        if (_gamesElapsed < maxGames - 1)
        {
            _gamesElapsed++;
            StartCoroutine(GameLoop());
        }
    }

    public void PayRent(string toPlayer, int coins)
    {
        if (OnPayRent != null)
            OnPayRent(toPlayer, coins);
        //players.Find(x => string.Equals(x.playerName, toPlayer)).AddCoins(coins);
    }

    List<Vector2> ReadConfigFile()
    {
        List<Vector2> config = new List<Vector2>();

        TextAsset textAsset = Resources.Load<TextAsset>("gameConfig");
        string[] lines = Regex.Split(textAsset.text, "\n|\r|\r\n");
        string[] configEntry;

        for(int i = 0; i < lines.Length; i++)
        {
            string[] split = new string[2] { " ", "  " };
            configEntry = lines[i].Split(split, StringSplitOptions.RemoveEmptyEntries);
            config.Add(new Vector2(int.Parse(configEntry[0].Trim()), int.Parse(configEntry[1].Trim())));
        }

        return config;
    }
}