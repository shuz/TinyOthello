using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections;

namespace TinyOthello.Kernel {

    public class Board {

        public Board() {
            int size = BOARD_SIZE;
            board = new Color[size, size];
            for (int i = 0; i < size; ++i) {
                for (int j = 0; j < size; ++j) {
                    board[i, j] = Color.Empty;
                }
            }
            board[size / 2 - 1, size / 2 - 1] = Color.Black;
            board[size / 2, size / 2 - 1] = Color.White;
            board[size / 2, size / 2] = Color.Black;
            board[size / 2 - 1, size / 2] = Color.White;

            currentColor = Color.Black;
            stepCount = 0;
            history = new List<HistoryEntry>(size * (size + 1));
            blackScore = 2;
            whiteScore = 2;
        }

        public void SetContent(Color[,] board) {
            Debug.Assert(board.GetLength(0) == board.GetLength(1));
            Debug.Assert(BOARD_SIZE == board.GetLength(0));
            this.board = (Color[,])board.Clone();
        }

        public Color[,] GetClonedContent() {
            return (Color[,])board.Clone();
        }

        public static int BoardSize {
            get { return BOARD_SIZE; }
        }

        public Color this[int x, int y] {
            get {
                return board[x, y];
            }
        }

        public int BlackScore {
            get { return blackScore; }
        }

        public int WhiteScore {
            get { return whiteScore; }
        }

        public int StonesOnBoard {
            get { return blackScore + whiteScore; }
        }

        public int GetScore(Color color) {
            return color == Color.Black ? blackScore : whiteScore;
        }

        public override Int32 GetHashCode() {
            int code = 0;
            int n = 0;
            for (int i = 0; i < BOARD_SIZE; ++i) {
                for (int j = 0; j < BOARD_SIZE; ++j) {
                    code ^= ((board[i, j] != Color.Empty) ? 1 : 0) << (n++ % 32);
                    code ^= ((board[i, j] == Color.White) ? 1 : 0) << (n++ % 32);
                }
            }
            return code;
        }

        public BitArray GetCompactBoard() {
            BitArray bits = new BitArray(BOARD_SIZE*BOARD_SIZE*2 + 1);
            int n = 0;
            for (int i = 0; i < BOARD_SIZE; ++i) {
                for (int j = 0; j < BOARD_SIZE; ++j) {
                    bits[n++] = (board[i, j] != Color.Empty);
                    bits[n++] = (board[i, j] == Color.White);
                }
            }
            bits[n++] = (currentColor == Color.Black);
            return bits;
        }

        private static readonly int[] X_DELTA = { -1 , - 1, -1, 0, 0, 1, 1, 1 };
        private static readonly int[] Y_DELTA = { -1, 0, 1, -1, 1, -1, 0, 1 };

        public bool IsLegalMove(int x, int y) {
            return IsLegalMove(x, y, this.currentColor);
        }

        public bool IsLegalMove(int x, int y, Color color) {
            if (board[x, y] != Color.Empty) return false;

            for (int i = 0; i < BOARD_SIZE; ++i) {
                if (CanReverseInDirection(x, y, i, color))
                    return true;
            }
            return false;
        }

        /**
         * returns the number of reversed stones
         */
        public int PutStone(int x, int y) {
            Debug.Assert(board[x, y] == Color.Empty);

            board[x, y] = currentColor;

            int[] reverses = new int[BOARD_SIZE];
            int n = 0;
            Color oppoColor = Utility.GetOpponentColor(currentColor);
            for (int i = 0; i < BOARD_SIZE; ++i) {
                if (!CanReverseInDirection(x, y, i, currentColor)) continue;

                // reverse all stones between
                int px = x + X_DELTA[i], py = y + Y_DELTA[i];
                while (IsInBoard(px, py) && board[px, py] == oppoColor) {
                    board[px, py] = currentColor;
                    ++n;
                    ++reverses[i];

                    px += X_DELTA[i];
                    py += Y_DELTA[i];
                }
            }
            Debug.Assert(n > 0);  // legal move

            if (currentColor == Color.White) {
                ++whiteScore;
                n = -n;
            } else {
                ++blackScore;
            }

            blackScore += n;
            whiteScore -= n;

            if (this.history.Count != stepCount) {
                CutHistory();
            }
            this.history.Add(new HistoryEntry(x, y, reverses));
            ++stepCount;
            currentColor = oppoColor;
            return n;
        }

        public HistoryEntry LastMove {
            get {
                if (history.Count == 0) return null;
                return history[history.Count-1];
            }
        }

