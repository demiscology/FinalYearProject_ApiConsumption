using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using System.Data.SqlClient;
using System.Net.Http;
using System.IO;

namespace FinalYearProject
{
    public partial class Form1 : Form
    {
        public static List<string> results_ = new List<string>();
        string Connectionstring = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=MalariaDetection; Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();

            using (SqlConnection sqlConnection = new SqlConnection(Connectionstring))
            {
                sqlConnection.Open();
                using (SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM dbo.Images", sqlConnection))
                {

                    DataTable data = new DataTable();
                    sqlData.Fill(data);

                    dataGridView1.DataSource = data;
                }
                sqlConnection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ImageFileLocation = "";
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All Files(*.*)|*.*";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ImageFileLocation = openFileDialog.FileName;

                    pictureBox1.ImageLocation = ImageFileLocation;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {

            if (pictureBox1.ImageLocation == null)
            {
                MessageBox.Show("Please Select an Image First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connectionstring))
                {
                    sqlConnection.Open();

                    using (SqlCommand sqlData = new SqlCommand("INSERT INTO Images (Image_Path) VALUES ('" + this.pictureBox1.ImageLocation + "')", sqlConnection))
                    {
                        sqlData.Connection = sqlConnection;
                        sqlData.ExecuteNonQuery();
                        pictureBox1.ImageLocation = null;
                    }

                    using (SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM dbo.Images", sqlConnection))
                    {

                        DataTable data = new DataTable();
                        sqlData.Fill(data);

                        dataGridView1.DataSource = data;
                    }
                    sqlConnection.Close();
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection sql = new SqlConnection(Connectionstring))
            {
                sql.Open();

                using (SqlCommand sqlData = new SqlCommand("DELETE FROM Images"))
                {
                    sqlData.Connection = sql;
                    sqlData.ExecuteNonQuery();
                }
                using (SqlConnection sqlConnection = new SqlConnection(Connectionstring))
                {
                    sqlConnection.Open();
                    using (SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM dbo.Images", sqlConnection))
                    {

                        DataTable data = new DataTable();
                        sqlData.Fill(data);

                        dataGridView1.DataSource = data;
                    }
                    sqlConnection.Close();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            
            string[] results = new string[dataGridView1.RowCount];

            string[] Imagepath = new string[dataGridView1.RowCount];

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                Imagepath[i] = dataGridView1.Rows[i].Cells[1].Value.ToString();
            }

            for (int i = 0; i < Imagepath.Count(); i++)
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "http://127.0.0.1:8881/models/api_test/v3/predict"))
                    {

                        var multipartContent = new MultipartFormDataContent();
                        multipartContent.Add(new StringContent("{\"key\": \"Image\"};type=application/json"), "data");
                        multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(Imagepath[i])), "Image", Path.GetFileName(Imagepath[i]));
                        request.Content = multipartContent;

                        var response = await httpClient.SendAsync(request);
                        var content = await response.Content.ReadAsStringAsync();
                        //textBox1./*AppendText*/(content+Imagepath[i]);
                        results_.Add(content + Imagepath[i]);
                    }
                }

            this.Hide();
            Result_Page result_Page = new Result_Page();
            result_Page.ShowDialog();
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }
    }
}