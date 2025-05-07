using TravelModel.domain;

namespace TravelServices.service;

public interface IObserver
{
    void flightModified(Flight flight);
}