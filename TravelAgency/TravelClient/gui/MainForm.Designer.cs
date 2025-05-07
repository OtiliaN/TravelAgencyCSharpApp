namespace TravelClient.gui
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listViewAllFlights = new System.Windows.Forms.ListView();
            columnHeaderDestination = new System.Windows.Forms.ColumnHeader();
            columnHeaderDeparture = new System.Windows.Forms.ColumnHeader();
            columnHeaderAirport = new System.Windows.Forms.ColumnHeader();
            columnHeaderSeats = new System.Windows.Forms.ColumnHeader();
            txtDestination = new System.Windows.Forms.TextBox();
            dtpDepartureDate = new System.Windows.Forms.DateTimePicker();
            btnSearchFlights = new System.Windows.Forms.Button();
            listViewSearchResults = new System.Windows.Forms.ListView();
            columnHeaderTime = new System.Windows.Forms.ColumnHeader();
            columnHeaderAvailableSeats = new System.Windows.Forms.ColumnHeader();
            lblSearchResults = new System.Windows.Forms.Label();
            lblAllFlights = new System.Windows.Forms.Label();
            lblDestination = new System.Windows.Forms.Label();
            lblDepartureDate = new System.Windows.Forms.Label();
            btnLogout = new System.Windows.Forms.Button();
            txtNumberOfSeats = new System.Windows.Forms.TextBox();
            lblNumberOfSeats = new System.Windows.Forms.Label();
            txtPassengers = new System.Windows.Forms.TextBox();
            lblPassengers = new System.Windows.Forms.Label();
            btnBookFlight = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // listViewAllFlights
            // 
            listViewAllFlights.BackColor = System.Drawing.Color.LightBlue;
            listViewAllFlights.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeaderDestination, columnHeaderDeparture, columnHeaderAirport, columnHeaderSeats });
            listViewAllFlights.FullRowSelect = true;
            listViewAllFlights.GridLines = true;
            listViewAllFlights.Location = new System.Drawing.Point(12, 107);
            listViewAllFlights.Name = "listViewAllFlights";
            listViewAllFlights.Size = new System.Drawing.Size(600, 400);
            listViewAllFlights.TabIndex = 0;
            listViewAllFlights.UseCompatibleStateImageBehavior = false;
            listViewAllFlights.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDestination
            // 
            columnHeaderDestination.Name = "columnHeaderDestination";
            columnHeaderDestination.Text = "Destination";
            columnHeaderDestination.Width = 150;
            // 
            // columnHeaderDeparture
            // 
            columnHeaderDeparture.Name = "columnHeaderDeparture";
            columnHeaderDeparture.Text = "Departure Date & Time";
            columnHeaderDeparture.Width = 167;
            // 
            // columnHeaderAirport
            // 
            columnHeaderAirport.Name = "columnHeaderAirport";
            columnHeaderAirport.Text = "Airport";
            columnHeaderAirport.Width = 150;
            // 
            // columnHeaderSeats
            // 
            columnHeaderSeats.Name = "columnHeaderSeats";
            columnHeaderSeats.Text = "Available Seats";
            columnHeaderSeats.Width = 130;
            // 
            // txtDestination
            // 
            txtDestination.BackColor = System.Drawing.Color.LightYellow;
            txtDestination.Location = new System.Drawing.Point(865, 20);
            txtDestination.Name = "txtDestination";
            txtDestination.Size = new System.Drawing.Size(200, 27);
            txtDestination.TabIndex = 1;
            // 
            // dtpDepartureDate
            // 
            dtpDepartureDate.CustomFormat = "yyyy-MM-dd";
            dtpDepartureDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpDepartureDate.Location = new System.Drawing.Point(865, 60);
            dtpDepartureDate.Name = "dtpDepartureDate";
            dtpDepartureDate.Size = new System.Drawing.Size(200, 27);
            dtpDepartureDate.TabIndex = 2;
            // 
            // btnSearchFlights
            // 
            btnSearchFlights.BackColor = System.Drawing.Color.LightGreen;
            btnSearchFlights.Location = new System.Drawing.Point(833, 107);
            btnSearchFlights.Name = "btnSearchFlights";
            btnSearchFlights.Size = new System.Drawing.Size(139, 36);
            btnSearchFlights.TabIndex = 3;
            btnSearchFlights.Text = "Search Flights";
            btnSearchFlights.UseVisualStyleBackColor = false;
            btnSearchFlights.Click += btnSearchFlights_Click;
            // 
            // listViewSearchResults
            // 
            listViewSearchResults.BackColor = System.Drawing.Color.LightCoral;
            listViewSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeaderTime, columnHeaderAvailableSeats });
            listViewSearchResults.FullRowSelect = true;
            listViewSearchResults.GridLines = true;
            listViewSearchResults.Location = new System.Drawing.Point(756, 181);
            listViewSearchResults.Name = "listViewSearchResults";
            listViewSearchResults.Size = new System.Drawing.Size(300, 230);
            listViewSearchResults.TabIndex = 4;
            listViewSearchResults.UseCompatibleStateImageBehavior = false;
            listViewSearchResults.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderTime
            // 
            columnHeaderTime.Name = "columnHeaderTime";
            columnHeaderTime.Text = "Departure Time";
            columnHeaderTime.Width = 150;
            // 
            // columnHeaderAvailableSeats
            // 
            columnHeaderAvailableSeats.Name = "columnHeaderAvailableSeats";
            columnHeaderAvailableSeats.Text = "Available Seats";
            columnHeaderAvailableSeats.Width = 150;
            // 
            // lblSearchResults
            // 
            lblSearchResults.AutoSize = true;
            lblSearchResults.ForeColor = System.Drawing.Color.DarkBlue;
            lblSearchResults.Location = new System.Drawing.Point(800, 150);
            lblSearchResults.Name = "lblSearchResults";
            lblSearchResults.Size = new System.Drawing.Size(0, 20);
            lblSearchResults.TabIndex = 5;
            // 
            // lblAllFlights
            // 
            lblAllFlights.AutoSize = true;
            lblAllFlights.ForeColor = System.Drawing.Color.DarkBlue;
            lblAllFlights.Location = new System.Drawing.Point(267, 60);
            lblAllFlights.Name = "lblAllFlights";
            lblAllFlights.Size = new System.Drawing.Size(77, 20);
            lblAllFlights.TabIndex = 6;
            lblAllFlights.Text = "All Flights:";
            // 
            // lblDestination
            // 
            lblDestination.AutoSize = true;
            lblDestination.ForeColor = System.Drawing.Color.DarkBlue;
            lblDestination.Location = new System.Drawing.Point(756, 27);
            lblDestination.Name = "lblDestination";
            lblDestination.Size = new System.Drawing.Size(88, 20);
            lblDestination.TabIndex = 7;
            lblDestination.Text = "Destination:";
            // 
            // lblDepartureDate
            // 
            lblDepartureDate.AutoSize = true;
            lblDepartureDate.ForeColor = System.Drawing.Color.DarkBlue;
            lblDepartureDate.Location = new System.Drawing.Point(729, 67);
            lblDepartureDate.Name = "lblDepartureDate";
            lblDepartureDate.Size = new System.Drawing.Size(115, 20);
            lblDepartureDate.TabIndex = 8;
            lblDepartureDate.Text = "Departure Date:";
            // 
            // btnLogout
            // 
            btnLogout.BackColor = System.Drawing.Color.LightCoral;
            btnLogout.Location = new System.Drawing.Point(12, 568);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new System.Drawing.Size(112, 46);
            btnLogout.TabIndex = 9;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // txtNumberOfSeats
            // 
            txtNumberOfSeats.BackColor = System.Drawing.Color.LightYellow;
            txtNumberOfSeats.Location = new System.Drawing.Point(855, 461);
            txtNumberOfSeats.Name = "txtNumberOfSeats";
            txtNumberOfSeats.Size = new System.Drawing.Size(100, 27);
            txtNumberOfSeats.TabIndex = 10;
            // 
            // lblNumberOfSeats
            // 
            lblNumberOfSeats.AutoSize = true;
            lblNumberOfSeats.ForeColor = System.Drawing.Color.DarkBlue;
            lblNumberOfSeats.Location = new System.Drawing.Point(711, 468);
            lblNumberOfSeats.Name = "lblNumberOfSeats";
            lblNumberOfSeats.Size = new System.Drawing.Size(123, 20);
            lblNumberOfSeats.TabIndex = 11;
            lblNumberOfSeats.Text = "Number of Seats:";
            // 
            // txtPassengers
            // 
            txtPassengers.BackColor = System.Drawing.Color.LightYellow;
            txtPassengers.Location = new System.Drawing.Point(855, 504);
            txtPassengers.Name = "txtPassengers";
            txtPassengers.Size = new System.Drawing.Size(190, 27);
            txtPassengers.TabIndex = 12;
            // 
            // lblPassengers
            // 
            lblPassengers.AutoSize = true;
            lblPassengers.ForeColor = System.Drawing.Color.DarkBlue;
            lblPassengers.Location = new System.Drawing.Point(751, 511);
            lblPassengers.Name = "lblPassengers";
            lblPassengers.Size = new System.Drawing.Size(83, 20);
            lblPassengers.TabIndex = 13;
            lblPassengers.Text = "Passengers:";
            // 
            // btnBookFlight
            // 
            btnBookFlight.BackColor = System.Drawing.Color.LightGreen;
            btnBookFlight.Location = new System.Drawing.Point(816, 558);
            btnBookFlight.Name = "btnBookFlight";
            btnBookFlight.Size = new System.Drawing.Size(156, 36);
            btnBookFlight.TabIndex = 14;
            btnBookFlight.Text = "Book Flight";
            btnBookFlight.UseVisualStyleBackColor = false;
            btnBookFlight.Click += btnBookFlight_Click;
            // 
            // MainForm
            // 
            ClientSize = new System.Drawing.Size(1141, 624);
            Controls.Add(btnBookFlight);
            Controls.Add(lblPassengers);
            Controls.Add(txtPassengers);
            Controls.Add(lblNumberOfSeats);
            Controls.Add(txtNumberOfSeats);
            Controls.Add(btnLogout);
            Controls.Add(lblDepartureDate);
            Controls.Add(lblDestination);
            Controls.Add(lblAllFlights);
            Controls.Add(lblSearchResults);
            Controls.Add(listViewSearchResults);
            Controls.Add(btnSearchFlights);
            Controls.Add(dtpDepartureDate);
            Controls.Add(txtDestination);
            Controls.Add(listViewAllFlights);
            Text = "Main Form";
           
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ListView listViewAllFlights;
        private System.Windows.Forms.ColumnHeader columnHeaderDestination;
        private System.Windows.Forms.ColumnHeader columnHeaderDeparture;
        private System.Windows.Forms.ColumnHeader columnHeaderAirport;
        private System.Windows.Forms.ColumnHeader columnHeaderSeats;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.DateTimePicker dtpDepartureDate;
        private System.Windows.Forms.Button btnSearchFlights;
        private System.Windows.Forms.ListView listViewSearchResults;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.ColumnHeader columnHeaderAvailableSeats;
        private System.Windows.Forms.Label lblSearchResults;
        private System.Windows.Forms.Label lblAllFlights;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Label lblDepartureDate;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.TextBox txtNumberOfSeats;
        private System.Windows.Forms.Label lblNumberOfSeats;
        private System.Windows.Forms.TextBox txtPassengers;
        private System.Windows.Forms.Label lblPassengers;
        private System.Windows.Forms.Button btnBookFlight;
    }
}