using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Crud
{

    public partial class Form1 : Form
    {

        private SqlConnection sqlConnection = new SqlConnection("Data Source=DESKTOP-CHJ6TVA;Initial Catalog=CRUDform;Integrated Security=True;Encrypt=False");
        private SqlCommand command = new SqlCommand();
        private object txtStudentId;
        private object txtNewName;

        public object ID { get; private set; }
        public object Age { get; private set; }

        public Form1()
        {
            InitializeComponent();
            command.Connection = sqlConnection;
        }


        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String ID = textBox1.Text;
            string name = textBox2.Text;
            string age = textBox3.Text;


            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(age))
            {
                MessageBox.Show("Please enter valid data before saving.");
                return;
            }

            SaveDataToFile(ID, name, age);

            MessageBox.Show("Data saved successfully!");
        }

        private void SaveDataToFile(string ID, string name, string age)
        {
            string filePath = "data.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"ID: {ID},Name: {name}, Age: {age}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();

                int ID = Convert.ToInt32(textBox1.Text);
                string name = textBox2.Text;
                int age = Convert.ToInt32(textBox3.Text);

                string insertQuery = "INSERT INTO student (ID,Name, Age) VALUES (@ID,@Name, @Age)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Age", age);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Data saved to database successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to database: {ex.Message}");
            }
            finally
            {

                sqlConnection.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
         
                    sqlConnection.Open();

                    
                    string deleteQuery = "DELETE FROM student WHERE ID = @primayKey";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@primayKey", 1);

                        int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        
                        MessageBox.Show("deleted succesfully");
                    else
                    {
                        MessageBox.Show("Student not found. Please check the student ID.");
                       
                    }
                }
                
            }
            
            catch (Exception ex)
            {
                
                MessageBox.Show($"Error: {ex.Message}");
              //  return false;
            }
            finally
            {
          sqlConnection.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int studentId;

            if (int.TryParse(textBox1.Text, out studentId))
            {
                string newName = textBox2.Text;
                int newAge;

                if (int.TryParse(textBox3.Text, out newAge))
                {
                    if (UpdateStudentInDatabase(studentId, newName, newAge))
                    {
                        MessageBox.Show("Student updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to update student. Check the student ID and database connection.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid age. Please enter a valid number.");
                }
            }
            else
            {
                MessageBox.Show("Invalid student ID. Please enter a valid number.");
            }
        }

        private bool UpdateStudentInDatabase(int studentId, string newName, int newAge)
        {
            try
            {
                // Ensure sqlConnection is initialized and openable
                if (sqlConnection == null || sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                string updateQuery = "UPDATE student SET name = @NewName, age = @NewAge WHERE ID = @StudentId";
                using (SqlCommand cmd = new SqlCommand(updateQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);  // Use local variable, not ID
                    cmd.Parameters.AddWithValue("@NewName", newName);   // Use local variable, not Name
                    cmd.Parameters.AddWithValue("@NewAge", newAge);     // Use local variable, not Age

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                // Only close the connection if it was opened in this method
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }
    }
    }
 
   

