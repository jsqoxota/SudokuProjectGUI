using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SudokuCreat;

namespace SudokuProjectGUI.SudokuProjectGUI
{
    public partial class Sudoku : Form
    {
        private int[,] SD;

        public Sudoku()
        {
            InitializeComponent();
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void beginButton_Click(object sender, EventArgs e)
        {
            SudokuCreat.Sudoku S = new SudokuCreat.Sudoku();
            S.SudokuCreate(1);
            SD = S.getSudoku();
            initialize();
            scoop();

            for(int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (SD[i, j] != 0)
                    {
                        string name = "textBox" + i + j;
                        ((TextBox)(this.Controls.Find(name, false)[0])).Text = SD[i, j].ToString();
                        ((TextBox)(this.Controls.Find(name, false)[0])).ReadOnly = true;
                    }
                }
            }
        }

        /// <summary>
        /// 挖空
        /// </summary>
        private void scoop()
        {
            RandomNum rn = new RandomNum();
            int[] temp = new int[2];
            int[] temps = new int[63];
            int[] result = new int[63];

            //每个九宫格挖2空
            for (int z = 0; z < 3; z++)
            {
                for (int i = 0; i < 3; i++)
                {
                    int[] value = new int[9] { 0, 1, 2, 9, 10, 11, 18, 19, 20 };
                    for (int j = 0; j < 9; j++)
                        value[j] += 3 * i + z * 27;
                    temp = rn.GetRandomNum(value, 2);
                    SD[temp[0] / 9, temp[0] % 9] = 0;
                    SD[temp[1] / 9, temp[1] % 9] = 0;
                }
            }
            //获得不为0的空的位置
            for(int i = 0,z=0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (SD[i, j] != 0)
                    {
                        temps[z] = i * 9 + j;
                        z++;
                    }
                }
            }
            //随机挖空
            Random randomNum = new Random();
            result = rn.GetRandomNum(temps, randomNum.Next(12, 42));
            for(int i = 0; i < result.Length; i++)
                SD[result[i] / 9, result[i] % 9] = 0;
        }

        private void initialize()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    string name = "textBox" + i + j;
                    ((TextBox)(this.Controls.Find(name, false)[0])).Text = null;
                    ((TextBox)(this.Controls.Find(name, false)[0])).ReadOnly = false;
                }
            }
        }
    }
}
