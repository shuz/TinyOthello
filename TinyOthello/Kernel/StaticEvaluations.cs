using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TinyOthello.Kernel {

    public interface IStaticEvaluatorConfig {
        int GetStableWeight(Board board);
        int GetMobililtyWeight(Board board);
        int GetEdgeWeight(Board board);
        int GetXPosWeight(Board board);
        int GetCPosWeight(Board board);
        int GetScoreWeight(Board board);
    }

    public sealed class StaticEvaluator {

        #region static features

        public static int CountStables(Board board, Color color) {
            int stables = 0;
            bool[,] counted = new bool[Board.BoardSize, Board.BoardSize];
            stables += CountStables(board, counted, 0, 0, 1, 1, color);
            stables += CountStables(board, counted, Board.BoardSize - 1, 0, -1, 1, color);
            stables += CountStables(board, counted, 0, Board.BoardSize - 1, 1, -1, color);
            stables += CountStables(board, counted, Board.BoardSize - 1, Board.BoardSize - 1, -1, -1, color);
            return stables;
        }

        private static int CountStables(Board board, bool[,] counted, int x, int y, int dx, int dy, Color color0) {
            if (board[x, y] == Color.Empty) return 0;

            Color color = board[x, y];

            int result = 0;
            int xcount, ycount;
            xcount = ycount = Board.BoardSize;

            int recount = 0;
            while (xcount >= 0 && ycount >= 0) {
                int i, j;
                if (counted[x, y])
                    ++recount;
                counted[x, y] = true;
                for (i = 1; i < xcount; ++i) {
                    if (board[x + i * dx, y] != color) break;
                    if (counted[x + i * dx, y])
                        ++recount;
                    counted[x + i * dx, y] = true;
                }
                result += i;
                xcount = i - 2;
                for (j = 1; j < ycount; ++j) {
                    if (board[x, y + j * dy] != color) break;
                    if (counted[x, y + j * dy])
                        ++recount;
                    counted[x, y + j * dy] = true;
                }
                result += j;
                --result;       // p0 is not counted twice
                ycount = j - 2;
                x += dx;
                y += dy;
            }

            return (result - recount) * (color == color0 ? 1 : -1);
        }

        public static int CountMobility(Board board, Color color0) {
            int score = 0;
            Color color1 = Utility.GetOpponentColor(color0);
            for (int i = 0; i < Board.BoardSize; ++i) {
                for (int j = 0; j < Board.BoardSize; ++j) {
                    if (board.IsLegalMove(i, j, color0)) {
                        ++score;
                    }
                    if (board.IsLegalMove(i, j, color1)) {
                        --score;
                    }
                }
            }
            return score;
        }

        public static int CountEdge(Board board, Color color0) {
            int score = 0;
            for (int i = 0; i < Board.BoardSize - 1; ++i) {
                if (board[i, 0] != Color.Empty)
                    score += (board[i, 0] == color0 ? 1 : -1);
            }
            for (int i = 0; i < Board.BoardSize - 1; ++i) {
                if (board[Board.BoardSize - 1, i] != Color.Empty)
                    score += (board[Board.BoardSize - 1, i] == color0 ? 1 : -1);
            }
            for (int i = Board.BoardSize - 1; i > 0; --i) {
                if (board[i, Board.BoardSize - 1] != Color.Empty)
                    score += (board[i, Board.BoardSize - 1] == color0 ? 1 : -1);
            }
            for (int i = Board.BoardSize - 1; i > 0; --i) {
                if (board[0, i] != Color.Empty)
                    score += (board[0, i] == color0 ? 1 : -1);
            }
            return score;
        }

        public static int CountStones(Board board, Color color) {
            return board.GetScore(color) - board.GetScore(Utility.GetOpponentColor(color));
        }

        public static int CountXPositions(Board board, Color color) {
            int score = 0;

            if (board[1, 1] != Color.Empty && board[0, 0] == Color.Empty)
                score += (board[1, 1] != color ? 1 : -1);

            if (board[Board.BoardSize - 2, 1] != Color.Empty && board[Board.BoardSize - 1, 0] == Color.Empty)
                score += (board[Board.BoardSize - 2, 1] != color ? 1 : -1);

            if (board[1, Board.BoardSize - 2] != Color.Empty && board[0, Board.BoardSize - 1] == Color.Empty)
                score += (board[1, Board.BoardSize - 2] != color ? 1 : -1);

            if (board[Board.BoardSize - 2, Board.BoardSize - 2] != Color.Empty &&
                                              board[Board.BoardSize - 1, Board.BoardSize - 1] == Color.Empty)
                score += (board[Board.BoardSize - 2, Board.BoardSize - 2] != color ? 1 : -1);

            return score;
        }

        public static int CountCPositions(Board board, Color color) {
            int score = 0;

            if (board[0, 1] != Color.Empty && board[0, 0] == Color.Empty)
                score += (board[0, 1] != color ? 1 : -1);

            if (board[1, 0] != Color.Empty && board[0, 0] == Color.Empty)
                score += (board[1, 0] != color ? 1 : -1);


            if (board[Board.BoardSize - 1, 1] != Color.Empty && board[Board.BoardSize - 1, 0] == Color.Empty)
                score += (board[Board.BoardSize - 1, 1] != color ? 1 : -1);

            if (board[Board.BoardSize - 2, 0] != Color.Empty && board[Board.BoardSize - 1, 0] == Color.Empty)
                score += (board[Board.BoardSize - 2, 0] != color ? 1 : -1);


            if (board[0, Board.BoardSize - 2] != Color.Empty && board[0, Board.BoardSize - 1] == Color.Empty)
                score += (board[0, Board.BoardSize - 2] != color ? 1 : -1);

            if (board[1, Board.BoardSize - 1] != Color.Empty && board[0, Board.BoardSize - 1] == Color.Empty)
                score += (board[1, Board.BoardSize - 1] != color ? 1 : -1);


            if (board[Board.BoardSize - 1, Board.BoardSize - 2] != Color.Empty &&
                                              board[Board.BoardSize - 1, Board.BoardSize - 1] == Color.Empty)
                score += (board[Board.BoardSize - 1, Board.BoardSize - 2] != color ? 1 : -1);

            if (board[Board.BoardSize - 2, Board.BoardSize - 1] != Color.Empty &&
                                              board[Board.BoardSize - 1, Board.BoardSize - 1] == Color.Empty)
                score += (board[Board.BoardSize - 2, Board.BoardSize - 1] != color ? 1 : -1);

            return score;
        }

        #endregion

        #region test methods

        public static void SelfTest() {
            Color x = Color.Black;
            Color o = Color.White;
            Color _ = Color.Empty;
            bool[,] counted = new bool[8, 8];
            Color[,] board = new Color[8, 8] {
                {x, x, x, x, x, _, x, x,},
                {x, x, o, x, x, x, x, o,},
                {x, x, x, x, o, o, o, o,}, 
                {x, x, o, x, o, _, _, _,},
                {o, x, x, x, x, _, _, _,},
                {x, o, o, x, x, x, _, o,},
                {x, x, x, x, o, o, o, x,},
                {x, x, x, x, o, _, _, o,}
            };
            Board bboard = new Board();
            bboard.SetContent(board);
            Debug.Assert(StaticEvaluator.CountStables(bboard, counted, 0, 0, 1, 1, x) == 10);
            Debug.Assert(StaticEvaluator.CountStables(bboard, counted, 7, 0, -1, 1, o) == -8);
            Debug.Assert(StaticEvaluator.CountStables(bboard, counted, 0, 7, 1, -1, x) == 2);
            Debug.Assert(StaticEvaluator.CountStables(bboard, counted, 7, 7, -1, -1, o) == 1);
            Debug.Assert(StaticEvaluator.CountStables(bboard, x) == 19);
            Debug.Assert(StaticEvaluator.CountEdge(bboard, x) == 11);
            board = new Color[8, 8] {
                {x, x, x, x, x, x, x, x,},
                {x, x, o, x, x, x, x, x,},
                {x, x, x, x, o, o, o, o,}, 
                {x, x, o, x, o, _, _, _,},
                {o, x, x, x, x, _, _, _,},
                {x, o, o, x, x, x, _, o,},
                {x, x, x, x, o, o, o, x,},
                {x, x, x, x, o, _, _, o,}
            };
            bboard.SetContent(board);
            counted = new bool[8, 8];
            Debug.Assert(StaticEvaluator.CountStables(bboard, counted, 0, 0, 1, 1, x) == 13);
            Debug.Assert(StaticEvaluator.CountStables(bboard, counted, 7, 0, -1, 1, o) == -8);
            Debug.Assert(StaticEvaluator.CountStables(bboard, counted, 0, 7, 1, -1, x) == 5);
            Debug.Assert(StaticEvaluator.CountStables(bboard, counted, 7, 7, -1, -1, o) == 1);
            Debug.Assert(StaticEvaluator.CountEdge(bboard, o) == -14);

            board = new Color[8, 8] {
                {_, _, x, x, x, x, _, _,},
                {_, x, o, x, x, x, x, _,},
                {x, x, x, x, o, o, o, o,}, 
                {x, x, o, x, o, _, _, _,},
                {o, x, x, x, x, _, _, _,},
                {x, o, o, x, x, x, _, o,},
                {_, o, x, x, o, o, x, _,},
                {_, _, x, x, o, _, _, o,}
            };
            bboard.SetContent(board);
            Debug.Assert(StaticEvaluator.CountXPositions(bboard, x) == -1);
            Debug.Assert(StaticEvaluator.CountXPositions(bboard, o) == 1);

            board = new Color[8, 8] {
                {_, x, _, _, _, _, x, _,},
                {x, _, _, _, _, _, _, x,},
                {_, _, _, _, _, _, _, _,}, 
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {x, _, _, _, _, _, _, x,},
                {_, x, _, _, _, _, _, o,}
            };
            bboard.SetContent(board);
            Debug.Assert(StaticEvaluator.CountCPositions(bboard, x) == -6);
            Debug.Assert(StaticEvaluator.CountCPositions(bboard, o) == 6);

            board = new Color[8, 8] {
                {_, x, _, _, _, _, _, _,},
                {x, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,}, 
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, x,},
                {_, _, _, _, _, _, x, o,}
            };
            bboard.SetContent(board);

            Debug.Assert(StaticEvaluator.CountCPositions(bboard, x) == -2);
            Debug.Assert(StaticEvaluator.CountCPositions(bboard, o) == 2);
            board = new Color[8, 8] {
                {_, _, _, _, _, _, o, _,},
                {_, _, _, _, _, _, _, o,},
                {_, _, _, _, _, _, _, _,}, 
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {x, _, _, _, _, _, _, _,},
                {o, x, _, _, _, _, _, _,}
            };
            bboard.SetContent(board);

            Debug.Assert(StaticEvaluator.CountCPositions(bboard, x) == 2);
            Debug.Assert(StaticEvaluator.CountCPositions(bboard, o) == -2);
            board = new Color[8, 8] {
                {o, x, _, _, _, _, _, _,},
                {x, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,}, 
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, o,},
                {_, _, _, _, _, _, o, _,}
            };
            bboard.SetContent(board);
            Debug.Assert(StaticEvaluator.CountCPositions(bboard, x) == 2);
            Debug.Assert(StaticEvaluator.CountCPositions(bboard, o) == -2);

            board = new Color[8, 8] {
                {_, _, _, _, _, _, o, o,},
                {_, _, _, _, _, _, _, o,},
                {_, _, _, _, _, _, _, _,}, 
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {_, _, _, _, _, _, _, _,},
                {x, _, _, _, _, _, _, _,},
                {_, x, _, _, _, _, _, _,}
            };
            bboard.SetContent(board);
            Debug.Assert(StaticEvaluator.CountCPositions(bboard, x) == -2);
            Debug.Assert(StaticEvaluator.CountCPositions(bboard, o) == 2);
        }

        #endregion

        public static int EvaluateEndOfGame(Board board) {
            Color color = board.CurrentColor;
            Color oppoColor = Utility.GetOpponentColor(color);
            if (board.GetScore(color) > board.GetScore(oppoColor)) {
                return INFINITY - 1 - board.GetScore(oppoColor);
            } else
            if (board.GetScore(color) < board.GetScore(oppoColor)) {
                return -INFINITY + 1 + board.GetScore(color);
            } else {
                return -END_GAME;
            }
        }

        public StaticEvaluator(IStaticEvaluatorConfig config) {
            this.config = config;
        }

        public StaticEvaluator(int stableWeight, int mobililtyWeight, int edgeWeight, int xposWeight, int cposWeight, int scoreWeight) {
            this.stableWeight = stableWeight;
            this.mobililtyWeight = mobililtyWeight;
            this.edgeWeight = edgeWeight;
            this.xposWeight = xposWeight;
            this.cposWeight = cposWeight;
            this.scoreWeight = scoreWeight;
        }

        public int Evaluate(Board board) {
            if (this.config != null) FillArguments(board);

            Color color = board.CurrentColor;
            int stables = (stableWeight == 0 ? 0 : stableWeight * StaticEvaluator.CountStables(board, color));
            int mobility = (mobililtyWeight == 0 ? 0 : mobililtyWeight * StaticEvaluator.CountMobility(board, color));
            int edge = (edgeWeight == 0 ? 0 : edgeWeight * StaticEvaluator.CountEdge(board, color));
            int xpos = (xposWeight == 0 ? 0 : xposWeight * StaticEvaluator.CountXPositions(board, color));
            int cpos = (cposWeight == 0 ? 0 : cposWeight * StaticEvaluator.CountCPositions(board, color));
            int score = (scoreWeight == 0 ? 0 : scoreWeight * StaticEvaluator.CountStones(board, color));
            return stables + mobility + edge + xpos + cpos + score;
        }

        private void FillArguments(Board board) {
            stableWeight = config.GetStableWeight(board);
            mobililtyWeight = config.GetMobililtyWeight(board);
            edgeWeight = config.GetEdgeWeight(board);
            xposWeight = config.GetXPosWeight(board);
            cposWeight = config.GetCPosWeight(board);
            scoreWeight = config.GetScoreWeight(board);
        }

        private int stableWeight, mobililtyWeight, edgeWeight, xposWeight, cposWeight, scoreWeight;
        private IStaticEvaluatorConfig config;

        public const int INFINITY = int.MaxValue - 10;
        public const int INVALID = int.MaxValue;

        // end of game is reached if the absolute value of score is larger than or equal to END_GAME
        public const int END_GAME = INFINITY - 64;
    }
}
