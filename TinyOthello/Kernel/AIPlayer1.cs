using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TinyOthello.Kernel {
    public class AIPlayer1 : AbstractAIPlayer {
        public AIPlayer1(Color color, int depth) : base(color) {
            this.depth = depth;
        }

        public override void PlayOneMove(Board board) {
            lock (board) {
                Debug.Assert(board.CurrentColor == color);
                List<Point> validMoves = GetValidMoves(board);

#if DEBUG
                int fp = board.GetHashCode();
#endif

                bestMove = null;
                Evaluate(board, -INFINITY, INFINITY, depth, true);

#if DEBUG
                Debug.Assert(fp == board.GetHashCode());
#endif
                totalDepth += depth;

                if (bestMove != null) {
                    board.PutStone(bestMove.x, bestMove.y);
                } else {
                    board.Pass();
                }
            }
        }

        public override int Evaluate(Board board, int alpha, int beta, int depth, bool recordBestMove) {
            if (depth == 0) return staticEvaluator.Evaluate(board);

            --depth;
            List<Point> validMoves = GetValidMoves(board);
            if (validMoves.Count == 0) {
                board.Pass();
                int score1;
                if (HasValidMove(board)) {
                    score1 = -Evaluate(board, -beta, -alpha, depth, false);
                } else {
                    board.Pass();
                    score1 = StaticEvaluator.EvaluateEndOfGame(board);
                    board.Undo();
                }
                board.Undo();
                return score1;
            }

            ++movesConsidered;

            int score = -INFINITY;
            foreach (Point p in validMoves) {
                Debug.Assert(board.IsLegalMove(p.x, p.y));
                board.PutStone(p.x, p.y);
                int value = -Evaluate(board, -beta, -alpha, depth, false);
                board.Undo();
                if (value > score) {
                    score = value;
                    if (recordBestMove) bestMove = p;
                    if (score > alpha) alpha = score;
                    if (score >= beta) break;
                }
            }
            return score;
        }

        public override void Reset() {
            base.Reset();
            bestMove = null;
        }

        private int depth;
        private Point bestMove;
    }
}
