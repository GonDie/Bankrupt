using Bankrupt.Board;
using Bankrupt.Managers;
using Bankrupt.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehaviour : ScriptableObject
{
    protected virtual void BuyTile(Player player, Tile tile)
    {
        player.RemoveCoins(tile.Values.buyPrice);
        BoardManager.Instance.BuyTile(player.CurrentTile, player.playerName, player.playerColor);
    }

    protected virtual void PayRent(Player player, Tile tile)
    {
        if (!string.Equals(tile.Values.ownedBy, player.playerName))
        {
            player.RemoveCoins(tile.Values.rentPrice);
            GameManager.Instance.PayRent(tile.Values.ownedBy, tile.Values.rentPrice);
        }
    }
}