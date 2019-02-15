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

namespace FinalYearProject
{
    public partial class Form1 : Form
    {
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


        private void button1_Click(object sender, EventArgs e)
        {
            var client = new RestClient("http://127.0.0.1:8881/models/api_test/v2/predict");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "53711025-2b90-4aae-820f-3fee76be3738");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"data\"\r\n\r\n{\"key\": \"Image\"};type=application/json\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"Image\"; filename=\"C:\\Users\\Enoch Sodiya\\Desktop\\cell_images\\Parasitized\\Parasitized (11).png\"\r\nContent-Type: image/png\r\n\r\n\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            textBox1.Text = content;
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

        private void button3_Click(object sender, EventArgs e)
        {
            string[] Imagepath = new string[dataGridView1.RowCount];

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                Imagepath[i] = dataGridView1.Rows[i].Cells[1].Value.ToString();
            }

            for (int i = 0; i < Imagepath.Count(); i++)
            {
                var client = new RestClient("http://127.0.0.1:8881/models/api_test/v2/predict");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"data\"\r\n\r\n{\"key\": \"Image\"};type=application/json\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"Image\"; filename=\"C:\\Users\\Enoch Sodiya\\Desktop\\cell_images\\Uninfected\\Uninfected (29).png\"\r\nContent-Type: image/png\r\n\r\n\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                var content = response.Content;
                textBox1.AppendText(content + " ");
            }
        }
    }
}
