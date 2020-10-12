using Bankrupt.Managers;
using Bankrupt.Player.Behaviour;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bankrupt.Player
{
    public class Player : MonoBehaviour
    {
        Transform _transform;

        public Color playerColor = Color.white;
        public string playerName;
        public Vector2 positionOffset;

        public ScriptableObject behaviour;
        public Text coinsText;

        int _coins = 300;
        public int Coins { get { return _coins; } }

        int _currentTile = -1;
        public int CurrentTile { get { return _currentTile; } }

        void Start()
        {
            _transform = GetComponent<Transform>();

            GameManager.Instance.OnPayRent += OnReceiveRent;
            GameManager.Instance.OnEndGame += Reset;

            Reset();
        }

        void OnReceiveRent(string toPlayer, int coins)
        {
            if (string.Equals(toPlayer, playerName))
                AddCoins(coins);
        }

        public void AddCoins(int coins)
        {
            _coins += coins;
            coinsText.text = _coins.ToString();
        }

        public void RemoveCoins(int coins)
        {
            _coins -= coins;
            coinsText.text = _coins.ToString();

            if (_coins < 0)
            {
                gameObject.SetActive(false);
                GameManager.Instance.PlayerLose(playerName);
            }
        }

        public void MoveToTile(int advanceTiles)
        {
            int targetTile = _currentTile + advanceTiles;

            if (targetTile >= BoardManager.Instance.Tiles.Count)
                targetTile -= BoardManager.Instance.Tiles.Count;

            Sequence seq = DOTween.Sequence();
            seq.OnComplete(() => ((IBehaviour)behaviour).Act(this));

            while (_currentTile != targetTile)
            {
                _currentTile++;
                if (_currentTile >= BoardManager.Instance.Tiles.Count)
                {
                    AddCoins(100);
                    _currentTile = 0;
                }

                seq.Append(_transform.DOMove(BoardManager.Instance.Tiles[_currentTile].Position + positionOffset, 0.5f));
            }

            seq.Play();
        }

        private void Reset()
        {
            gameObject.SetActive(true);

            _transform.position = (Vector2)BoardManager.Instance.startTile.position + positionOffset;

            _currentTile = -1;

            _coins = 300;
            coinsText.text = _coins.ToString();
        }
    }
}