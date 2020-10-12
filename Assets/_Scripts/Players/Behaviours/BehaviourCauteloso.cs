using Bankrupt.Board;
using Bankrupt.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bankrupt.Player.Behaviour
{
    public class BehaviourCauteloso : BaseBehaviour, IBehaviour
    {
        public void Act(Player player)
        {
            Tile tile = BoardManager.Instance.Tiles[player.CurrentTile];

            if (!tile.Values.isBought)
            {
                if (player.Coins >= tile.Values.buyPrice && player.Coins - tile.Values.buyPrice >= 80)
                {
                    BuyTile(player, tile);
                }
            }
            else
                PayRent(player, tile);
        }
    }
}