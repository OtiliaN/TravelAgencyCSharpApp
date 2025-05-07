namespace TravelNetworking.networking.jsonprotocol;

// Tipurile de răspunsuri care pot fi trimise de la server către client
public enum ResponseType
{
    OK,
    Error,
    Login,
    Logout,
    GetAllFlights,
    GetFlightsByDestinationAndDate,
    BuyTickets,
    ModifiedFlight
}
   