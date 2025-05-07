namespace TravelServices.service;

public class ServiceException : Exception
{
    public ServiceException() { }
    
    public ServiceException(string message) : base(message) { }
    
    public ServiceException(string message, Exception cause) : base(message, cause) { }
}