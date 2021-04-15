using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyşeHafızaOyunu
{
    public partial class Form1 : Form
    {
        PictureBox BeforeImage = new PictureBox();

        #region PROPERTY
        #region Private property
        private int clickCount = 0;
        private int playerRank = 2;
        private int evenNumber = 0; // Çift sayı
        private int remaining = 0; // Kalan
        #endregion

        #region Public property
        public int ClickCount
        {
            get { return clickCount; }
            set { clickCount = value; }
        }
        public int PlayerRank
        {
            get { return playerRank; }
            set { playerRank = value; }
        }
        public int EvenNumber
        {
            get { return evenNumber; }
            set { evenNumber = value; }
        }
        public int Remaining
        {
            get { return remaining; }
            set { remaining = value; }
        }
        #endregion
        #endregion

        #region CONSTRUCTOR
        public Form1()
        {
            InitializeComponent();
            timer1Started.Interval = 1000;
            timer1Selected.Interval = 1000;
        }
        #endregion

        #region EVENT

        #region FormLoad
        private void Form1_Load(object sender, EventArgs e)
        {
            Restart();
        }
        #endregion

        #region TIMERLAR

        #region Timer1Started --> Oyunun başlamasına 5 saniye kala olayı
        private void timer1Started_Tick(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(lblPlayStartTime.Text); //5
            count--;
            lblPlayStartTime.Text = count.ToString();

            #region 5 saniye biter ve soru işaretleri gözükür. Oyun başlar .. 
            if (count == 0)
            {
                #region Oyun başladıktan sonra oyunun başlamasına 5 saniye olan sayaç ve labeli gizleniyor ...
                label3.Hide();
                lblPlayStartTime.Hide();
                #endregion

                timer1Started.Stop();
                GetImageMark(); // Soru işareti resimleri gelir
            }
            #endregion

        }
        #endregion

        #region Timer1Selected --> 2. kartı seçmek için 5 saniye kala olayı.
        private void timer1Selected_Tick(object sender, EventArgs e)
        {
            int selectedTime = Convert.ToInt32(lblSelectReamingTime.Text); //5
            selectedTime--;
            lblSelectReamingTime.Text = selectedTime.ToString();

            #region 2. kartı seçmek için belirlenen 5 saniye süre bittiğinde ... 
            if (selectedTime == 0)
            {
                lblSelectReamingTime.Text = "0";
                timer1Selected.Stop();
                BeforeImage.Image = ımageList1.Images[0]; // Soru işaretleri görünür .. 
                lblSelectReamingTime.Text = "5";
                playerRank = playerRank + 1;
                PlayerRankFind(); // Oyuncu geçişi
                ClickCount = 0;
                return;
            }
            #endregion
        }
        #endregion

        #endregion

        #region PictureBox
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox nowImage = (sender as PictureBox); 
            nowImage.Image = ımageList1.Images[(int)nowImage.Tag];
            panel1.Refresh();
            panel1.Refresh();
            Thread.Sleep(1000);

            #region İkinci Tıklama
            if (ClickCount == 1)
            {
                if (BeforeImage == nowImage)
                {
                    MessageBox.Show("Lütfen tekrar farklı bir resimle deneyiniz. Önceki resimle aynı olamaz ... ");
                    return;
                }

                lblSelectReamingTime.Text = "5";
                timer1Selected.Stop();

                Thread.Sleep(2000);

                #region Önceki resim ile sonraki resmin eşit olma durumu 
                if (BeforeImage.Tag.ToString() == nowImage.Tag.ToString())
                {
                    ScoreOfSystem(); // Puanlama Sistemi

                    #region Resimler Gizleniyor...
                    nowImage.Hide();
                    BeforeImage.Hide();
                    #endregion

                    lblRemaining.Text = (--remaining).ToString();
                    if (remaining == 0)
                    {
                        MessageBox.Show("Kart kalmadı. Yeniden başlatılıyor ... ");
                        Restart();
                    }

                }
                #endregion

                #region Önce seçilen resim ile sonra seçilen resmin farklı olma durumu 
                else
                {
                    ClickCount = 0;
                    GetImageMark(); // Resim Gizleme
                    playerRank += 1;
                    PlayerRankFind();
                    return;
                }
                #endregion

                ClickCount = 0;
                BeforeImage = null;
                return;
            }
            #endregion

            #region Birinci Tıklama
            if (ClickCount == 0)
            {
                timer1Selected.Start();
                BeforeImage = null;
                BeforeImage = nowImage;
                ClickCount = 1;
                PlayerRankFind();
            }
            #endregion
        }
        #endregion

        #region Yeniden Başlat Butonu 
        private void btnRestart_Click(object sender, EventArgs e)
        {
            Restart();
        }
        #endregion

        #endregion

        #region METHODS

        #region Yeniden Başlatma
        public void Restart()
        {
            evenNumber = ımageList1.Images.Count - 2;
            remaining = evenNumber;

            lblRemaining.Text = remaining.ToString(); // Kalan kart
            label3.Show();
            lblPlayStartTime.Show();
            lblPlayStartTime.Text = "5";
            lblSelectReamingTime.Text = "5";
            PictureTag();
            BeforeImage = null;
            ImageShow();
            lblFirstPlayer.Text = "0";
            lblSecondPlayer.Text = "0";
            timer1Started.Start();
        }
        #endregion

        #region Resim Tag Doldurma
        public void PictureTag()
        {
            ArrayList arrayListTag = new ArrayList();
            for(int i = 0; i < evenNumber * 2; i++)
            {
                arrayListTag.Add((i % evenNumber) + 1);
            }

            Random rnd = new Random();
            foreach(PictureBox picture in panel1.Controls)
            {
                int assValue = rnd.Next(arrayListTag.Count);
                picture.Tag = arrayListTag[assValue];
                picture.Show();
                arrayListTag.RemoveAt(assValue);
            }
        }
        #endregion

        #region Resim Gösterme
        public void ImageShow()
        {
            foreach(PictureBox picture in panel1.Controls)
            {
                picture.Image = ımageList1.Images[(int)picture.Tag];
            }
        }
        #endregion

        #region 5 saniye sonra oyun başladığında soru işaretleri gösterilir. / Image Hide
        public void GetImageMark()
        {
            foreach(PictureBox picture in panel1.Controls)
            {
                picture.Image = ımageList1.Images[0]; // image[0] = Soru işaretidir.
            }
        }
        #endregion

        #region Oyuncu Sırası Belirleme
        public void PlayerRankFind()
        {
            if (playerRank % 2 == 0)
            {
                lblPlayerRank.Text = "1";
            }
            if (playerRank % 2 != 0)
            {
                lblPlayerRank.Text = "2";
            }
        }
        #endregion

        #region Puanlama Sistemi
        void ScoreOfSystem()
        {
            int playerFirstScore = Convert.ToInt32(lblFirstPlayer.Text);
            int playerSecondScore = Convert.ToInt32(lblSecondPlayer.Text);

            if(lblPlayerRank.Text == "1")
            {
                playerFirstScore = playerFirstScore + 10;
            }
            if (lblPlayerRank.Text == "2")
            {
                playerSecondScore = playerSecondScore + 10;
            }
            #region Puanın 110 olma durumu - kazanılması 
            if (lblFirstPlayer.Text == "110")
            {
                DialogResult result = MessageBox.Show("Tebrikler Oyuncu-1 kazandı. Tekrar oynamak ister misiniz ? ", "OYUNCU-1 KAZANDI", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (result == DialogResult.Yes)
                {
                    Application.Restart();
                }
                if (result == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            if (lblSecondPlayer.Text == "110")
            {
                DialogResult result = MessageBox.Show("Tebrikler Oyuncu-2 kazandı. Tekrar oynamak ister misiniz ? ", "OYUNCU-2 KAZANDI", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (result == DialogResult.Yes)
                {
                    Application.Restart();
                }
                if (result == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            #endregion
        }
        #endregion

        #endregion      
    }
}
