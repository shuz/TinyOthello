using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace TinyOthello.Kernel {

    public interface IAIPlayer : IPlayer {
        int MovesConsidered { get; set; }
        int TotalDepth { get; set; }
        StaticEvaluator StaticEvaluator { get; set; }
        void Reset();
    }

    public class AIPlayerTest {
        public static void SelfTest(IAIPlayer player, IAIPlayer player2) {
            Color x = Color.Black;
            Color o = Color.White;
            Color _ = Color.Empty;
            Board bboard = new Board();

            Color[,] board = new Color[8, 8] {
                {_, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _},
                {_, _, x, o, _, _, _, _},
                {_, _, x, x, _, x, _, _},
                {_, _, _, o, x, o, _, _},
                {_, _, x, x, x, o, x, _},
                {_, x, _, _, _, o, _, _},
                {_, _, _, _, _, _, _, _}
            };
            bboard.SetContent(board);
            bboard.Pass();

            player.PlayOneMove(bboard);
            Debug.Assert(bboard.History[bboard.CurrentStep - 1].X == 7);
            Debug.Assert(bboard.History[bboard.CurrentStep - 1].Y == 0);

            board = new Color[8, 8] {
                {_, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _},
                {_, _, o, x, _, _, _, _},
                {_, _, o, o, _, o, _, _},
                {_, _, _, x, o, x, _, _},
                {_, _, o, o, o, x, x, _},
                {_, o, _, _, _, x, _, _},
                {_, _, _, _, _, _, _, _}
            };
            bboard.SetContent(board);

            player2.PlayOneMove(bboard);
            Debug.Assert(bboard.History[bboard.CurrentStep - 1].X == 7);
            Debug.Assert(bboard.History[bboard.CurrentStep - 1].Y == 0);
        }
    }
}
