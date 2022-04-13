namespace Broker_Shuranskiy.Models
{
    public class Stocks
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Average_5yers_div_yeld { get; set; }
        public float Min_Lot { get; set; }
    }
}
