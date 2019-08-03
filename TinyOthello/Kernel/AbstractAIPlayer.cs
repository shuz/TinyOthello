using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TinyOthello.Kernel {
    public abstract class AbstractAIPlayer : IAIPlayer {

        public AbstractAIPlayer(Color color) : this(color, Utility.DEFAULT_STATIC_EVALUATOR) {}

        public AbstractAIPlayer(Color color, StaticEvaluator se) {
            this.color = color;
            this.staticEvaluator = se;
        }

        public abstract void PlayOneMove(Board board);

        public Color Color {
            get { return color; }
        }

        public int MovesConsidered {
            get { return movesConsidered; }
            set { movesConsidered = value; }
        }

        public int TotalDepth {
            get { return totalDepth; }
            set { totalDepth = value; }
        }

        public StaticEvaluator StaticEvaluator {
            get { return staticEvaluator; }
            set { staticEvaluator = value; }
        }

        protected virtual List<Point> GetValidMoves(Board board) {
            List<Point> points = new List<Point>();
            for (int i = 0; i < Board.BoardSize; ++i) {
                for (int j = 0; j < Board.BoardSize; ++j) {
                    if (board.IsLegalMove(i, j)) {
                        points.Add(new Point(i, j));
                    }
                }
            }
            return points;
        }

        protected virtual bool HasValidMove(Board board) {
            for (int i = 0; i < Board.BoardSize; ++i) {
                for (int j = 0; j < Board.BoardSize; ++j) {
                    if (board.IsLegalMove(i, j)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public abstract int Evaluate(Board board, int alpha, int beta, int depth, bool recordBestMove);

        public virtual void Reset() {
            movesConsidered = 0;
            totalDepth = 0;
        }

        public override string ToString() {
            return base.ToString() + " total depth: " + totalDepth + " total considered: " + movesConsidered;
        }

        protected Color color;
        protected int movesConsidered;
        protected int totalDepth;
        protected StaticEvaluator staticEvaluator;

        protected const int INFINITY = StaticEvaluator.INFINITY;
        protected const int INVALID = StaticEvaluator.INVALID;
    }
}
