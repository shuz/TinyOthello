using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TinyOthello.Kernel {
    public class AIPlayer5 : AbstractAIPlayer {

        public AIPlayer5(Color color, int maxConsider)
            : this(color, maxConsider, (color == Color.Black ? 2 : 1), true) { }

        public AIPlayer5(Color color, int maxConsider, int initDepth, bool breakOnMaxConsider)
            : base(color) {
            this.maxConsider = maxConsider;
            this.initDepth = initDepth;
            this.ddepth = 2;
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

                        this.movesConsidered += currentConsider;
#if DEBUG
                        Debug.Assert(fp == board.GetHashCode());
#endif

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

            if (bestMove == null) {
                bestMove = validMoves[validMoves.Count - 1];
                validMoves.RemoveAt(validMoves.Count - 1);
            }

            int score = -INFINITY;
            int alpha0 = alpha;

            board.PutStone(bestMove.x, bestMove.y);
            score = -Evaluate(board, -beta, -alpha, depth-1, false);
            board.Undo();
            if (score >= beta) goto end;

            foreach (Point p in validMoves) {
                Debug.Assert(board.IsLegalMove(p.x, p.y));
                board.PutStone(p.x, p.y);
                int value = -Evaluate(board, -score-1, -score, depth - 1, false);
                board.Undo();
                if (value > score) {
                    score = value;
                    bestMove = p;
                    if (score > alpha) alpha = score;
                    if (score >= beta) break;
                    if (score < beta) {
                        board.PutStone(p.x, p.y);
                        value = -Evaluate(board, -beta, -score, depth-1, false);
                        board.Undo();
                    }
                    if (value > score) {
                        score = value;
                        bestMove = p;
                        if (score > alpha) alpha = score;
                        if (score >= beta) break;
                    }
                }
            }
            
end:        if (recordBestMove) this.bestMove = bestMove;
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
