using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using TinyOthello.Kernel;

namespace TinyOthello.Console {
    public class ConsoleHumanPlayer : IPlayer {
        public ConsoleHumanPlayer(Color color) {
            this.color = color;
        }

        public void PlayOneMove(Board board) {
            Debug.Assert(board.CurrentColor == color);
            try {

                String str = System.Console.ReadLine();
                switch (str[0]) {
                    case 'p': {
                            if (str == "pass")
                                board.Pass();
                            else {
                                int x = str[2] - '1';
                                int y = str[1] - 'a';
                                if (!board.IsInBoard(x, y) || !board.IsLegalMove(x, y)) {
                                    System.Console.WriteLine("Illegal move.");
                                    break;
                                } else
                                    board.PutStone(x, y);
                            }
                        } break;
                    case 'u': {
                            int n = 1;
                            if (str.Length > 1)
                                n = Int32.Parse(str.Substring(1));
                            if (board.UndoCount >= n)
                                board.Undo(n);
                            else
                                System.Console.WriteLine("Undo too many moves");
                        } break;
                    case 'r': {
                            int n = 1;
                            if (str.Length > 1)
                                n = Int32.Parse(str.Substring(1));
                            if (board.RedoCount >= n)
                                board.Redo(n);
                            else
                                System.Console.WriteLine("Redo too many moves");
                        } break;
                    case 'c': {
                            board.CutHistory();
                        } break;
                    default: {
                            System.Console.WriteLine("Command error");
                            break;
                        }
                }
            } catch (Exception) {
                System.Console.WriteLine("Command error");
            }
        }

        public Color Color {
            get { return color; }
        }

        private Color color;
    }
}
