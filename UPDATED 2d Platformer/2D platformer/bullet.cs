using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace _2D_platformer
{
    class bullet
    {
        public string direction;
        public int speed = 10;
        PictureBox Bullet = new PictureBox();
        Timer tm = new Timer();

        public int bulletLeft;
        public int bulletTop;

        public void mkBullet(Form form)
        {
            //TO CREATE BULLET WHEN FUNCTION IS CALLED
            Bullet.BackColor = System.Drawing.Color.White;
            Bullet.Size = new Size(5, 5);
            Bullet.Tag = "bullet";
            Bullet.Left = bulletLeft;
            Bullet.Top = bulletTop;
            Bullet.BringToFront();
            form.Controls.Add(Bullet);

            tm.Interval = speed;
            tm.Tick += new EventHandler(tm_Tick);
            tm.Start();
        }

        public void tm_Tick(object sender, EventArgs e)
        {
            //MOVEMENT OF BULLET OR DIRECTION OF BULLET
            if (direction == "left")
            {
                Bullet.Left -= speed;
            }
            if (direction == "right")
            {
                Bullet.Left += speed;
            }
            if (Bullet.Left < 10 || Bullet.Left > 500)
            {
                //IF BULLET REACH A CERTAIN DISTANCE< REMOVE BULLET
                tm.Stop();
                tm.Dispose();
                Bullet.Dispose();
                tm = null;
                Bullet = null;
            }
        }
    }
}
