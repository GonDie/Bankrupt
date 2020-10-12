using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bankrupt.Board
{
    public class Tile : MonoBehaviour
    {
        public Color defaultTileColor = Color.white;
        public Color evenTileColor = Color.gray;
        public Color availableTextColor = Color.green;
        public Color boughtTextColor = Color.red;

        Transform _transform;
        Image _image;
        Text _text;

        TileValues _values;
        public TileValues Values { get { return _values; } }
        public Vector2 Position { get { return _transform.position; } }

        void Init()
        {
            _transform = GetComponent<Transform>();
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<Text>();

            GameManager.Instance.OnPlayerLose += OnPlayerLose;
            GameManager.Instance.OnEndGame += ResetTile;
        }

        public void SetValues(TileValues values)
        {
            Init();

            _values = values;

            ResetTile();
        }

        public int BuyTile(string playerName, Color color)
        {
            _image.color = color;

            _text.text = _values.rentPrice.ToString();
            _text.color = boughtTextColor;

            _values.ownedBy = playerName;
            _values.isBought = true;

            return _values.buyPrice;
        }

        public int RentTile()
        {
            return _values.rentPrice;
        }

        void OnPlayerLose(string playerName)
        {
            if (string.Equals(Values.ownedBy, playerName))
                ResetTile();
        }

        void ResetTile()
        {
            _values.ownedBy = "";
            _values.isBought = false;

            _image.color = _values.isEven ? evenTileColor : defaultTileColor;

            _text.text = _values.buyPrice.ToString();
            _text.color = availableTextColor;
        }
    }

    public class TileValues
    {
        public int buyPrice, rentPrice;
        public string ownedBy = "";
        public bool isBought = false;
        public bool isEven;
    }
}