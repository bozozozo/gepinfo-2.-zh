using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        int i = 0; 
        bool forward = true; 
        int fastcounter = 0;
        int number_of_slides;
        List<Image> images = new List<Image>();
        bool hide_mode = false;
        bool atlatszo = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            atlatszo = !atlatszo;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Minimum = 1;    
            trackBar1.Maximum = 1500; 
            trackBar1.Value = 100;      
            trackBar2.Minimum = 0;
            trackBar2.Maximum = 1;
            trackBar2.LargeChange = 1;  
            label1.Text = "Speed";
            label2.Text = "# of Pic";
            label3.Text = "100 ms";
            label4.Text = "";
            this.Text = "Animation Player";  
            button1.Text = "Start";
            button2.Text = "Forward/Reverse";
            button3.Text = "Fast Play";
            button4.Text = "Load Images";
            button5.Text = "Pic Effects";
            button6.Text = "Picture on Picture";
            timer1.Enabled = false;
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, Color.Black);
            comboBox1.Text = "Válasszon effektust!";

            comboBox1.Items.AddRange(new string[] { "horizontális flipp", "vertikális flipp", "90 fok(-os pálinka hatása)", "szempöl", "picibb", "vÍzes márk" });
            comboBox1.SelectedIndex = -1; 
            comboBox1.Enabled = false; 
            comboBox1.Visible = false;
            button6.Visible = false;

            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            number_of_slides = images.Count;
            if (number_of_slides == 0) return;
            timer1.Interval = trackBar1.Value;
            pictureBox1.Image = images[i];
            listBox1.SelectedIndex = i;
            if (forward)
                i = (i + 1) % number_of_slides;
            else
            {
                i = (i + number_of_slides - 1) % number_of_slides;
                if (i == 0) i = number_of_slides - 1;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                button1.Text = "Start";
            else
                button1.Text = "Stop";
            timer1.Enabled = !timer1.Enabled;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            forward = !forward;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (fastcounter)
            {
                case 0:
                    button3.Text = "Fast Play 2x";
                    timer1.Interval /= 2;
                    break;
                case 1:
                    button3.Text = "Fast Play 4x";
                    timer1.Interval /= 2;
                    break;
                case 2:
                    button3.Text = "Fast Play 8x";
                    timer1.Interval /= 2;
                    break;
                case 3:
                    button3.Text = "Normal Play";
                    timer1.Interval = 200;
                    break;
            }
            fastcounter = (fastcounter + 1) % 4;
            trackBar1.Value = timer1.Interval;
            label3.Text = trackBar1.Value.ToString() + " ms";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            images.Clear();
            pictureBox1.Image = null;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string szijjarto = folderBrowserDialog1.SelectedPath, fn;
                foreach (string fullpathfn in Directory.GetFiles(szijjarto))
                {
                    Image img = Image.FromFile(fullpathfn);
                    images.Add(img);

                    fn = Path.GetFileName(fullpathfn); 
                    listBox1.Items.Add(fn);            
                }
                trackBar2.Minimum = 1;               
                trackBar2.Maximum = listBox1.Items.Count;

                
                comboBox1.Enabled = true;
            }
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button5.Visible = true;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int si = listBox1.SelectedIndex;
            pictureBox1.Image = images[si];
            trackBar2.Value = si + 1;
            label4.Text = trackBar2.Value.ToString() + ". pic";

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString() + " msec";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar2.Value.ToString() + ". pic";
            listBox1.SelectedIndex = trackBar2.Value - 1;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = true;
            button6.Visible = true;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(images[listBox1.SelectedIndex]);
            Graphics g = Graphics.FromImage(bmp);

            switch (comboBox1.SelectedIndex)
            {
                case 0: 
                    bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case 1: 
                    bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                case 2: 
                    bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 3: 
                    g.DrawString("szempöl tekszt", new Font("Arial", 20), Brushes.Red, new PointF(10, 10));
                    break;
                case 4: 
                    bmp = new Bitmap(bmp, new Size(bmp.Width / 2, bmp.Height / 2));
                    break;
                case 5: 
                    g.DrawString("Vótör Márk", new Font("Arial", 20), Brushes.Blue, new PointF(bmp.Width - 100, bmp.Height - 30));
                    break;
            }

            g.Dispose();
            images[listBox1.SelectedIndex] = bmp;
            pictureBox1.Image = bmp;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (Graphics g = this.CreateGraphics())
            {
                Rectangle rect = new Rectangle(pictureBox1.Left - 5, pictureBox1.Top - 5, pictureBox1.Width + 10, pictureBox1.Height + 10);
                using (Pen pen = new Pen(Color.Green, 3))
                {
                    g.DrawRectangle(pen, rect);
                }
            }
        }
    }
}
