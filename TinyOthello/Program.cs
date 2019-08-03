using System;
using System.Collections.Generic;
using System.Text;
using TinyOthello.Kernel;
using System.Diagnostics;
using System.Windows.Forms;

namespace TinyOthello {
    static class Program {
        public static void Main() {

#if DEBUG
            Trace.Listeners.Add(new ConsoleTraceListener());

            // self tests
            StaticEvaluator.SelfTest();
            AIPlayer1 p1 = new AIPlayer1(Color.White, 3);
            AIPlayer2 p2 = new AIPlayer2(Color.White, 1000);
            AIPlayer3 p3 = new AIPlayer3(Color.White, 1000);
            AIPlayer4 p4 = new AIPlayer4(Color.White, 1000);
            AIPlayer5 p5 = new AIPlayer5(Color.White, 1000);
            AIPlayer4Plus p4p = new AIPlayer4Plus(Color.White);

            AIPlayer1 p1b = new AIPlayer1(Color.Black, 3);
            AIPlayer2 p2b = new AIPlayer2(Color.Black, 1000);
            AIPlayer3 p3b = new AIPlayer3(Color.Black, 1000);
            AIPlayer4 p4b = new AIPlayer4(Color.Black, 1000);
            AIPlayer5 p5b = new AIPlayer5(Color.Black, 1000);
            AIPlayer4Plus p4pb = new AIPlayer4Plus(Color.Black);

            AIPlayerTest.SelfTest(p1, p1b);
            AIPlayerTest.SelfTest(p2, p2b);
            AIPlayerTest.SelfTest(p3, p3b);
            AIPlayerTest.SelfTest(p4, p4b);
            AIPlayerTest.SelfTest(p5, p5b);
            AIPlayerTest.SelfTest(p4p, p4pb);
#endif
            AITest.MainLoop();
        }
    }
}
