
namespace Store_CS_WebAPI_Client.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string OrderedItem { get; set; }
        public int OrderedQuantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalBill { get; set; }
        public string OrderedDate { get; set; }
    }
}
