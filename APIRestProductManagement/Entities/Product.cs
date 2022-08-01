namespace APIRestProductManagement.Entities
{
    public class Product
    {
        /*
         * •	Código de producto (secuencial y no nulo)
            •	Descripción del producto (no nulo)
            •	Estado del producto (activo o inactivo)
            •	Fecha de fabricación
            •	Fecha de validez
            •	Código de proveedor
            •	Descripción del proveedor
            •	Telefono del proveedor

         */

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
