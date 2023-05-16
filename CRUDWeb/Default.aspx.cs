using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.IO;

namespace CRUDWeb
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Port=3306;Database=crud;Uid=root;Pwd=;";
            string query = "SELECT name, address, date_of_birth, gender, civil_status, profile_picture_location FROM profile";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // Create table header row
                        TableRow headerRow = new TableRow();
                        headerRow.Cells.Add(new TableCell() { Text = "Profile Picture" });
                        headerRow.Cells.Add(new TableCell() { Text = "Name" });
                        headerRow.Cells.Add(new TableCell() { Text = "Address" });
                        headerRow.Cells.Add(new TableCell() { Text = "Date of Birth" });
                        headerRow.Cells.Add(new TableCell() { Text = "Gender" });
                        headerRow.Cells.Add(new TableCell() { Text = "Civil Status" });
                        headerRow.Cells.Add(new TableCell() { Text = "Actions" });
                        myTable.Rows.Add(headerRow);

                        // Add data rows
                        while (reader.Read())
                        {
                            TableRow dataRow = new TableRow();

                            // Create an Image control and set its properties
                            Image profileImage = new Image();
                            profileImage.ImageUrl = reader.GetString(5); // Assuming the image URL is in the 6th column
                            profileImage.Width = Unit.Pixel(100); // Set the desired width of the image

                            // Create a TableCell and add the Image control to it
                            TableCell imageCell = new TableCell();
                            imageCell.Controls.Add(profileImage);

                            // Add the TableCell to the data row
                            dataRow.Cells.Add(imageCell);

                            dataRow.Cells.Add(new TableCell() { Text = reader.GetString(0) });
                            dataRow.Cells.Add(new TableCell() { Text = reader.GetString(1) });
                            dataRow.Cells.Add(new TableCell() { Text = reader.GetDateTime(2).ToString("yyyy-MM-dd") });
                            dataRow.Cells.Add(new TableCell() { Text = reader.GetString(3) });
                            dataRow.Cells.Add(new TableCell() { Text = reader.GetString(4) });

                            // Create a TableCell for the buttons
                            TableCell actionCell = new TableCell();

                            // Create a Panel to contain the buttons
                            Panel buttonPanel = new Panel();

                            // Create the Edit ImageButton
                            ImageButton editButton = new ImageButton();
                            editButton.ImageUrl = "~/Images/edit.png";
                            editButton.CommandArgument = reader.GetString(0); // Assuming the first column contains a unique identifier
                            editButton.Command += EditButton_Command; // Attach the event handler for the edit button
                            editButton.CssClass = "image-button";

                            // Create the Delete ImageButton
                            ImageButton deleteButton = new ImageButton();
                            deleteButton.ImageUrl = "~/Images/delete.png";
                            deleteButton.CommandArgument = reader.GetString(0); // Assuming the first column contains a unique identifier
                            deleteButton.Command += DeleteButton_Command; // Attach the event handler for the delete button
                            deleteButton.CssClass = "image-button";

                            // Add the buttons to the buttonPanel
                            buttonPanel.Controls.Add(editButton);
                            buttonPanel.Controls.Add(deleteButton);

                            // Add the buttonPanel to the actionCell
                            actionCell.Controls.Add(buttonPanel);

                            // Add the actionCell to the dataRow
                            dataRow.Cells.Add(actionCell);

                            myTable.Rows.Add(dataRow);


                        }
                    }
                    reader.Close();
                }
                connection.Close();
            }
        }

        protected void DeleteButton_Command(object sender, CommandEventArgs e)
        {
            // Retrieve the unique identifier from the command argument
            string uniqueIdentifier = e.CommandArgument.ToString();
            string connectionString = "Server=localhost;Port=3306;Database=crud;Uid=root;Pwd=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT profile_picture_location FROM profile WHERE name = @name";
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", uniqueIdentifier);
                    string profilePictureLocation = command.ExecuteScalar()?.ToString();

                    // Delete the profile picture file
                    if (!string.IsNullOrEmpty(profilePictureLocation))
                    {
                        string profilePicturePath = Server.MapPath(profilePictureLocation);
                        if (File.Exists(profilePicturePath))
                        {
                            File.Delete(profilePicturePath);
                        }
                    }
                }
            }

            // Execute the SQL command to delete the record
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Assuming your table is named "profile" and the identifier column is named "name"
                string deleteQuery = $"DELETE FROM profile WHERE name = @name";

                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", uniqueIdentifier);
                    command.ExecuteNonQuery();
                }
            }

            // Refresh or redirect to the updated page as needed
            Response.Redirect(Request.Url.ToString());
        }
        protected void EditButton_Command(object sender, CommandEventArgs e)
        {
            // Retrieve the unique identifier from the command argument
            string uniqueIdentifier = e.CommandArgument.ToString();

            // Retrieve the row data for the selected profile from the database
            string connectionString = "Server=localhost;Port=3306;Database=crud;Uid=root;Pwd=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT name, address, date_of_birth, gender, civil_status FROM profile WHERE name = @name";
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", uniqueIdentifier);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the column values from the data reader
                            string name = reader.GetString(0);
                            string addy = reader.GetString(1);
                            DateTime dateOfBirth = reader.GetDateTime(2);
                            string gender = reader.GetString(3);
                            string civilStatus = reader.GetString(4);

                            // Show the modal popup
                            openPopup();

                            hfName.Value = name; //var to store variable for identifier for sql

                            // Set the values for the modal inputs
                            fullname.Text = name;
                            address.Text = addy;
                            calendarDOB.Text = dateOfBirth.ToString("yyyy-MM-dd");
                            if (gender == "male")
                            {
                                male.Checked = true;
                            }
                            else if (gender == "female")
                            {
                                female.Checked = true;
                            }
                            civilStatusDropdown.SelectedValue = civilStatus;
                        }
                    }
                }
            }
        }


        protected void closePopup_Click(object sender, EventArgs e)
        {
            closePopup();
        }

        protected void updateData(object sender, EventArgs e)
        {
            // Retrieve the values from the modal inputs
            string name = fullname.Text;
            string addy = address.Text;
            DateTime dateOfBirth = DateTime.ParseExact(calendarDOB.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string gender = male.Checked ? "male" : "female";
            string civilStatus = civilStatusDropdown.SelectedValue;

            // Retrieve the unique identifier from the hidden field
            string uniqueIdentifier = hfName.Value;

            // Retrieve the previous profile picture location
            string previousProfilePictureLocation = string.Empty;
            string connectionString = "Server=localhost;Port=3306;Database=crud;Uid=root;Pwd=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT profile_picture_location FROM profile WHERE name = @uniqueIdentifier";
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@uniqueIdentifier", uniqueIdentifier);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            previousProfilePictureLocation = reader.GetString(0);
                        }
                    }
                }
            }

            // Update the row in the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if photoUpload has a file
                if (photoUpload.HasFile)
                {
                    // Get the file extension and create a unique filename
                    string fileExtension = Path.GetExtension(photoUpload.FileName);
                    string newFileName = Guid.NewGuid().ToString() + fileExtension;

                    // Define the file path where the photo will be saved
                    string photoFolderPath = Server.MapPath("~/Uploads/");
                    string photoFilePath = Path.Combine(photoFolderPath, newFileName);

                    // Save the file
                    photoUpload.SaveAs(photoFilePath);

                    // Delete the previous photo file if it exists
                    if (!string.IsNullOrEmpty(previousProfilePictureLocation))
                    {
                        string previousPhotoFilePath = Server.MapPath(previousProfilePictureLocation);
                        if (File.Exists(previousPhotoFilePath))
                        {
                            File.Delete(previousPhotoFilePath);
                        }
                    }

                    // Update the profile picture location in the database
                    string updateQuery = "UPDATE profile SET name = @name, address = @address, date_of_birth = @dateOfBirth, gender = @gender, civil_status = @civilStatus, profile_picture_location = @profilePictureLocation WHERE name = @uniqueIdentifier";
                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@address", addy);
                        command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                        command.Parameters.AddWithValue("@gender", gender);
                        command.Parameters.AddWithValue("@civilStatus", civilStatus);
                        command.Parameters.AddWithValue("@profilePictureLocation", "/Uploads/" + newFileName);
                        command.Parameters.AddWithValue("@uniqueIdentifier", uniqueIdentifier);
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Update the row in the database without changing the profile picture location
                    string updateQuery = "UPDATE profile SET name = @name, address = @address, date_of_birth = @dateOfBirth, gender = @gender, civil_status = @civilStatus WHERE name = @uniqueIdentifier";
                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@address", addy);
                        command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                        command.Parameters.AddWithValue("@gender", gender);
                        command.Parameters.AddWithValue("@civilStatus", civilStatus);
                        command.Parameters.AddWithValue("@uniqueIdentifier", uniqueIdentifier);
                        command.ExecuteNonQuery();
                    }
                }
            }
            closePopup();
            Response.Redirect(Request.RawUrl);
        }


            protected void closePopup()
        {
            mymodal.Style["display"] = "none";
        }

        protected void openPopup()
        {
            mymodal.Style["display"] = "block";
        }
    }
}
