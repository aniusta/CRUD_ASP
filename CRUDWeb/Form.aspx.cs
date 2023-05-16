using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace CRUDWeb
{
    public partial class Contact : Page
    {
        string connectionString = "Server=localhost;Port=3306;Database=crud;Uid=root;Pwd=;";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected bool ValidateForm()
        {
            bool isValid = true;

            // Validate Name
            if (string.IsNullOrEmpty(fullname.Text))
            {
                isValid = false;
                fullname.CssClass += " error";
            }

            // Validate Address
            if (string.IsNullOrEmpty(address.Text))
            {
                isValid = false;
                address.CssClass += " error";
            }

            // Validate Date of Birth
            if (calendarDOB.SelectedDate == DateTime.MinValue)
            {
                isValid = false;
            }

            // Validate Civil Status
            if (string.IsNullOrEmpty(civilStatusDropdown.SelectedValue))
            {
                isValid = false;
                civilStatusDropdown.CssClass += " error";
            }

            // Validate Photo Upload
            if (!photoUpload.HasFile)
            {
                isValid = false;
                // You can add a CSS class to the parent container or apply styling to the file upload control itself.
                // Example:
                photoUpload.CssClass += " error";
            }

            return isValid;
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                // Form is valid, process the submission

                // Get the form values
                string name = fullname.Text;
                string address = this.address.Text;
                DateTime dateOfBirth = calendarDOB.SelectedDate;
                string gender = genderRadioList.SelectedValue;
                string civilStatus = civilStatusDropdown.SelectedValue;

                // Check if a file is uploaded
                if (photoUpload.HasFile)
                {
                    try
                    {
                        // Specify the directory to save the uploaded file
                        string uploadDirectory = Server.MapPath("~/Uploads/");
                        // Generate a unique filename for the uploaded file
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoUpload.FileName);
                        // Save the uploaded file to the server
                        photoUpload.SaveAs(Path.Combine(uploadDirectory, fileName));

                        // Save the file location in the database
                        string profilePictureLocation = "~/Uploads/" + fileName;

                        // Create a connection to the MySQL database
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            // Open the database connection
                            connection.Open();

                            // Prepare the SQL query to insert the form data into the database
                            string query = "INSERT INTO profile (name, address, date_of_birth, gender, civil_status, profile_picture_location) " +
                                "VALUES (@name, @address, @dateOfBirth, @gender, @civilStatus, @profilePictureLocation)";

                            // Create a command object with the query and connection
                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                // Set the parameter values
                                command.Parameters.AddWithValue("@name", name);
                                command.Parameters.AddWithValue("@address", address);
                                command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                                command.Parameters.AddWithValue("@gender", gender);
                                command.Parameters.AddWithValue("@civilStatus", civilStatus);
                                command.Parameters.AddWithValue("@profilePictureLocation", profilePictureLocation);

                                // Execute the query
                                command.ExecuteNonQuery();
                            }

                            // Close the database connection
                            connection.Close();
                        }
                        // Display success message in a popup
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "UploadSuccess", "alert('File uploaded successfully!');", true);

                        // Reset the form values to empty
                        fullname.Text = string.Empty;
                        this.address.Text = string.Empty;
                        calendarDOB.SelectedDate = DateTime.MinValue;
                        genderRadioList.ClearSelection();
                        civilStatusDropdown.ClearSelection();
                        photoUpload.Dispose(); // Clear the uploaded file

                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (e.g., display an error message)
                        errorLabel.Text = "An error occurred while processing the submission. Please try again later.";
                        errorLabel.Visible = true;
                        return;
                    }
                }
            }
        }
    }
}