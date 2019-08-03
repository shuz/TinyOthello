using System;
using System.Collections.Generic;
using System.Text;
using TinyOthello.Kernel;

namespace TinyOthello.Console {
    public class ConsoleBoardViewer : IBoardViewer {

        public void ShowBoard(Board board) {
            System.Console.WriteLine("  a b c d e f g h");
            for (int i = 0; i < 8; ++i) {
                System.Console.Write((i + 1) + " ");
                for (int j = 0; j < 8; ++j) {
                    string ch = ". ";
                    switch (board[i, j]) {
                        case Color.Black:
                            ch = "x ";
                            break;
                        case Color.White:
                            ch = "o ";
                            break;
                    }
                    if (ch == ". " && board.IsLegalMove(i, j))
                        ch = "_ ";
                    System.Console.Write(ch);
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine("Current move: " + (board.CurrentColor == Color.Black ? "x" : "o"));
            System.Console.WriteLine("Current step: " + (board.CurrentStep + 1));
            System.Console.WriteLine("Current status: x {0}  o {1}", board.BlackScore, board.WhiteScore);
            if (board.LastMove != null) {
                if (board.LastMove.X != -1) {
                    char y = (char)(board.LastMove.Y + 'a');
                    char x = (char)(board.LastMove.X + '1');
                    System.Console.WriteLine("Last move position: {0}{1}", y, x);
                } else {
                    System.Console.WriteLine("Last move: pass");
                }
            }
        }
    }
}