        public void Pass() {
            if (this.history.Count != stepCount) {
                CutHistory();
            }
            this.history.Add(new HistoryEntry(-1, -1, null));
            ++stepCount;
            currentColor = Utility.GetOpponentColor(currentColor);
        }

        public void GotoMove(int count) {
            Debug.Assert(0 <= count && count < history.Count);
            if (count > stepCount) {
                Redo(count - stepCount);
            } else
            if (count < stepCount) {
                Undo(stepCount - count);
            }
            Debug.Assert(stepCount == count);
        }

        public Color CurrentColor {
            get {
                return currentColor;
            }
        }

        public int CurrentStep {
            get {
                return stepCount;
            }
        }

        public class HistoryEntry {
            public HistoryEntry(int x, int y, int[] reverses) {    // (x, y) == (-1, -1) indicates a pass
                this.x = x;
                this.y = y;
                this.reverses = reverses;
            }

            public int X { get { return x; } }
            public int Y { get { return y; } }

            public int GetReverses(int i) {
                return reverses[i];
            }

            private readonly int x, y;
            private readonly int[] reverses;  // number of stones reversed in each direction

        };

        public ReadOnlyCollection<HistoryEntry> History {
            get {
                return history.AsReadOnly();
            }
        }

        public int UndoCount {
            get { return stepCount; }
        }

        public int RedoCount {
            get { return history.Count - stepCount; }
        }

        public void Undo() {
            Undo(1);
        }

        public void Undo(int n) {
            Debug.Assert(0 < n && n <= UndoCount);
            while (n-- > 0) {
                HistoryEntry entry = history[--stepCount];
                if (entry.X != -1 && entry.Y != -1) {
                    if (currentColor == Color.White) {
                        --blackScore;
                    } else {
                        --whiteScore;
                    }
                    board[entry.X, entry.Y] = Color.Empty;
                    ReverseNeighbours(entry, currentColor);
                }
                currentColor = Utility.GetOpponentColor(currentColor);
            }
        }

        public void Redo() {
            Redo(1);
        }

        public void Redo(int n) {
            Debug.Assert(0 < n && n <= RedoCount);
            while (n-- > 0) {
                HistoryEntry entry = history[stepCount++];
                if (entry.X != -1 && entry.Y != -1) {
                    if (currentColor == Color.Black) {
                        ++blackScore;
                    } else {
                        ++whiteScore;
                    }
                    board[entry.X, entry.Y] = currentColor;
                    ReverseNeighbours(entry, currentColor);
                }
                currentColor = Utility.GetOpponentColor(currentColor);
            }
        }

        /**
         * cut the history after current step
         */
        public void CutHistory() {
            history.RemoveRange(stepCount, history.Count - stepCount);
        }

        public bool IsEndOfGame() {
            if (stepCount < 2) return false;
            return history[stepCount - 2].X == -1 && history[stepCount - 1].X == -1;
        }

        private void ReverseNeighbours(HistoryEntry entry, Color color) {
            int revCount = 0;
            for (int i = 0; i < BOARD_SIZE; ++i) {
                int n = entry.GetReverses(i);
                int px = entry.X + X_DELTA[i], py = entry.Y + Y_DELTA[i];

                revCount += n;
                while (n-- > 0) {
                    board[px, py] = color;

                    px += X_DELTA[i];
                    py += Y_DELTA[i];
                }
            }
            if (color == Color.White)
                revCount = -revCount;
            blackScore += revCount;
            whiteScore -= revCount;
        }

        public bool IsInBoard(int x, int y) {
            return 0 <= x && x < BOARD_SIZE && 0 <= y && y < BOARD_SIZE;
        }

        private bool CanReverseInDirection(int x, int y, int d, Color color) {
            Color oppoColor = Utility.GetOpponentColor(color);
            int px = x + X_DELTA[d], py = y + Y_DELTA[d];
            if (!IsInBoard(px, py) || board[px, py] != oppoColor)
                return false;           // neighbour in this direction is not an opponent's stone

            // look for an ally stone so that we can reverse all stones between
            while (IsInBoard(px, py) && board[px, py] == oppoColor) {
                px += X_DELTA[d];
                py += Y_DELTA[d];
            }

            if (!IsInBoard(px, py) || board[px, py] != color)
                return false;           // not found
            return true;
        }

        private const int BOARD_SIZE = 8;
        private int blackScore;
        private int whiteScore;
        private Color[,] board;
        private Color currentColor;

        private List<HistoryEntry> history;
        private int stepCount;

    }
}

