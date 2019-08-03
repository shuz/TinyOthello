using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TinyOthello.Kernel {
    
    public enum Color { Empty = 0, Black, White };

    public class Point {
        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public int x, y;
    }
    
    public class Utility {
        private class DefaultAIConfig : IAIConfig {
            #region IAIConfig Members

            public int GetMaxConsider(Board board) {
                return 130000;
            }

            public int GetBreakMax(Board board) {
                if (board.StonesOnBoard < 46) return 20000;
                return 130000;
            }

            public int GetDDepth(Board board) {
                return 2;
            }

            public bool GetBreakOnMaxConsider(Board board) {
                return true;
            }

            public int GetInitDepth(Board board) {
                return (board.StonesOnBoard % 2 == 0 ? 2 : 1);
            }

            public int GetMaxDepth(Board board) {
                if (board.StonesOnBoard < 25) return 6;
                if (board.StonesOnBoard < 40) return 10;
                return int.MaxValue;
            }

            #endregion
        }

        private class DefaultStaticConfig : IStaticEvaluatorConfig {

            #region IStaticEvaluatorConfig Members

            public int GetStableWeight(Board board) {
                return 10000;
            }

            public int GetMobililtyWeight(Board board) {
                return 200;
            }

            public int GetEdgeWeight(Board board) {
                return 80;
            }

            public int GetXPosWeight(Board board) {
                if (board.StonesOnBoard < 40) return 500;
                if (board.StonesOnBoard < 50) return 200;
                return 0;
            }

            public int GetCPosWeight(Board board) {
                if (board.StonesOnBoard < 40) return 160;
                if (board.StonesOnBoard < 50) return 80;
                return 0;
            }

            public int GetScoreWeight(Board board) {
                if (board.StonesOnBoard < 55) return 0;
                return 30;
            }

            #endregion
        }

        public static readonly StaticEvaluator DEFAULT_STATIC_EVALUATOR = new StaticEvaluator(new DefaultStaticConfig());
        public static readonly IAIConfig DEFAULT_AI_CONFIG = new DefaultAIConfig();

        public static Color GetOpponentColor(Color c) {
            Debug.Assert(c != Color.Empty);
            return c == Color.Black ? Color.White : Color.Black; ;
        }

        public static int Random(int h) {
            return rand.Next(h);
        }

        private static Random rand = new Random();
    }
}
