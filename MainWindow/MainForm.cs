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

namespace MainWindow
{

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void VIEWALL_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=StOrmeR;Database=Todolist;Trusted_Connection=True;";
            string query = "SELECT * FROM CONGVIEC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    List.DataSource = dataTable;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            panel3.Visible = false;
            DateTime currentTime = DateTime.Now;
            String[] ct_array = currentTime.GetDateTimeFormats('D');
            label4.Text = ct_array[0];

            string connectionString = "Server=StOrmeR;Database=Todolist;Trusted_Connection=True;";
            string query = "SELECT * FROM CONGVIEC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    List.DataSource = dataTable;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=StOrmeR;Database=Todolist;Trusted_Connection=True;";

            string taskName = Taskname_tb.Text;
            string description = Description_tb.Text;
            int priority = int.Parse(Priority_tb.Text);
            DateTime deadline = Deadline_dtp.Value.Date;

            string query = $"INSERT INTO CONGVIEC (TASKNAME, DESCRIPTION_TASK, DEADLINE, PRIORITY_TASK, DONE) VALUES " +
                $"('{taskName}', '{description}', '{deadline:yyyy-MM-dd}', {priority}, 0)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();

                }
            }

            // Clear the input fields
            Taskname_tb.Text = "";
            Description_tb.Text = "";
            Priority_tb.Text = "";
            Deadline_dtp.Value = DateTime.Now;

            // Refresh the DataGridView to reflect the newly added data

        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (List.SelectedRows.Count > 0)
            {
                // Check if the input elements have data
                if (!string.IsNullOrEmpty(Taskname_tb.Text) && !string.IsNullOrEmpty(Description_tb.Text) && !string.IsNullOrEmpty(Priority_tb.Text))
                {
                    // Get the index of the selected row
                    int rowIndex = List.SelectedRows[0].Index;

                    // Retrieve the values from the input elements
                    string taskName = Taskname_tb.Text;
                    string description = Description_tb.Text;
                    int priority = int.Parse(Priority_tb.Text);
                    DateTime deadline = Deadline_dtp.Value.Date;

                    // Update the corresponding values in the selected row
                    List.Rows[rowIndex].Cells["TASKNAME"].Value = taskName;
                    List.Rows[rowIndex].Cells["DESCRIPTION_TASK"].Value = description;
                    List.Rows[rowIndex].Cells["PRIORITY_TASK"].Value = priority;
                    List.Rows[rowIndex].Cells["DEADLINE"].Value = deadline;

                    // Clear the input fields
                    Taskname_tb.Text = "";
                    Description_tb.Text = "";
                    Priority_tb.Text = "";
                    Deadline_dtp.Value = DateTime.Now;
                }
                else
                {
                    MessageBox.Show("Please enter all the required information.");
                }
            }
            else
            {
                MessageBox.Show("Please select a row in the DataGridView.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (List.SelectedRows.Count > 0)
            {
                // Get the index of the selected row
                int rowIndex = List.SelectedRows[0].Index;

                // Get the primary key value from the selected row
                string primaryKeyValue = List.Rows[rowIndex].Cells["TASKNAME"].Value.ToString();

                // Delete the corresponding data from the database
                DeleteDataFromDatabase(primaryKeyValue);

                // Remove the selected row from the DataGridView
                List.Rows.RemoveAt(rowIndex);
            }
            else
            {
                MessageBox.Show("Please select a row in the DataGridView.");
            }
        }

        private void DeleteDataFromDatabase(string primaryKeyValue)
        {
            string connectionString = "Server=StOrmeR;Database=Todolist;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a SQL command to delete the row with the specified primary key
                string sql = "DELETE FROM CONGVIEC WHERE TASKNAME = @PrimaryKeyValue";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Set the value of the primary key parameter
                    command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);

                    // Execute the SQL command
                    command.ExecuteNonQuery();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
                string selectedSortItem = comboBox1.SelectedItem.ToString();

                // Sort the data in the DataGridView based on the selected item
                switch (selectedSortItem)
                {
                    case "DEFAULT":
                        // No sorting needed, display the data as it is
                        break;
                    case "Name (A-Z)":
                        List.Sort(List.Columns["TASKNAME"], ListSortDirection.Ascending);
                        break;
                    case "Name (Z-A)":
                        List.Sort(List.Columns["TASKNAME"], ListSortDirection.Descending);
                        break;
                    case "DEADLINE":
                        List.Sort(List.Columns["DEADLINE"], ListSortDirection.Ascending);
                        break;
                    case "PRIORITY":
                        List.Sort(List.Columns["PRIORITY_TASK"], ListSortDirection.Ascending);
                        break;
                    default:
                        break;
                } 

        }
    }
}

