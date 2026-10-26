using System;
using System.IO;
using System.Windows.Forms;

namespace faylmeneceri
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnQovluq_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtYol.Text = fbd.SelectedPath;
                    lstFayllar.Items.Clear();

                    string[] fayllar = Directory.GetFiles(fbd.SelectedPath);

                    foreach (string fayl in fayllar)
                    {
                        lstFayllar.Items.Add(Path.GetFileName(fayl));
                    }

                    lblStatus.Text = $"Hazırdır | Fayl sayı: {fayllar.Length}";
                }
            }
        }

        private void lstFayllar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFayllar.SelectedItem != null)
            {
                string secilmisFaylAdi = lstFayllar.SelectedItem.ToString();
                string tamYol = Path.Combine(txtYol.Text, secilmisFaylAdi);

                if (File.Exists(tamYol))
                {
                    FileInfo fi = new FileInfo(tamYol);
                    double olcuKB = fi.Length / 1024.0;

                    lblMelumat.Text = $"Ad: {fi.Name}   Ölçü: {olcuKB:F1} KB   Yaranma tarixi: {fi.CreationTime.ToShortDateString()}";
                }
            }
        }

        private void btnAc_Click(object sender, EventArgs e)
        {
            if (lstFayllar.SelectedItem != null)
            {
                string secilmisFaylAdi = lstFayllar.SelectedItem.ToString();
                string tamYol = Path.Combine(txtYol.Text, secilmisFaylAdi);

                if (File.Exists(tamYol))
                {
                    txtMetn.Text = File.ReadAllText(tamYol);
                }
            }
            else
            {
                MessageBox.Show("Zəhmət olmasa siyahıdan açmaq üçün bir fayl seçin!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaxla_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                if (!string.IsNullOrEmpty(txtYol.Text))
                {
                    sfd.InitialDirectory = txtYol.Text;
                }

                sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                sfd.Title = "Faylı yadda saxla";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, txtMetn.Text);

                    MessageBox.Show("Fayl uğurla yadda saxlanıldı!", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!string.IsNullOrEmpty(txtYol.Text) && sfd.FileName.StartsWith(txtYol.Text))
                    {
                        lstFayllar.Items.Clear();
                        string[] fayllar = Directory.GetFiles(txtYol.Text);
                        foreach (string fayl in fayllar)
                        {
                            lstFayllar.Items.Add(Path.GetFileName(fayl));
                        }
                        lblStatus.Text = $"Hazırdır | Fayl sayı: {fayllar.Length}";
                    }
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (lstFayllar.SelectedItem != null)
            {
                string secilmisFaylAdi = lstFayllar.SelectedItem.ToString();
                string tamYol = Path.Combine(txtYol.Text, secilmisFaylAdi);

                DialogResult cavab = MessageBox.Show($"'{secilmisFaylAdi}' faylını silmək istədiyinizdən əminsinizmi?",
                                                      "Silməni Təsdiq Et",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (cavab == DialogResult.Yes)
                {
                    if (File.Exists(tamYol))
                    {
                        File.Delete(tamYol);

                        lstFayllar.Items.Remove(lstFayllar.SelectedItem);
                        txtMetn.Clear();
                        lblMelumat.Text = "";

                        lblStatus.Text = $"Hazırdır | Fayl sayı: {lstFayllar.Items.Count}";

                        MessageBox.Show("Fayl uğurla silindi.", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Zəhmət olmasa silmək üçün siyahıdan bir fayl seçin!", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}