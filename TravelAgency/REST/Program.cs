using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace REST
{
    class MainClass
    {
        static HttpClient client = new HttpClient(new LoggingHandler(new HttpClientHandler()));

        private static string URL_BASE = "http://localhost:8080/org/flights";

        public static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri(URL_BASE);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // GET ALL
            Console.WriteLine("Get all flights");
            var flights = await GetFlightsAsync("flights");
            flights.ForEach(Console.WriteLine);

            // GET BY ID
            string flightId = flights[0].Id.ToString();
            Console.WriteLine("Get flight with id: " + flightId);
            Flight flight = await GetFlightAsync("flights/" + flightId);
            Console.WriteLine("Flight found: " + flight);

            // CREATE
            Flight flightToSave = new Flight
            {
                Destination = "Iasi",
                Airport = "Traian Vuia",
                DepartureDateTime = DateTime.UtcNow.AddDays(10),
                AvailableSeats = 300
            };
            Console.WriteLine("Create flight: " + flightToSave);
            Flight createdFlight = await CreateFlightAsync("", flightToSave);
            Console.WriteLine("Flight created: " + createdFlight);

            // UPDATE
            if (createdFlight != null)
            {
                Flight flightToUpdate = new Flight
                {
                    Id = createdFlight.Id,
                    Destination = "Los Angeles",
                    Airport = "LAX",
                    DepartureDateTime = createdFlight.DepartureDateTime.AddDays(1),
                    AvailableSeats = 99
                };
                Console.WriteLine("Update flight: " + flightToUpdate);
                Flight updatedFlight = await UpdateFlightAsync("flights/" + createdFlight.Id, flightToUpdate);
                Console.WriteLine("Flight updated: " + updatedFlight);

                // DELETE
                Console.WriteLine("Delete flight with id: " + createdFlight.Id);
                Flight deletedFlight = await DeleteFlightAsync("flights/" + createdFlight.Id);
                Console.WriteLine("Flight deleted: " + deletedFlight);
            }
        }

        static async Task<Flight> GetFlightAsync(string path)
        {
            Flight flight = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                flight = JsonConvert.DeserializeObject<Flight>(jsonString);
            }
            return flight;
        }

        static async Task<List<Flight>> GetFlightsAsync(string path)
        {
            List<Flight> flights = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                flights = JsonConvert.DeserializeObject<List<Flight>>(jsonString);
            }
            return flights;
        }

        static async Task<Flight> CreateFlightAsync(string path, Flight flight)
        {
            Flight createdFlight = null;
            HttpResponseMessage response = await client.PostAsJsonAsync(path, flight);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                createdFlight = JsonConvert.DeserializeObject<Flight>(jsonResponse);
            }
            return createdFlight;
        }

        static async Task<Flight> UpdateFlightAsync(string path, Flight flight)
        {
            Flight updatedFlight = null;
            HttpResponseMessage response = await client.PutAsJsonAsync(path, flight);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                updatedFlight = JsonConvert.DeserializeObject<Flight>(jsonResponse);
            }
            return updatedFlight;
        }

        static async Task<Flight> DeleteFlightAsync(string path)
        {
            Flight deletedFlight = null;
            HttpResponseMessage response = await client.DeleteAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                deletedFlight = JsonConvert.DeserializeObject<Flight>(jsonResponse);
            }
            return deletedFlight;
        }

        public class Flight
        {
            [JsonProperty("id")]
            public long? Id { get; set; }

            [JsonProperty("destination")]
            public string Destination { get; set; }

            [JsonProperty("airport")]
            public string Airport { get; set; }

            [JsonProperty("departureDateTime")]
            public DateTime DepartureDateTime { get; set; }

            [JsonProperty("availableSeats")]
            public int AvailableSeats { get; set; }

            public override string ToString()
            {
                return $"Id: {Id}, Destination: {Destination}, Airport: {Airport}, Departure: {DepartureDateTime}, AvailableSeats: {AvailableSeats}";
            }
        }

        public class LoggingHandler : DelegatingHandler
        {
            public LoggingHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                Console.WriteLine("Request:");
                Console.WriteLine(request.ToString());
                if (request.Content != null)
                {
                    Console.WriteLine(await request.Content.ReadAsStringAsync());
                }
                Console.WriteLine();

                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
                Console.WriteLine("Response:");
                Console.WriteLine(response.ToString());
                if (response.Content != null)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                Console.WriteLine();
                return response;
            }
        }
    }
}