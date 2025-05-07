namespace TravelNetworking.networking.jsonprotocol;

// Tipurile de cereri care pot fi trimise Intre client Si server
public enum RequestType
{
    Login,
    Logout,
    GetAllFlights,
    GetFlightsByDestinationAndDate,
    BuyTickets
}