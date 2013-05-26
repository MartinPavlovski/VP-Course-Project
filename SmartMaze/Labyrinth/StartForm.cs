using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Labyrinth
{
    public partial class StartForm : Form
    {
        public Form frm;
        public static int SlednoNivo;
        int valid;

        public StartForm()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("FormBack.jpg");
            BackgroundImageLayout = ImageLayout.Stretch;
            NivoCb.Items.Add("Ниво 1 (Easy)");
            NivoCb.Items.Add("Ниво 2 (Medium)");
            NivoCb.Items.Add("Ниво 3 (Hard)");
            NivoCb.Items.Add("Ниво 4 (Very Hard)");
            NivoCb.Items.Add("Ниво 5 (Expert)");
            NivoCb.SelectedIndex = 0;
            valid = 0;
        }

        private void QuitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (valid == 1)
            {
                if (NivoCb.SelectedIndex == 0)
                    frm = new Form1(5, 1, 3, 60, TbNick.Text);
                if (NivoCb.SelectedIndex == 1)
                    frm = new Form1(7, 2, 5, 120, TbNick.Text);
                if (NivoCb.SelectedIndex == 2)
                    frm = new Form1(10, 3, 5, 180, TbNick.Text);
                if (NivoCb.SelectedIndex == 3)
                    frm = new Form1(17, 4, 10, 300, TbNick.Text);
                if (NivoCb.SelectedIndex == 4)
                    frm = new Form1(25, 5, 15, 600, TbNick.Text);

                this.Hide();
                frm.ShowDialog();
                this.Show();
                if (SlednoNivo == 1)
                {
                    frm = new Form1(5, 1, 3, 60, TbNick.Text);
                    this.Hide();
                    frm.ShowDialog();
                    this.Show();
                }
                if (SlednoNivo == 2)
                {
                    frm = new Form1(7, 2, 5, 120, TbNick.Text);
                    this.Hide();
                    frm.ShowDialog();
                    this.Show();
                }
                if (SlednoNivo == 3)
                {
                    frm = new Form1(10, 3, 5, 180, TbNick.Text);
                    this.Hide();
                    frm.ShowDialog();
                    this.Show();
                }
                if (SlednoNivo == 4)
                {
                    frm = new Form1(17, 4, 10, 300, TbNick.Text);
                    this.Hide();
                    frm.ShowDialog();
                    this.Show();
                }
                if (SlednoNivo == 5)
                {
                    frm = new Form1(25, 5, 15, 600, TbNick.Text);
                    this.Hide();
                    frm.ShowDialog();
                    this.Show();
                }
            }
        }

        private void TbNick_Validating(object sender, CancelEventArgs e)
        {
            if (TbNick.Text.Trim().Length == 0)
            {
                valid = 0;
                errorProvider1.SetError(TbNick, "Внесете име на играч пред да започнете!");
            }
            else
            {
                errorProvider1.SetError(TbNick, null);
                valid = 1;
            }
        }
    }
}
