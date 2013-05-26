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
    public partial class Form1 : Form
    {
        Timer timer;
        Graphics graphics;
        Bitmap doubleBuffer;
        static readonly int TIMER_INTERVAL = 1000;
        public static int N;
        public int Kocka { get; set; }
        char[][] LabyrinthWorld;
        Ball ball;
        int end_i, end_j;
        int NegPoeni, VkPoeni;
        int Nivo;
        int time, f, pat, krajFlag, FirstPress, ff, prikaziPateka, restartFlag;
        string Igrac;
        List<Koordinata> pateka;

        public Form1(int n, int nivo, int poeni, int t, string igrac)
        {
            InitializeComponent();
            Top = Left = 50;
            ff = 0;
            prikaziPateka = 0;
            restartFlag = 0;
            pateka = new List<Koordinata>();
            doubleBuffer = new Bitmap(LavPanel.Width, LavPanel.Height);

            Restart.Image = Image.FromFile("Restart.jpg");
            Restart.SizeMode = PictureBoxSizeMode.StretchImage;
            GiveUp.Image = Image.FromFile("WhiteFlag.jpg");
            GiveUp.SizeMode = PictureBoxSizeMode.StretchImage;

            Igrac = igrac;
            ImeLbl.Text = string.Format("{0}", Igrac);
            time = t;
            TimeBar.Maximum = t;
            TimeBar.Value = t;
            NegativniBar.Maximum = poeni;
            f = 1;
            pat = 0;
            krajFlag = 0;

            FirstPress = 0;

            TimeLbl.Text = string.Format("{0}:{1}{2}", time / 60, (time - (time / 60) * 60) / 10,(time - (time / 60) * 60)-((time - (time / 60) * 60) / 10)*10);
            this.Text = string.Format("Smart Maze - Ниво {0}", nivo);

            Nivo = nivo;
            NivoLbl.Text = string.Format("Ниво {0}", Nivo);
            VkPoeni = poeni;
            NegPoeni = 0;
            NegLbl.Text = string.Format("{0}/{1}", NegPoeni, VkPoeni);
            N = n;
            LabyrinthWorld = Labyrinth.generirajLavirint(N);
            
            Kocka = (doubleBuffer.Width) / (LabyrinthWorld.Length);            

            newGame();
        }

        public void newGame()
        {
            graphics = CreateGraphics();
            for (int i = 0; i < LabyrinthWorld.Length; i++)
                for (int j = 0; j < LabyrinthWorld[0].Length; j++)
                    if (LabyrinthWorld[i][j] == 'S')
                    {
                        ball = new Ball((j) * Kocka, (i) * Kocka, Kocka);
                        ball.start_i = i;
                        ball.start_j = j;
                        ball.Draw(graphics);
                    }
                    else if (LabyrinthWorld[i][j] == 'E')
                    {
                        end_i = i;
                        end_j = j;
                    }

            Invalidate();
            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TIMER_INTERVAL;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            time--;
            TimeBar.Value--;
            TimeLbl.Text = string.Format("{0}:{1}{2}", time / 60, (time - (time / 60) * 60) / 10, (time - (time / 60) * 60) - ((time - (time / 60) * 60) / 10)*10);
            Invalidate();
            if (time == 0)
            {
                krajFlag = 1;
                Kraj(string.Format("{0}, вашето време истече. Крај на играта!", Igrac));
            }

        }

        private void LavPanel_Paint(object sender, PaintEventArgs e)
        {
            if (f == 1)
            {
                pat++;
                Graphics g = Graphics.FromImage(doubleBuffer);
                g.Clear(Color.White);

                for (int i = 0; i < LabyrinthWorld.Length; i++)
                    for (int j = 0; j < LabyrinthWorld[0].Length; j++)
                    {
                        if (LabyrinthWorld[i][j] == '#')
                            g.FillRectangle(new SolidBrush(Color.DodgerBlue), new Rectangle((j) * Kocka, (i) * Kocka, Kocka, Kocka));
                        else if (LabyrinthWorld[i][j] == ' ')
                            g.FillRectangle(new SolidBrush(Color.White), new Rectangle((j) * Kocka, (i) * Kocka, Kocka, Kocka));
                        else if (LabyrinthWorld[i][j] == 'E')
                            g.DrawImage(Image.FromFile("door.jpg"), (j) * Kocka, (i) * Kocka, Kocka, Kocka);
                        else if (LabyrinthWorld[i][j] == 'S')
                            g.DrawImage(Image.FromFile("castle.jpg"), (j) * Kocka, (i) * Kocka, Kocka, Kocka);
                        else if (LabyrinthWorld[i][j] != '#' && prikaziPateka == 1)
                            g.FillEllipse(new SolidBrush(Color.Red), new Rectangle((j) * Kocka + Kocka/5, (i) * Kocka + Kocka/5, Kocka-2*Kocka/5, Kocka-2*Kocka/5));
                    }

                int staro_i = ball.start_i;
                int staro_j = ball.start_j;

                if (FirstPress == 1)
                    ball.Move(Kocka);
                if(prikaziPateka==0)
                    ball.Draw(g);

                g.FillRectangle(new SolidBrush(SystemColors.Control), LabyrinthWorld.Length*Kocka, 0, LavPanel.Width-LabyrinthWorld.Length*Kocka, LavPanel.Width);
                g.FillRectangle(new SolidBrush(SystemColors.Control), 0, LabyrinthWorld.Length * Kocka, LavPanel.Width, LavPanel.Width - LabyrinthWorld.Length * Kocka);

                int postoi = 0;
                for (int i = 0; i < pateka.Count; i++)
                    if (ball.start_i==pateka[i].i && ball.start_j==pateka[i].j)
                        postoi = 1;

                Koordinata kor = new Koordinata(ball.start_i, ball.start_j);
                if (postoi == 0)
                    pateka.Add(kor);
                else if(ff == 1 && prikaziPateka==0 && restartFlag!=1)
                {
                    NegPoeni++;
                    NegativniBar.Value++;
                    NegLbl.Text = string.Format("{0}/{1}", NegPoeni, VkPoeni);
                    pateka.Clear();

                    if (NegPoeni == VkPoeni)
                    {
                        krajFlag = 1;
                        Kraj(string.Format("{0}, го постигнавте максималниот број на негативни поени. Крај на играта!", Igrac));
                    }
                }

                if (restartFlag == 1 && pat==2)
                    restartFlag = 0;

                ff = 0;
                e.Graphics.DrawImageUnscaled(doubleBuffer, 0, 0);

                if (ball.start_i == end_i && ball.start_j == end_j)
                {
                    krajFlag = 1;
                    f = 0;
                    timer.Stop();
                }
                if (pat > 1)
                {
                    f = 0;
                    FirstPress = 1;
                }
                if (pat == 2)
                    pateka.Clear();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            f = 1;
            ball.ChangeDirection(Ball.DIRECTION.NONE);
            if (e.KeyCode == Keys.Up && ball.start_i > 0)
            {
                if (LabyrinthWorld[ball.start_i - 1][ball.start_j] != '#')
                {
                    ff = 1;
                    ball.ChangeDirection(Ball.DIRECTION.UP);
                }
            }
            if (e.KeyCode == Keys.Down && ball.start_i < LabyrinthWorld.Length-1)
            {
                if (LabyrinthWorld[ball.start_i + 1][ball.start_j] != '#')
                {
                    ff = 1;
                    ball.ChangeDirection(Ball.DIRECTION.DOWN);
                }
            }
            if (e.KeyCode == Keys.Left && ball.start_j > 0)
            {
                if (LabyrinthWorld[ball.start_i][ball.start_j - 1] != '#')
                {
                    ff = 1;
                    ball.ChangeDirection(Ball.DIRECTION.LEFT);
                }
            }
            if (e.KeyCode == Keys.Right && ball.start_j < LabyrinthWorld.Length - 1)
            {
                if (LabyrinthWorld[ball.start_i][ball.start_j + 1] != '#')
                {
                    ff = 1;
                    ball.ChangeDirection(Ball.DIRECTION.RIGHT);
                }
            }
            LavPanel.Invalidate();

            if ((LabyrinthWorld[ball.start_i - 1][ball.start_j] == 'E' && ball.Direction == Ball.DIRECTION.UP
                || LabyrinthWorld[ball.start_i + 1][ball.start_j] == 'E' && ball.Direction == Ball.DIRECTION.DOWN
                || LabyrinthWorld[ball.start_i][ball.start_j - 1] == 'E' && ball.Direction == Ball.DIRECTION.LEFT
                || LabyrinthWorld[ball.start_i][ball.start_j + 1] == 'E' && ball.Direction == Ball.DIRECTION.RIGHT)
                && prikaziPateka==0)
            {
                if (Nivo == 5)
                {
                    DialogResult result = MessageBox.Show(string.Format("ЧЕСТИТКИ ЗА ПОБЕДАТА {0}!\nГо поминавте последното ниво на играта.\nСакате ли да играте од првото ниво повторно?", Igrac), "Крај на игра", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        StartForm.SlednoNivo = 1;
                        this.Close();
                    }
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        this.Close();
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show(string.Format("ЧЕСТИТКИ ЗА ПОБЕДАТА {0}!\nСакате ли да прејдете на следното ниво?", Igrac), "Прејди на следно ниво?", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (Nivo == 1)
                            StartForm.SlednoNivo = 2;
                        if (Nivo == 2)
                            StartForm.SlednoNivo = 3;
                        if (Nivo == 3)
                            StartForm.SlednoNivo = 4;
                        if (Nivo == 4)
                            StartForm.SlednoNivo = 5;
                        this.Close();
                    }
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        this.Close();
                    }
                }
            }      
        }

        public void Kraj(string message)
        {
            timer.Stop();
            TimeBar.Value = 0;
            MessageBox.Show(message);
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (krajFlag != 1 && prikaziPateka==0)
            {
                timer.Stop();
                DialogResult result = MessageBox.Show(string.Format("{0}, по излезот од тековниот прозорец вашата игра нема да биде зачувана.\nДали сте сигурен дека сакате да ја напуштите играта?", Igrac), "Излез од играта?", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    StartForm.SlednoNivo = -1;
                }
                if (result == System.Windows.Forms.DialogResult.No)
                {   
                    e.Cancel = true;
                    timer.Start();
                }
            }
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            restartFlag = 1;
            LabyrinthWorld = Labyrinth.generirajLavirint(N);
            timer.Stop();
            prikaziPateka = 0;
            f = 1;
            pat = 0;
            krajFlag = 0;
            FirstPress = 0;

            time = TimeBar.Maximum;
            TimeBar.Value = TimeBar.Maximum;
            NegativniBar.Value = NegativniBar.Minimum;
            newGame();
            
            LavPanel.Invalidate();
            KeyEventArgs ee = new KeyEventArgs(Keys.Enter);
            NegPoeni = 0;
            NegLbl.Text = string.Format("{0}/{1}", NegPoeni, VkPoeni);
        }

        private void GiveUp_Click(object sender, EventArgs e)
        {
            if (prikaziPateka != 1)
            {
                LabyrinthWorld = Labyrinth.najdiPatekaVoRandomLavirint_DFS(LabyrinthWorld);
                prikaziPateka = 1;
                timer.Stop();
                TimeBar.Enabled = false;
                TimeBar.Value = TimeBar.Minimum;
                NegativniBar.Enabled = false;
                NegativniBar.Value = NegativniBar.Minimum;
                TimeLbl.Text = string.Format("0:00");
                LavPanel.Invalidate();
                KeyEventArgs ee = new KeyEventArgs(Keys.Enter);
                Form1_KeyDown(sender, ee);
            }
        }

        private void Restart_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Restart_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void GiveUp_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void GiveUp_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }       

    }
}
