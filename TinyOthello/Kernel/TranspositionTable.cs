using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TinyOthello.Kernel {
    public class TranspositionTable {

        public TranspositionTable() {
            int slots = Board.BoardSize * Board.BoardSize + 1;
            caches = new Hashtable[slots];
            for (int i = 0; i < slots; ++i) {
                caches[i] = new Hashtable(new CacheEntryEqualityComparator());
            }
        }

        private class CacheEntryEqualityComparator : IEqualityComparer {
            #region IEqualityComparer Members

            bool IEqualityComparer.Equals(object x, object y) {
                Debug.Assert(x.GetType() == typeof(BitArray) && y.GetType() == typeof(BitArray));

                BitArray array1 = (BitArray)x;
                int[] iarray1 = new int[(array1.Count + 31) / 32];
                array1.CopyTo(iarray1, 0);

                BitArray array2 = (BitArray)y;
                int[] iarray2 = new int[(array2.Count + 31) / 32];
                array2.CopyTo(iarray2, 0);

                Debug.Assert(array1.Count == array2.Count);

                for (int i = 0; i < iarray1.Length; ++i) {
                    if (iarray1[i] != iarray2[i]) return false;
                }

                return true;
            }

            int IEqualityComparer.GetHashCode(object obj) {
                Debug.Assert(obj.GetType() == typeof(BitArray));
                BitArray array = (BitArray)obj;
                int[] iarray = new int[(array.Count + 31) / 32];
                array.CopyTo(iarray, 0);

                int code = 0;
                foreach (int i in iarray) {
                    code ^= i;
                }
                return code;
            }

            #endregion
        }


        private enum Bound { Lower, Upper, Accurate };

        private class CacheEntry {
            public int score;
            public Bound bound;
            public int depth;
            public Color color;

            public Point bestMove;
        }

        public int LookUp(Board board, ref int alpha, ref int beta, ref Point bestMove, int depth) {
            CacheEntry entry = (CacheEntry)caches[board.StonesOnBoard][board.GetCompactBoard()];

            if (entry != null) {
                Debug.Assert(entry.color == board.CurrentColor);

                bestMove = entry.bestMove;
                if (entry.depth >= depth) {
#if DEBUG && DEBUG_TT
                    AIPlayer1 aiplayer = new AIPlayer1(entry.color, entry.depth);
                    int rawval = aiplayer.Evaluate(board, -INFINITY, INFINITY, entry.depth, false);
#endif
                    switch (entry.bound) {
                        case Bound.Lower:
#if DEBUG && DEBUG_TT
                            Debug.Assert(rawval >= entry.score);
#endif
                            alpha = Math.Max(alpha, entry.score);
                            break;
                        case Bound.Upper:
#if DEBUG && DEBUG_TT
                            Debug.Assert(rawval <= entry.score);
#endif
                            beta = Math.Min(beta, entry.score);
                            break;
                        case Bound.Accurate:
#if DEBUG && DEBUG_TT
                            Debug.Assert(rawval == entry.score);
#endif
                            alpha = beta = entry.score;
                            break;
                    }
                    if (alpha >= beta) return entry.score;
                }
            }
            return INVALID;
        }

        public void Save(Board board, int score, int alpha, int beta, Point bestMove, int depth) {
            CacheEntry oldEntry = (CacheEntry)caches[board.StonesOnBoard][board.GetCompactBoard()];
            if (oldEntry != null && oldEntry.depth > depth) return;

            CacheEntry entry = new CacheEntry();
            entry.score = score;
            entry.depth = depth;
            entry.color = board.CurrentColor;
            entry.bestMove = bestMove;
            if (score <= alpha)
                entry.bound = Bound.Upper;
            else
            if (score >= beta)
                entry.bound = Bound.Lower;
            else
                entry.bound = Bound.Accurate;

            caches[board.StonesOnBoard][board.GetCompactBoard()] = entry;
        }

        public void Clear() {
            for (int i = 0; i < caches.Length; ++i) {
                caches[i].Clear();
            }
            lastCount = 0;
        }

        public void RemoveUntil(int n) {
            for (int i = lastCount; i < n; ++i) {
                caches[i].Clear();
            }
            lastCount = n;
        }

        private Hashtable[] caches;
        private int lastCount;

        private const int INFINITY = StaticEvaluator.INFINITY;
        private const int INVALID = StaticEvaluator.INVALID;
    }
}
