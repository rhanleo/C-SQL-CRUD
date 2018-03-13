using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SQLCRUD
{
    public partial class Form1 : Form 
    {
        string ConnString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\DotNetPro\SQLCRUD\SQLCRUD\book.mdf;Integrated Security=True";
        int Id;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            viewData();
            clear();
            btn_delete.Enabled = false;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand sqlcmd = new SqlCommand("addEdit", conn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@Id", Id);
                sqlcmd.Parameters.AddWithValue("@Title", txtbox_title.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@Author", txtbox_author.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@Description", txtbox_decription.Text.Trim());
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Data saved successfully");
                viewData();
                clear();
            }
        }

        void viewData()
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlDataAdapter dta = new SqlDataAdapter("bookViewAll", conn);
                dta.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtbl = new DataTable();
                dta.Fill(dtbl);
                dgv.DataSource = dtbl;
                dgv.Columns[0].Visible = false;
                
            }
        }

        private void clear()
        {
            txtbox_author.Text = txtbox_title.Text = txtbox_decription.Text = "";
            Id = 0;
            btn_delete.Enabled = false;
        }

        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                if(dgv.CurrentRow.Index != -1)
                {
                    txtbox_title.Text = dgv.CurrentRow.Cells[1].Value.ToString();
                    txtbox_author.Text = dgv.CurrentRow.Cells[2].Value.ToString();
                    txtbox_decription.Text = dgv.CurrentRow.Cells[3].Value.ToString();
                    Id = Convert.ToInt32(dgv.CurrentRow.Cells[0].Value.ToString());
                    btn_save.Text = "Update";
                    btn_delete.Enabled = true;

                }

            }

        }

      

        private void btn_delete_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("deleteId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", dgv.CurrentRow.Cells[0].Value);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data deleted successfully");
                clear();
                viewData();
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            clear();
        }
    }

}
