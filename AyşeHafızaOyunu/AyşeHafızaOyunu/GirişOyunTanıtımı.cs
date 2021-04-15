using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyşeHafızaOyunu
{
    public partial class GirişOyunTanıtımı : Form
    {
        #region CONSTUCTOR
        public GirişOyunTanıtımı()
        {
            InitializeComponent();
        }
        #endregion

        #region Çıkış Yap Butonu
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkış yapmak istediğinize emin misiniz ? ", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                return;
            }
        }
        #endregion

        #region Oyuna Başla Butonu
        private void btnStart_Click(object sender, EventArgs e)
        {
            Form1 go = new Form1();
            this.Hide();
            go.Show();
        }
        #endregion
    }
}
