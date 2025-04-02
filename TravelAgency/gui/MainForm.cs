using AgentieTurismCSharp.domain;
using TravelAgency.service;

namespace TravelAgency;
public partial class MainForm : Form
{
    private readonly Service _service;
    private const string DateFormat = "yyyy-MM-dd";
    private const string TimeFormat = "HH:mm";

    public MainForm(Service service)
    {
        InitializeComponent();
        _service = service;
        LoadFlights();
    }

    private void LoadFlights()
    {
        IEnumerable<Flight> flights = _service.GetAllFlights();
        listViewAllFlights.Items.Clear();
        foreach (var flight in flights)
        {
            string[] row = { flight.Destination, flight.DepartureDateTime.ToString($"{DateFormat} {TimeFormat}"), flight.Airport, flight.AvailableSeats.ToString() };
            var listViewItem = new ListViewItem(row) { Tag = flight };
            listViewAllFlights.Items.Add(listViewItem);
        }
    }

    private void btnSearchFlights_Click(object sender, EventArgs e)
    {
        string destination = txtDestination.Text;
        DateTime departureDate = dtpDepartureDate.Value;

        IEnumerable<Flight> flights = _service.SearchFlights(destination, departureDate);
        listViewSearchResults.Items.Clear();
        foreach (var flight in flights)
        {
            string[] row = { flight.DepartureDateTime.ToString(TimeFormat), flight.AvailableSeats.ToString() };
            var listViewItem = new ListViewItem(row) { Tag = flight };
            
            listViewSearchResults.Items.Add(listViewItem);
        }

        lblSearchResults.Text = $"Search results for: {destination} on {departureDate.ToString(DateFormat)}";
    }

    private void btnLogout_Click(object sender, EventArgs e)
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
    
    private void btnBookFlight_Click(object sender, EventArgs e)
    {
        if (listViewSearchResults.SelectedItems.Count == 0)
        {
            MessageBox.Show("Please select a flight to book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Flight flight = listViewSearchResults.SelectedItems[0].Tag as Flight;
        if (flight == null)
        {
            MessageBox.Show("Selected flight is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        } int numberOfSeats;
        if (!int.TryParse(txtNumberOfSeats.Text, out numberOfSeats))
        {
            MessageBox.Show("Invalid number of seats.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        List<string> passengers = txtPassengers.Text.Split(',').Select(p => p.Trim()).ToList();

        if (passengers.Count != numberOfSeats)
        {
            MessageBox.Show("Number of passengers must match the number of seats.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

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
}