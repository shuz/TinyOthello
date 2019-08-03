using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TinyOthello.Kernel {
    public class Game {
        public Game(IPlayer player1, IPlayer player2, IBoardViewer viewer) {
            this.player1 = player1;
            this.player2 = player2;
            this.viewer = viewer;
            Debug.Assert(player1.Color != player2.Color);
        }

        public int MainLoop(Board board) {
            Debug.Assert(!played);
            played = true;
            
            while (!board.IsEndOfGame()) {
                viewer.ShowBoard(board);
                IPlayer player;
                lock (this) {
                    player = GetPlayer(board.CurrentColor);
                }
                player.PlayOneMove(board);
            }
            return board.BlackScore - board.WhiteScore;
        }

        public IPlayer GetPlayer(Color color) {
            if (color == player1.Color)
                return player1;
            return player2;
        }

        public void SetPlayer(Color color, IPlayer player) {
            lock (this) {
                if (color == player1.Color) {
                    player1 = player;
                } else {
                    player2 = player;
                }
            }
        }

        private IPlayer player1, player2;
        private IBoardViewer viewer;
        private bool played;
    }
}
