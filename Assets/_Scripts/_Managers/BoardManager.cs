using Bankrupt.Board;
using Bankrupt.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bankrupt.Managers
{
    public class BoardManager : Singleton<BoardManager>
    {
        public GameObject tilePrefab;
        public Transform boardContainer;
        public Transform startTile;

        List<Tile> _tiles;
        public List<Tile> Tiles { get { return _tiles; } }

        int _boardSize = 5;
        Vector2[] _boardDirections = new Vector2[4] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

        public void BuildBoard(List<Vector2> config)
        {
            int count = 0;
            _tiles = new List<Tile>();

            Vector2 tilePos = (Vector2)startTile.position + (_boardDirections[0] * (tilePrefab.GetComponent<RectTransform>().sizeDelta.x - 5f));

            for (int i = 0; i < _boardDirections.Length; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    Tile tile = Instantiate(tilePrefab, tilePos, Quaternion.identity, boardContainer).GetComponent<Tile>();
                    tile.SetValues(new TileValues()
                    {
                        buyPrice = (int)config[count].x,
                        rentPrice = (int)config[count].y,
                        isEven = count % 2 != 0
                    });

                    _tiles.Add(tile);

                    tilePos += _boardDirections[i] * (tilePrefab.GetComponent<RectTransform>().sizeDelta.x - 5f);

            count++;
                }
            }
        }

        public void BuyTile(int tileIndex, string playerName, Color color)
        {
            Tiles[tileIndex].BuyTile(playerName, color);
        }
    }
}