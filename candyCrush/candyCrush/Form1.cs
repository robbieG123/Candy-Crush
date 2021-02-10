using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace candyCrush
{
    public partial class Form1 : Form
    {
        Button[,] btn = new Button[9, 9];
        Random r = new Random();

        Button selected;

        public Form1()
        {
            InitializeComponent();
            for (int x = 0; x < btn.GetLength(0); x++)

            {
                for (int y = 0; y < btn.GetLength(1); y++)
                {
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(55 + (55 * x), 55 + (55 * y), 55, 55); //Sets size of buttons on grid
                    btn[x, y].BackColor = randomColor(); //Assigns random color to grid 

                    int[] coOrdinates = { x, y };
                    btn[x, y].Tag = coOrdinates;

                    btn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x, y]);


                }
            }
        }

        Color randomColor()
        {
            int color = r.Next(1, 6);

            switch(color)
            {
                case 1:
                    return Color.Red;
                case 2:
                    return Color.Blue;
                case 3:
                    return Color.Purple;
                case 4:
                    return Color.Orange;
                case 5:
                    return Color.Green;
                default:
                    return Color.Red;
            }
        }


        void btnEvent_Click(object sender, EventArgs e)
        {
            if (selected == null)
                selected = (Button)sender;


            if (adjacent((Button)sender))
            {
                Button b = (Button)sender;
                swap(b, selected);

                row();

                selected = null;
                return;
            }

            selected = (Button)sender;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            row(); //detects any lines in the random colours
        }
        
        //swaps the colour of two buttons
        private void swap(Button b1, Button b2)
        {
            Color b1Col = b1.BackColor;
            Color b2Col = b2.BackColor;

            b1.BackColor = b2Col;
            b2.BackColor = b1Col;

            Update();
        }

        //checks if a selected button is adjacent to the previously selected one 
        private bool adjacent(Button origin)
        {
            if (origin.Equals(selected))
                return false;

            int[] coOrdinates = (int[])origin.Tag;
            int[,] offset = {
                                      { 0, -1 },
                            {-1, 0 },            { 1, 0 },
                                      { 0, 1 }
                    };

            if (coOrdinates[1] == 0) //top row
            {
                offset.SetValue(0, 0, 1);
            }
            if (coOrdinates[0] == 8) //right row
            {
                offset.SetValue(0, 2, 0);
            }
            if (coOrdinates[1] == 8) //bottom row
            { 
                offset.SetValue(0, 3, 1);
            }
            if (coOrdinates[0] == 0) //left row
            {
                offset.SetValue(0, 1, 0);
            }

      
            for (int i=0; i<offset.GetLength(0); i++)
            {
                int x = coOrdinates[0] + offset[i,0];
                int y = coOrdinates[1] + offset[i, 1];

                if (selected.Equals(btn[x, y]))
                    return true;
            }

            return false;
        }

        //checks for a line of 3 or more of the same colour
        private void row()
        {
            for (int x = 0; x < btn.GetLength(0); x++)
            {
                for (int y = 0; y < btn.GetLength(1); y++)
                {
                    int[,] offset = {
                                      { 0, -1 },
                            {-1, 0 },            { 1, 0 },
                                      { 0, 1 }
                    };

                    //remove the offset if the row is on an edge so as not to cause an out of bounds error 
                    if (y == 0) //top row
                    {
                        offset.SetValue(0, 0, 1);
                    }
                    if (x == 8) //right row
                    {
                        offset.SetValue(0, 2, 0);
                    }
                    if (y == 8) //bottom row
                    {
                        offset.SetValue(0, 3, 1);
                    }
                    if (x == 0) //left row
                    {
                        offset.SetValue(0, 1, 0);
                    }

                    //check each adjacent tile using the offsets
                    for (int i = 0; i < offset.GetLength(0); i++)
                    {
                        int col = x + offset[i, 0];
                        int row = y + offset[i, 1];

                        //check the two colours (had to string compare the name since comparing the colours wasnt working for some reason
                        if (btn[col,row].BackColor.Name.Equals(btn[x, y].BackColor.Name))
                        {
                            Stack buttons = new Stack();
                            bool endReached = false;

                            buttons.Push(btn[x, y]);

                            int cCol = col;
                            int cRow = row;
                            
                            while (!endReached)
                            {
                                if (cRow < 0 || cCol < 0 || cRow > 8 || cCol > 8)
                                {
                                    endReached = true;
                                    break;
                                }
                                else if (btn[cCol, cRow].BackColor.Name.Equals(btn[x, y].BackColor.Name))
                                {
                                    buttons.Push(btn[cCol, cRow]);

                                    if (i == 0)
                                        cRow--;
                                    else if (i == 1)
                                        cCol--;
                                    else if (i == 2)
                                        cCol++;
                                    else if (i == 3)
                                        cRow++;
                                }
                                else
                                    endReached = true;
                            }

                            
                            if (buttons.Count >= 3)
                            {
                                int bCount = buttons.Count;
                                for (int c=0; c<bCount; c++)
                                {
                                    Button b = (Button)buttons.Pop();
                                    b.BackColor = Color.Transparent;
                                    Update();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}