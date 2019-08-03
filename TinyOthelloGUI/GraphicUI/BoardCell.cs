using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TinyOthello.Kernel;

namespace TinyOthello.GraphicUI {
    public partial class BoardCell : Button {
        private const int SIZE = 40;

        public BoardCell(int x, int y) {
            InitializeComponent();

            this.x = x;
            this.y = y;
            Location = new System.Drawing.Point(x * SIZE, y * SIZE);
        }

        public int X { get { return x; } }
        public int Y { get { return y; } }

        public enum State { Empty = 0, Black, White, Highlight }

        public State Content {
            get { return state; }
            set { 
                state = value; 
                switch (state) {
                    case State.Empty:
                        BackColor = System.Drawing.Color.FromArgb(238, 179, 100);
                        Image = null;
                        break;
                    case State.Highlight:
                        BackColor = System.Drawing.Color.FromArgb(238, 210, 130);
                        Image = null;
                        break;
                    case State.Black:
                        BackColor = System.Drawing.Color.FromArgb(238, 179, 100);
                        Image = Resources.Black;
                        break;
                    case State.White:
                        BackColor = System.Drawing.Color.FromArgb(238, 179, 100);
                        Image = Resources.White;
                        break;
                }
            }
        }

        private State state;
        private int x, y;
    }
}
