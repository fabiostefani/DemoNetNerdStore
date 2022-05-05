using FluentValidation.Results;

namespace Core.Message.Integration;

public class ResponseMessage : Message
{
    public ValidationResult ValidationResult { get; set; }
    
    public ResponseMessage(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }
}