using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2D_platformer
{
    public partial class Form1 : Form
    {
        bool goleft = false;
        bool goright = false;
        bool jumping = false;
        string facing = "Left"; //DIRECTION WHERE THE PLAYER IS FACING 

        int jumpSpeed = 10;
        int force = 8;
        int score = 0;

        public string Facing { get => facing; set => facing = value; }

        public Form1()
        {
            InitializeComponent();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            //CHECK IF KEY IS DOWN
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
                Facing = "left";
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
                Facing = "right";
            }
            if (e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            //CHECK IF KEY IS UP
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if (jumping)
            {
                jumping = false;
            }
            if (e.KeyCode == Keys.E)
            {
                shoot(Facing);
            }

        }

        private void shoot(string direct)
        {
            //BULLET SHOOTING ABILITY BY CALLING BULLET CLASS
            bullet shoot = new bullet();
            shoot.direction = direct;
            shoot.bulletLeft = player.Left + (player.Width / 2);
            shoot.bulletTop = player.Top + (player.Height / 2);
            shoot.mkBullet(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //BASICALLY THE GAME ENGINE THAT RUN THE GAME
            player.Top += jumpSpeed;

            // PLAYER MOVEMENT
            if (jumping && force < 0)
            {
                jumping = false;
            }
            if (goleft)
            {
                player.Left -= 5;
            }
            if (goright)
            {
                player.Left += 5;
            }
            if (jumping)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            
            // to check if picturebox and tag is platform
            foreach (Control x in this.Controls)
            {
                //CHECK IF ITS PLATFORM OR NOT, IF ITS PLATFORM, PLAYER WILL STAND ON PLATFORM
                if (x is PictureBox && x.Tag == "platform")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && !jumping)
                    {
                        force = 8;
                        player.Top = x.Top - player.Height;
                    }
                }
                //CHECK IF IT COIN OR NOT, IF PLAYER TOUCHED COIN, COIN DISPOSE AND EARN SCORE
                if(x is PictureBox && x.Tag == "coin")
                {
                    if(player.Bounds.IntersectsWith(x.Bounds) && !jumping)
                    {
                        this.Controls.Remove(x);
                        score += 10;
                    }
                }
                //TO CHECK IF ITS BULLET OR NOT, IF ITS BULLET DISPOSE IT AFTER BEING SHOOT FOR A DISTANCE
                if(x is PictureBox && x.Tag == "bullet")
                {
                    if(((PictureBox)x).Left < 1 || ((PictureBox)x).Left > 500){
                        this.Controls.Remove(((PictureBox)x));
                        ((PictureBox)x).Dispose();
                    }
                }
                foreach (Control j in this.Controls)
                {
                    //WHEN BULLET TOUCHED ENEMY, ENEMY DIED AND DESTROY
                    if ((j is PictureBox && j.Tag == "bullet") && (x is PictureBox && x.Tag == "enemy"))
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            this.Controls.Remove(j);
                            j.Dispose();
                            this.Controls.Remove(x);
                            x.Dispose();
                            LeftT.Stop();
                            RightT.Stop();
                            score += 10;
                        }
                    }
            }
            }

            label1.Text = "Score = " + score.ToString();
            //WHEN PLAYER TOUCH THE DOOR OR PORTAL, PLAYER WIN THE GAME AND SHOW SCORE
            if (player.Bounds.IntersectsWith(door.Bounds))
            {
                timer1.Stop();
                MessageBox.Show("YOU WIN!!\n\nScore : " + score);
            }
            //WHEN PLAYER TOUCHED ENEMY, PLAYER DIED AND LOSE THE GAME
            if(player.Bounds.IntersectsWith(enemy.Bounds))
            {
                timer1.Stop();
                LeftT.Stop();
                RightT.Stop();
                MessageBox.Show("YOU LOSE!!");
            }
        }

        //MOVEMENT FOR ENEMY
        //ENEMY MOVE RIGHT
        private void LeftT_Tick(object sender, EventArgs e)
        {
            enemy.Left -= 3;

            if (enemy.Left <= 0)
            {
                LeftT.Enabled = false;
                RightT.Enabled = true;
            }
        }
        //ENEMY MOVE LEFT
        private void RightT_Tick(object sender, EventArgs e)
        {
            enemy.Left += 3;

            if(enemy.Left >= Width)
            {
                LeftT.Enabled = true;
                RightT.Enabled = false;
            }
        }
       
       
        //INTERFACE FOR OUR GAME

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void about2DPlatformerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About 2D platformer...\n\nPrepared by Ong Wee Han & Yap Yuien Feng.");
        }

        private void howToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press E to shoot.\nPress Space to jump.\nPress Left & Right to move.\n");
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
