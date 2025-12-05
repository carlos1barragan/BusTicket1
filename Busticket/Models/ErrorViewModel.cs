namespace Busticket.Models
{
    public class AgregarCarritoDto
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
