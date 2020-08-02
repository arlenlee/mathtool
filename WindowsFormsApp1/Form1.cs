﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;


namespace MathpixCsharp
{
    public partial class Form1 : Form
    {
        GetCode gg = new GetCode();
        Bitmap bit;
        string app_id="";
        string app_key = "";

        public Bitmap Bit { get => bit; set => bit = value; }

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.id == string.Empty)
            {
                login t = new login();
                t.StartPosition = FormStartPosition.CenterParent;
                t.ShowDialog();
                Properties.Settings.Default.Save();
            }
            else
            {
                app_id = Properties.Settings.Default.id;
                app_key = Properties.Settings.Default.key;
                //MessageBox.Show(app_id);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if ((int)e.Modifiers == ((int)Keys.Control + (int)Keys.Alt)&&e.KeyCode==Keys.M)
            {
                //
            }
        }
        private async Task<bool> ScreenShotToCode(Bitmap bit)
        {
            gg.SetImg(bit);
            try
            {
                List<string> codeList = await gg.GetLatex();
                textBox1.Text = codeList[0];
                textBox2.Text = codeList[1];
                textBox3.Text = codeList[2];
            }
            catch(Exception e)
            {
                //MessageBox.Show("Error: {0}", e.Message);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
            return true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button2.Text = "重试";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            button3.Text = "复制inline";
            button4.Text = "复制dispaly";
            button5.Text = "复制MathML";
            ScreenShot sf= new ScreenShot();
            sf.Owner = this;
            this.Opacity = 0.0;
            sf.ShowDialog();//make sure it's done
            this.pictureBox1.Image = Bit;
            await ScreenShotToCode(Bit);
            this.Opacity = 1.0;
            if (inline.Checked == true)
                Clipboard.SetText(textBox1.Text);
            else if (display.Checked == true)
                Clipboard.SetText(textBox2.Text);
            else if (MathML.Checked == true)
                Clipboard.SetText(textBox3.Text);
        }

        private void button2_Click(object sender, EventArgs e)//retry
        {
            try
            {
                ScreenShotToCode(Bit);
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("错误，请重试");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(textBox1.Text);
                button3.Text = "复制成功！";
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("错误，代码为空！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(textBox2.Text);
                button4.Text = "复制成功！";
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("错误，代码为空！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(textBox3.Text);
                button5.Text = "复制成功！";
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("错误，代码为空！");
            }
        }

    }
}
