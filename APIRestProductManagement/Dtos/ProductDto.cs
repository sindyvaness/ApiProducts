namespace APIRestProductManagement.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public short Status { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ValidityDate { get; set; }
        public int SupplierCode { get; set; }
        public string SupplierDescription { get; set; }
        public string SupplierPhone { get; set; }
    }
}
