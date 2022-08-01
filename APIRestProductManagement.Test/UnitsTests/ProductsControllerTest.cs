using APIRestProductManagement.Controllers;
using APIRestProductManagement.Entities;
using AutoMapper;

namespace APIRestProductManagement.Test.UnitsTests
{
    [TestClass]
    public class ProductsControllerTest : BaseTests
    {       

        [TestMethod]
        public async Task GetTest()
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

            context.Products.Add(new Product()
            {
                Id = new Guid(),
                Description = "PRODUCTO 6",
                Status = 1,
                ManufacturingDate = Convert.ToDateTime("2022/01/01"),
                ValidityDate = Convert.ToDateTime("2022/12/31"),
                SupplierCode = 1,
                SupplierDescription = "PROVEEDOR ABC",
                SupplierPhone = "12345"

            });

            await context.SaveChangesAsync();

            var conext2= BuildDbContext(bdName);

            // Ejecucion
            var controller = new ProductsController(context, mapper);

            var answer = await controller.Get();

            //Verificacion
            var productCount = answer.Value;
            Assert.AreEqual(2, productCount.Count);
        }
    }
}