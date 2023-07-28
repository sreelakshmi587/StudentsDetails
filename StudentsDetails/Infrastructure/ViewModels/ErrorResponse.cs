using System.Text.Json;

namespace StudentsDetails.Infrastructure.ViewModels
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public string? Messsage { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
