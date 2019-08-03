using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using TinyOthello.Kernel;
using Color = TinyOthello.Kernel.Color;
using System.Diagnostics;

namespace TinyOthello.GraphicUI {
    public partial class MainWindow : Form, IBoardViewer {
        public MainWindow() {
            InitializeComponent();

            boardGUI = new BoardCell[Board.BoardSize, Board.BoardSize];
            for (int i = 0; i < Board.BoardSize; ++i) {
                for (int j = 0; j < Board.BoardSize; ++j) {
                    boardGUI[i, j] = new BoardCell(i, j);
                    boardPanel.Controls.Add(boardGUI[i, j]);
                    boardGUI[i, j].Click += new EventHandler(MainWindowCell_Click);
                }
            }

            board = new Board();
        }

        private void MainWindowCell_Click(object sender, EventArgs e) {
            if (Monitor.TryEnter(board)) {
                BoardCell cell = (BoardCell)sender;
                if (cell.Content == BoardCell.State.Highlight) {
                    MovePlayed(board.CurrentColor, cell.X, cell.Y);
                }
                Monitor.Exit(board);
            }
        }

        public delegate void PlayMoveHandler(Color color, int x, int y);
        public event PlayMoveHandler MovePlayed;

        private BoardCell[,] boardGUI;

        #region IBoardViewer Members

        public void ShowBoard(Board board) {
            lock (board) {
                bool needHighLight = !(GetPlayer(board.CurrentColor) is IAIPlayer);
                canPass = GetPlayer(board.CurrentColor) is GraphicHumanPlayer;
                for (int i = 0; i < Board.BoardSize; ++i) {
                    for (int j = 0; j < Board.BoardSize; ++j) {
                        switch (board[i, j]) {
                            case Color.Empty:
                                if (needHighLight && board.IsLegalMove(i, j)) {
                                    boardGUI[i, j].Content = BoardCell.State.Highlight;
                                    canPass = false;
                                } else {
                                    boardGUI[i, j].Content = BoardCell.State.Empty;
                                }
                                break;
                            case Color.Black:
                                boardGUI[i, j].Content = BoardCell.State.Black;
                                break;
                            case Color.White:
                                boardGUI[i, j].Content = BoardCell.State.White;
                                break;
                        }
                    }
                }
                this.Invoke(new SetStatusCallback(SetStatus), 
                            new object[] { canPass, board.BlackScore, board.WhiteScore, board.CurrentColor });
            }
        }

        private delegate void SetStatusCallback(bool canPass, int black, int white, Color current);
        private void SetStatus(bool canPass, int black, int white, Color current) {
            btnPass.Enabled = canPass;
            picCurrent.Image = current == Color.Black ? Resources.Black : Resources.White;
            lblBlack.Text = "" + black;
            lblWhite.Text = "" + white;
            if (GetPlayer(current) is IAIPlayer)
                boardPanel.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            else
                boardPanel.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private delegate void ResetCursorCallback();
        private void ResetBoardCursor() {
            boardPanel.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        #endregion

        private void GameThread() {
            game = new Game(player1, player2, this);
            int val = game.MainLoop(board);

            Debug.Print("player 1: " + player1);
            Debug.Print("player 2: " + player2);

            string msg;
            if (val > 0) 
                msg = "Black won by " + val + " stones";
            else
            if (val < 0)
                msg = "White won by " + -val + " stones";
            else
                msg = "Draw";

            this.Invoke(new ResetCursorCallback(ResetBoardCursor));

            MessageBox.Show(msg);
        }

        public IPlayer GetPlayer(Color color) {
            if (color == player1.Color)
                return player1;
            return player2;
        }

        private Thread gameThread;
        private Board board;
        private Game game;
        private IPlayer player1, player2;
        private bool canPass;

        private void btnPass_Click(object sender, EventArgs e) {
            lock (board) {
                if (!canPass) return;
                MovePlayed(board.CurrentColor, -1, -1);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) {
            if (gameThread != null) 
                gameThread.Abort();
        }

        private void btnUndo_Click(object sender, EventArgs e) {
            if (Monitor.TryEnter(board)) {
                if (board.UndoCount >= 2)
                    board.Undo(2);
                Monitor.Exit(board);
                ShowBoard(board);
            }
        }

        private void btnRedo_Click(object sender, EventArgs e) {
            if (Monitor.TryEnter(board)) {
                if (board.RedoCount >= 2)
                    board.Redo(2);
                Monitor.Exit(board);
                ShowBoard(board);
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e) {
            if (Monitor.TryEnter(board)) {
                if (gameThread != null) {
                    gameThread.Abort();
                    System.GC.Collect();
                }
                Monitor.Exit(board);

                board = new Board();
                CreatePlayer1();
                CreatePlayer2();

                gameThread = new Thread(new ThreadStart(GameThread));
                gameThread.Priority = ThreadPriority.BelowNormal;
                gameThread.Start();
            }
        }

        private void CreatePlayer1() {
            if (blackIsAI.Checked) {
                player1 = new AIPlayer4Plus(Color.Black);
            } else {
                player1 = new GraphicHumanPlayer(Color.Black, this);
            }
        }

        private void CreatePlayer2() {
            if (whiteIsAI.Checked) {
                player2 = new AIPlayer4Plus(Color.White);
            } else {
                player2 = new GraphicHumanPlayer(Color.White, this);
            }
        }

        private void blackIsAI_CheckedChanged(object sender, EventArgs e) {
            CreatePlayer1();
            if (game != null) {
                game.SetPlayer(Color.Black, player1);
            }
        }

        private void whiteIsAI_CheckedChanged(object sender, EventArgs e) {
            CreatePlayer2();
            if (game != null) {
                game.SetPlayer(Color.White, player2);
            }
        }
    }
}