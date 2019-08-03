using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TinyOthello.Kernel {
    public class AIPlayer2 : AbstractAIPlayer {

        public AIPlayer2(Color color, int maxConsider)
            : this(color, maxConsider, 2, 1, true) { }

        public AIPlayer2(Color color, int maxConsider, int initDepth, int ddepth, bool breakOnMaxConsider) : base(color) {
            this.maxConsider = maxConsider;
            this.initDepth = initDepth;
            this.ddepth = 1;
            this.breakOnMaxConsider = breakOnMaxConsider;
        }

        private class SearchDoneException : Exception { }

        public override void PlayOneMove(Board board) {
            lock (board) {
                Debug.Assert(board.CurrentColor == color);

                int depth = this.initDepth;
                int currentConsider0 = 0;
                int startMove = board.CurrentStep;

                Point candidate = null;

                currentConsider = -1;       // make sure the initial value of currentConsider0 differs from 0

#if DEBUG
                int hash = board.GetHashCode();
#endif

                try {
                    do {
                        currentConsider0 = currentConsider;
                        currentConsider = 0;

#if DEBUG
                        int fp = board.GetHashCode();
#endif

                        bestMove = null;
                        Evaluate(board, -INFINITY, INFINITY, depth, true);

#if DEBUG
                        Debug.Assert(fp == board.GetHashCode());
#endif

                        this.movesConsidered += currentConsider;

                        depth += ddepth;
                        candidate = bestMove;

                        Debug.Print("current consider: " + currentConsider);
                    } while (currentConsider < maxConsider && currentConsider != currentConsider0);
                } catch (SearchDoneException) {
                    if (board.CurrentStep != startMove)
                        board.GotoMove(startMove);
                    this.movesConsidered += currentConsider;
                }
                depth -= ddepth;
#if DEBUG
                Debug.Assert(hash == board.GetHashCode());
                Debug.Print("max depth: " + depth);
#endif
                this.totalDepth += depth;

                if (candidate != null)
                    board.PutStone(candidate.x, candidate.y);
                else
                    board.Pass();
            }
        }

        public override int Evaluate(Board board, int alpha, int beta, int depth, bool recordBestMove) {
            if (currentConsider == maxConsider && breakOnMaxConsider) {
                throw new SearchDoneException();
            }

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

            ++currentConsider;

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
            currentConsider = 0;
        }

        private int initDepth;
        private Point bestMove;

        private int currentConsider;
        private int maxConsider;
        private int ddepth;
        private bool breakOnMaxConsider;
    }
}
