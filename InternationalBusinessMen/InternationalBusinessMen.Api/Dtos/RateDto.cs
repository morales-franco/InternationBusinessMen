namespace InternationalBusinessMen.Api.Dtos
{
    public class RateDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }

        public RateDto()
        {

        }

        public RateDto(string from, string to, decimal rate)
        {
            From = from;
            To = to;
            Rate = rate;
        }
    }
}
