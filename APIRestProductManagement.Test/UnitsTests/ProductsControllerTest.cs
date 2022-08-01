using APIRestProductManagement.Controllers;
using APIRestProductManagement.Entities;
using AutoMapper;

namespace APIRestProductManagement.Test.UnitsTests
{
    [TestClass]
    public class ProductsControllerTest : BaseTests
    {       

        [TestMethod]
        public void GetTest()
        {
            // Preparacion
            var bdName = Guid.NewGuid().ToString();
            var context = BuildDbContext(bdName);
            var mapper = ConfigureAutoMapper();

            context.Products.Add(new Product()
            {
                Id = new Guid(),
                Description = "PRODUCTO 5",
                Status = 1,
                ManufacturingDate = Convert.ToDateTime("2022/01/01"),
                ValidityDate = Convert.ToDateTime("2022/12/31"),
                SupplierCode = 1,
                SupplierDescription = "PROVEEDOR ABC",
                SupplierPhone = "12345"

            });

            ProductsController productsController = new ProductsController(context, mapper);

            // Ejecucion

            //Verificacion
        }
    }
}