using MediatR;

namespace Core.Message;

public abstract class Event : Message, INotification
{
    
}