namespace Mango.Services.EmailApi.Models
{
    public class EmailLogger
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Message { get; set; } = default!;
        public DateTime? EmailSent { get; set; }
    }
}
