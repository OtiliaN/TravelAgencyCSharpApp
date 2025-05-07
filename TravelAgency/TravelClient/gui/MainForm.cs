using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TravelModel.domain;
using TravelServices.service;

namespace TravelClient.gui
{
    public partial class MainForm : Form, IObserver
    {
        private readonly IService _service;
        private const string DateFormat = "yyyy-MM-dd";
        private const string TimeFormat = "HH:mm";
        private Agent loggedAgent;


        public MainForm(IService service)
        {
            _service = service;
             InitializeComponent();
        }
        
        public void SetLoggedAgent(Agent agent)
        {
            this.loggedAgent = agent;
            LoadFlights();
        }

        private void LoadFlights()
        {
            try
            {
                IEnumerable<Flight> flights = _service.GetAllFlights();
                listViewAllFlights.Items.Clear();
                foreach (var flight in flights)
                {
                    string[] row =
                    {
                        flight.Destination,
                        flight.DepartureDateTime.ToString($"{DateFormat} {TimeFormat}"),
                        flight.Airport,
                        flight.AvailableSeats.ToString()
                    };
                    var listViewItem = new ListViewItem(row) { Tag = flight };
                    listViewAllFlights.Items.Add(listViewItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load flights: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearchFlights_Click(object sender, EventArgs e)
        {
            string destination = txtDestination.Text.Trim();
            DateTime departureDate = dtpDepartureDate.Value.Date;

            if (string.IsNullOrEmpty(destination))
            {
                MessageBox.Show("Please enter a destination.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                IEnumerable<Flight> flights = _service.GetFlightsByDestinationAndDate(destination, departureDate);
                listViewSearchResults.Items.Clear();
                foreach (var flight in flights)
                {
                    string[] row =
                    {
                        flight.DepartureDateTime.ToString(TimeFormat),
                        flight.AvailableSeats.ToString()
                    };
                    var listViewItem = new ListViewItem(row) { Tag = flight };

                    listViewSearchResults.Items.Add(listViewItem);
                }

                lblSearchResults.Text = $"Search results for: {destination} on {departureDate.ToString(DateFormat)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to search flights: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                LoginForm loginForm = new LoginForm(_service);
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    this.Show();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBookFlight_Click(object sender, EventArgs e)
        {
            if (listViewSearchResults.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a flight to book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Flight flight = listViewSearchResults.SelectedItems[0].Tag as Flight;
            if (flight == null)
            {
                MessageBox.Show("Selected flight is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtNumberOfSeats.Text.Trim(), out int numberOfSeats) || numberOfSeats <= 0)
            {
                MessageBox.Show("Please enter a valid number of seats.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> passengers = txtPassengers.Text.Split(',')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList();

            if (passengers.Count != numberOfSeats)
            {
                MessageBox.Show("Number of passengers must match the number of seats.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool success = _service.BuyTickets(flight, passengers, numberOfSeats);

                if (success)
                {
                    MessageBox.Show("Booking successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFlights(); // Refresh the flights list
                }
                else
                {
                    MessageBox.Show("Not enough available seats or booking failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to book flight: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Implementation of IObserver
        public void flightModified(Flight flight)
        {
            // Update the specific flight in the UI
            Invoke((MethodInvoker)delegate
            {
                foreach (ListViewItem item in listViewAllFlights.Items)
                {
                    var currentFlight = item.Tag as Flight;
                    if (currentFlight != null && currentFlight.Id == flight.Id)
                    {
                        currentFlight.AvailableSeats = flight.AvailableSeats;
                        item.SubItems[3].Text = flight.AvailableSeats.ToString();
                        break;
                    }
                }
            });
        }
    }
}