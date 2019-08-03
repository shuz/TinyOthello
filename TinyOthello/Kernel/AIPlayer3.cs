using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TinyOthello.Kernel {
    public class AIPlayer3 : AbstractAIPlayer {

        public AIPlayer3(Color color, int maxConsider)
            : this(color, maxConsider, color == Color.Black ? 2 : 1, 2, true) { }

        public AIPlayer3(Color color, int maxConsider, int initDepth, int ddepth, bool breakOnMaxConsider)
            : base(color) {
            this.maxConsider = maxConsider;
            this.initDepth = initDepth;
            this.ddepth = ddepth;
            this.breakOnMaxConsider = breakOnMaxConsider;
        }

        private class SearchDoneException : Exception { }

        public override void PlayOneMove(Board board) {
            lock (board) {
                Debug.Assert(board.CurrentColor == color);

                int depth = this.initDepth;
                int startMove = board.CurrentStep;
                int currentConsider0 = 0;
                Point candidate = null;

                currentConsider = 0;

#if DEBUG
                int hash = board.GetHashCode();
#endif
                int score = 0;
                try {
                    do {
                        currentConsider0 = currentConsider;
                        currentConsider = 0;

#if DEBUG
                        int fp = board.GetHashCode();
#endif

                        bestMove = null;
                        score = Evaluate(board, -INFINITY, INFINITY, depth, true);

#if DEBUG
                        Debug.Assert(fp == board.GetHashCode());
#endif

                        this.movesConsidered += currentConsider;

                        depth += ddepth;
                        candidate = bestMove;

                        Debug.Print("score: " + score + " current consider: " + currentConsider);
                    } while (currentConsider < maxConsider && Math.Abs(score) < StaticEvaluator.END_GAME);
                } catch (SearchDoneException) {
                    if (board.CurrentStep != startMove)
                        board.GotoMove(startMove);
                    this.movesConsidered += currentConsider;
                }
                depth -= ddepth;
#if DEBUG
                Debug.Assert(hash == board.GetHashCode());
#endif
                Debug.Print("max depth: " + depth);

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

            Point bestMove = null;
            int cacheval = tt.LookUp(board, ref alpha, ref beta, ref bestMove, depth);
            if (cacheval != INVALID) {
                if (recordBestMove) this.bestMove = bestMove;
                return cacheval;
            }

            if (depth == 0) return staticEvaluator.Evaluate(board);


            List<Point> validMoves = GetValidMoves(board);
            if (validMoves.Count == 0) {
                board.Pass();
                int score1;
                if (HasValidMove(board)) {
                    score1 = -Evaluate(board, -beta, -alpha, depth - 1, false);
                } else {
                    board.Pass();
                    score1 = StaticEvaluator.EvaluateEndOfGame(board);
                    board.Undo();
                }
                board.Undo();
                return score1;
            }

            ++currentConsider;

            // move ordering: try best move first
            if (bestMove != null) {
                validMoves.Add(validMoves[0]);
                validMoves[0] = bestMove;
            }

            int score = -INFINITY;
            int alpha0 = alpha;
            foreach (Point p in validMoves) {
                Debug.Assert(board.IsLegalMove(p.x, p.y));
                board.PutStone(p.x, p.y);
                int value = -Evaluate(board, -beta, -alpha, depth - 1, false);
                board.Undo();
                if (value > score) {
                    score = value;
                    bestMove = p;
                    if (score > alpha) alpha = score;
                    if (score >= beta) break;
                }
            }
            if (recordBestMove) this.bestMove = bestMove;
            tt.Save(board, score, alpha0, beta, bestMove, depth);
            return score;
        }

        public override void Reset() {
            base.Reset();
            bestMove = null;
            currentConsider = 0;
            tt = new TranspositionTable();
        }

        private int initDepth;
        private Point bestMove;

        private int currentConsider;
        private int maxConsider;
        private int ddepth;
        private bool breakOnMaxConsider;
        private TranspositionTable tt = new TranspositionTable();
    }
}
