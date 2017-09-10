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

        /// <summary>
        /// 只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar)|| e.KeyChar == 48)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beginButton_Click(object sender, EventArgs e)
        {
            SudokuCreat.Sudoku S = new SudokuCreat.Sudoku();
            S.SudokuCreate(1);
            SD = S.getSudoku();
            initialize();
            scoop();

            //挖空效果
            for(int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (SD[i, j] != 0)
                    {
                        string name = "textBox" + i + j;
                        ((TextBox)(this.Controls.Find(name, false)[0])).Text = SD[i, j].ToString();
                        ((TextBox)(this.Controls.Find(name, false)[0])).BackColor = System.Drawing.SystemColors.Control;
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

        /// <summary>
        /// 初始化textbox
        /// </summary>
        private void initialize()
        {
            label1.Visible = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    string name = "textBox" + i + j;
                    ((TextBox)(this.Controls.Find(name, false)[0])).Text = null;
                    ((TextBox)(this.Controls.Find(name, false)[0])).ReadOnly = false;
                    ((TextBox)(this.Controls.Find(name, false)[0])).ForeColor = System.Drawing.SystemColors.WindowText;
                    ((TextBox)(this.Controls.Find(name, false)[0])).BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkButton_Click(object sender, EventArgs e)
        {
            initCplor();
            if (SD != null)
            {
                bool flag = true;
                for (int i = 0; i < 9; i++)
                {
                    int[] x = new int[9];
                    int[] y = new int[9];
                    for (int j = 0; j < 9; j++)
                    {
                        if (SD[i, j] == 0)
                        {
                            string name = "textBox" + i + j;
                            ((TextBox)(this.Controls.Find(name, false)[0])).BackColor = Color.Red;
                            flag = false;
                        }
                        else x[SD[i,j]-1]++;
                        if (SD[j, i] != 0) y[SD[j, i] - 1]++;
                    }
                    for(int z = 0; z < 9; z++)
                    {
                        if(x[z] > 1)
                        {
                            flag = false;
                            for (int m = 0;m<9;m++)
                            {
                                if (SD[i, m] == z + 1)
                                {
                                    string name = "textBox" + i + m;
                                    ((TextBox)(this.Controls.Find(name, false)[0])).ForeColor = Color.Red;
                                }
                            }
                        }
                        if(y[z] > 1)
                        {
                            flag = false;
                            for (int m = 0; m < 9; m++)
                            {
                                if (SD[m, i] == z + 1)
                                {
                                    string name = "textBox" + m + i;
                                    ((TextBox)(this.Controls.Find(name, false)[0])).ForeColor = Color.Red;
                                }
                            }
                        }
                    }
                }
                for (int z = 0; z < 3; z++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int[] Z = new int[9];
                        int[] value = new int[9] { 0, 1, 2, 9, 10, 11, 18, 19, 20 };
                        for (int j = 0; j < 9; j++)
                            value[j] += 3 * i + z * 27;
                        for (int j = 0; j < 9; j++)
                        {
                            if(SD[value[j] / 9, value[j] % 9]!=0)
                                Z[SD[value[j] / 9, value[j] % 9]-1]++;
                        }
                        for (int j = 0; j < 9; j++)
                        {
                            if (Z[j] > 1)
                            {
                                flag = false;
                                for(int x = 0; x < 9; x++)
                                {
                                    if (SD[value[x] / 9, value[x] % 9] == j + 1)
                                    {
                                        string name = "textBox" + value[x] / 9 + value[x] % 9;
                                        ((TextBox)(this.Controls.Find(name, false)[0])).ForeColor = Color.Red;
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag)label1.Visible = true;
            }
        }

        /// <summary>
        /// 向数组中保存更改结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Leave(object sender, EventArgs e)
        {
            if (SD != null)
            {
                string name = ((TextBox)sender).Name;
                int i = int.Parse(name.Substring(7, 1));
                int j = int.Parse(name.Substring(8, 1));
                if (((TextBox)sender).Text == "") SD[i, j] = 0;
                else SD[i, j] = int.Parse(((TextBox)sender).Text);
            }
        }

        /// <summary>
        /// 背景为白色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Enter(object sender, EventArgs e)
        {
            if (((TextBox)sender).ReadOnly == false)
            {
                label1.Visible = false;
                ((TextBox)sender).BackColor = Color.White;
            }
            ((TextBox)sender).ForeColor = System.Drawing.SystemColors.WindowText;
        }

        /// <summary>
        /// 颜色初始化
        /// </summary>
        private void initCplor()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    string name = "textBox" + i + j;
                    ((TextBox)(this.Controls.Find(name, false)[0])).ForeColor = System.Drawing.SystemColors.WindowText;
                    if(((TextBox)(this.Controls.Find(name, false)[0])).ReadOnly == false)
                        ((TextBox)(this.Controls.Find(name, false)[0])).BackColor = Color.White;
                    else ((TextBox)(this.Controls.Find(name, false)[0])).BackColor = System.Drawing.SystemColors.Control;
                }
            }
        }
    }
}
