using System;
using System.Collections.Generic;
using System.Text;
using TinyOthello.Kernel;
using System.Threading;

namespace TinyOthello.GraphicUI {
    public class GraphicHumanPlayer : IPlayer {

        public GraphicHumanPlayer(Color color, MainWindow mainWindow) {
            this.color = color;
            mainWindow.MovePlayed += new MainWindow.PlayMoveHandler(MainWindow_MovePlayed);
        }

        private void MainWindow_MovePlayed(Color color, int x, int y) {
            lock (this) {
                if (color != this.color) return;
                this.x = x;
                this.y = y;
                played = true;
                Monitor.PulseAll(this);
            }
        }

        #region IPlayer Members

        public void PlayOneMove(Board board) {
            lock (this) {
                played = false;
                while (!played) {
                    Monitor.Wait(this);
                }
                if (x != -1)
                    board.PutStone(x, y);
                else 
                    board.Pass();
            }
        }

        public Color Color {
            get {
                return color;
            }
        }

        #endregion

        private Color color;
        private int x, y;
        private volatile bool played;
    }
}
