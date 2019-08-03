using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TinyOthello.Console;
using System.Diagnostics;

namespace TinyOthello.Kernel {
    public class AITest {

        private static int Test(IPlayer p1, IPlayer p2) {
            Debug.Assert(p1.Color == Color.Black && p2.Color == Color.White);

            IBoardViewer viewer = new ConsoleBoardViewer();

            Game game = new Game(p1, p2, viewer);
            Board board = new Board();
            int result = game.MainLoop(board);
            string msg = (result == 0 ? "draw" : "winner: " + (result > 0 ? "black" : "white"));
            System.Console.WriteLine(msg);
            System.Console.WriteLine("black {0}", p1);
            System.Console.WriteLine("white {0}", p2);

            return result;
        }

        public static void MainLoop() {
            const int maxdepth = 4;
            const int maxconsider = 2000;
            const int ainum = 5;

            IAIPlayer[] bplayers = new IAIPlayer[ainum];
            bplayers[0] = new AIPlayer1(Color.Black, maxdepth);
            bplayers[1] = new AIPlayer2(Color.Black, maxconsider);
            bplayers[2] = new AIPlayer3(Color.Black, maxconsider);
            bplayers[3] = new AIPlayer4(Color.Black, maxconsider);
            bplayers[4] = new AIPlayer5(Color.Black, maxconsider);

            IAIPlayer[] wplayers = new IAIPlayer[ainum];
            wplayers[0] = new AIPlayer1(Color.White, maxdepth);
            wplayers[1] = new AIPlayer2(Color.White, maxconsider);
            wplayers[2] = new AIPlayer3(Color.White, maxconsider);
            wplayers[3] = new AIPlayer4(Color.White, maxconsider);
            wplayers[4] = new AIPlayer5(Color.White, maxconsider);

            int[,] bdepths = new int[ainum, ainum];
            int[,] wdepths = new int[ainum, ainum];
            int[,] bmaxconsiders = new int[ainum, ainum];
            int[,] wmaxconsiders = new int[ainum, ainum];
            int[,] winlose = new int[ainum, ainum];

            int[] win = new int[ainum];
            int[] lose = new int[ainum];
            int[] draw = new int[ainum];

            for (int i = 0; i < ainum; ++i) {
                for (int j = 0; j < ainum; ++j) {
                    int result = Test(bplayers[i], wplayers[j]);
                    if (i != j) {
                        if (result > 0) {
                            win[i]++; lose[j]++;
                        } else
                        if (result < 0) {
                            win[j]++; lose[i]++;
                        } else {
                            draw[i]++; draw[j]++;
                        }
                    }

                    winlose[i, j] = result;
                    bdepths[i, j] = bplayers[i].TotalDepth;
                    wdepths[i, j] = wplayers[j].TotalDepth;
                    bmaxconsiders[i, j] = bplayers[i].MovesConsidered;
                    wmaxconsiders[i, j] = wplayers[j].MovesConsidered;

                    bplayers[i].Reset();
                    wplayers[j].Reset();

                    System.GC.Collect();
                }
            }

            for (int i = 0; i < ainum; ++i) {
                System.Console.WriteLine("Player {0} win: {1}, lose {2}, draw {3}", i+1, win[i], lose[i], draw[i]);
            }
            System.Console.WriteLine();

            System.Console.WriteLine("Win Lose Table");
            for (int i = 0; i < ainum; ++i) {
                for (int j = 0; j < ainum; ++j) {
                    System.Console.Write(winlose[i, j] + "\t");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();

            System.Console.WriteLine("Black Depth Table");
            for (int i = 0; i < ainum; ++i) {
                for (int j = 0; j < ainum; ++j) {
                    System.Console.Write(bdepths[i, j] + "\t");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();

            System.Console.WriteLine("White Depth Table");
            for (int i = 0; i < ainum; ++i) {
                for (int j = 0; j < ainum; ++j) {
                    System.Console.Write(wdepths[i, j] + "\t");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();

            System.Console.WriteLine("Black Consider Table");
            for (int i = 0; i < ainum; ++i) {
                for (int j = 0; j < ainum; ++j) {
                    System.Console.Write(bmaxconsiders[i, j] + "\t");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();

            System.Console.WriteLine("White Consider Table");
            for (int i = 0; i < ainum; ++i) {
                for (int j = 0; j < ainum; ++j) {
                    System.Console.Write(wmaxconsiders[i, j] + "\t");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();

            System.Console.ReadLine();
        }
    }
}
