using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalYearProject
{
    public partial class Result_Page : Form
    {
        List<string> results = Form1.results_;

        public Result_Page()
        {
            InitializeComponent();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("IDD");
            dataTable.Columns.Add("Image");
            dataTable.Columns.Add("Prediction");
            dataTable.Columns.Add("Probability");
            int count = 0;
            foreach (var a in results)
            {
                var Id = 0;
                var ImagePath = "";
                var Prediction = "";
                var Probability = "";
                if (a.Contains("1.0"))
                {
                    Id = count;
                    ImagePath = a.Substring(128);
                    Prediction = a.Substring(51, 1);
                    Probability = a.Substring(76, 3);
                }
                else
                {
                    Id = count;
                    ImagePath = a.Substring(131);
                    Prediction = a.Substring(51, 1);
                    Probability = a.Substring(76, 6);
                }
                dataTable.Rows.Add(Id, ImagePath, Prediction, Probability);

                count++;
            }

            dataGridView2.DataSource = dataTable;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            var count = dataGridView2.SelectedRows.Count;
            if (count < 1)
            {
                MessageBox.Show("Select a row,not the cell to view image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var image_path = dataGridView2.SelectedRows[0].Cells;
                var a = image_path[1].Value;

                pictureBox1.ImageLocation = a.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 from = new Form1();
            from.ShowDialog();
            this.Close();
        }
    }
}
