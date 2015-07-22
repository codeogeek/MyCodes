using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace Assignment
{
    public delegate void onStartDelegate();
    public delegate void onFireDelegate();
    public delegate void ControlFireButtonDelegate();
    public delegate void ScorecardDelegate();
    public delegate void level2TargetDelegate();
    public partial class Form1 : Form
    {
        int y = 3;
        int level1Timer = 500;
        int level2Timer = 400;
        int fireButtonTimer;
        int fireX = 135;
        bool FireLoopBreak = false;
        int score = 9;
        bool canEnter = true;
        bool onTimerforLevel2 = false;

        int currTargetX, currTargetY=3, currArrowX=135, currArrowY;

        ///////////////////////int fireTextboxOpacityController = 1;
        public Form1()
        {
            InitializeComponent();            
                (new SoundPlayer("E:\\dotnet\\workspace\\Assignment\\Assignment\\sound\\cannon_fire.wav")).Play();            
        }

        ///on fire button click
        private void button3_Click(object sender, EventArgs e)
        {
            (new SoundPlayer("E:\\dotnet\\workspace\\Assignment\\Assignment\\sound\\blocker_hit.wav")).Play();
            canEnter = true;
            FireLoopBreak = false;
            if (comboBox1.SelectedItem!=null)
                fireButtonTimer = Convert.ToInt32(comboBox1.SelectedItem.ToString()) * 20;
            else
                fireButtonTimer = 500;            
            Thread t = new Thread(new ThreadStart(onFireThread));
            t.IsBackground = true;
            t.Start();
        }
        private void onFireThread()
        {            
            while (true)
            {
                Invoke(new ControlFireButtonDelegate(FireButtonControl));
                if (FireLoopBreak)
                    break;
                Invoke(new onFireDelegate(onFire));
                Thread.Sleep(fireButtonTimer);
                
            }            
        }

        private void FireButtonControl()
        {
            button3.Enabled = false;
            if(fireX>738)
            {
                fireX = 135;
                button3.Enabled = true;
                FireLoopBreak=true;
            }
        }
        public void onFire()
        {
            TextBox t = new TextBox();
            foreach (object o in this.Controls)
            {
                if (o is TextBox)
                {
                    TextBox t1 = (TextBox)o;                    
                    this.Controls.Remove(t1);
                }
            }
            int k = fireX + 50;
            //if (k > 738)
            //  k = 135;
            fireX = k;
            t.SetBounds(k, 170, 45, 13);
            t.BackColor = Color.Black;

            currArrowX = k;
            currArrowY = 170;

            this.Controls.Add(t);       
        }

        //////////////////////////////////// on start button click
        private void button1_Click(object sender, EventArgs e)
        {            
            Thread t = new Thread(new ThreadStart(onStartThread));
            t.IsBackground = true;
            t.Start();            

            
        }

        private void onStartThread()
        {
            for (; ; )
            {
                try
                {
                    Invoke(new onStartDelegate(onStart));
                    Thread.Sleep(level1Timer);
                }
                catch(Exception e)
                { }
            }
        }
        private void onStart()
        {
            Label l=new Label();
            foreach(object o in this.Controls)
            {
                if (o is Label)
                {
                    Label l1 = (Label)o;
                    this.Controls.Remove(l1);
                }
            }
            int k = y + 48;            
            if (k > 391)
                k = 3;
            y = k;                                      
                l.SetBounds(738,k,13,43);
                l.BackColor=Color.Black;

                currTargetX = 738;
                currTargetY = k;
                this.Controls.Add(l);

                 
               
        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
            
            if (canEnter)
            {
                int hDiff = 738 - currArrowX;
                if (hDiff <= 60 && hDiff >= 22)
                {
                    if (currTargetY >= 127 && currTargetY <= 190)
                    {
                        level1Timer -= 10;
                        if (onTimerforLevel2)
                            level2Timer -= 10;
                        score++;
                        (new SoundPlayer("E:\\dotnet\\workspace\\Assignment\\Assignment\\sound\\cannon_fire.wav")).Play();
                        button4.Text = "Score " + score.ToString();
                        canEnter = false;
                        currArrowX = 135;
                        //currTargetY = 3;

                        if(score==10)
                        {
                            onTimerforLevel2 = true;
                            Thread t = new Thread(new ThreadStart(level2TargetThread));
                            t.Start();
                        }
                    }
                }
            }            
        }
        //2nd level
        private void level2TargetThread()
        {
            while(true)
            {
                try
                {
                    Invoke(new level2TargetDelegate(level2));
                    Thread.Sleep(level2Timer);
                }
                catch(Exception e)
                { }
            }
        }
        private void level2()
        {
            Label l = new Label();
            foreach (object o in this.Controls)
            {
                if (o is Label)
                {
                    Label l1 = (Label)o;
                    this.Controls.Remove(l1);
                }
            }
            int k = y + 48;
            if (k > 391)
                k = 3;
            y = k;
            l.SetBounds(688, k, 13, 43);
            l.BackColor = Color.Black;

            currTargetX = 738;
            currTargetY = k;
            this.Controls.Add(l);
        }
    }
}
