using Microsoft.AspNetCore.Mvc;
using Hw_11_30_SearchShop.Data;
using Hw_11_30_SearchShop.Models;

namespace Hw_11_30_SearchShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly SqlContext sqlContext;

        public ProductController(SqlContext context)
        {
            sqlContext = context;
        }
        private Products GetProduct(Guid id)
        {
            Products products = null;
            string query = $"select top 1 [Id], [Name], [Description], [Price] From [Products] where [Id]= '{id}'";
            sqlContext.ExecuteQeury(query, reader =>
            {
                if(reader.Read())
                {
                    products = new Products()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                    };
                }

            });
            return products;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Products> product = new List<Products>();
            string query = "Select [Id],[Name],[Description],[Price] From [Products]";
            sqlContext.ExecuteQeury(query, reader =>
            {
                while (reader.Read())
                {
                    product.Add(new Products
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                    });

                }
            });
            return Ok(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            HttpContext.Response.ContentType = "text/html";
            return Content("""
                <form method="post" action="../Product/Create">
                <div class="form-group">
                    <label for="Name" class="form-check-label">Name:</label>
                    <input type="text" name="Name" class="from-control" required>
                </div>
                <div class="form-group">
                    <label for="Description" class="form-check-label">Description:</label>
                    <input type="text" name="Description" class="from-control" required>
                </div>
                <div class="form-group">
                    <label for="Price" class="form-check-label">Price:</label>
                    <input type="number" name="Price" class="from-control" required>
                </div>
                <div class="form-group mt-3">
                    <button type="submit" class="btn btn-primary">Submit</button>
                </div>
                </form>
                """);
        }
        [HttpPost]
        public IActionResult Create(Products product)
        {
            string query = $"insert into [Products],([Id],[Name],[Description],[Price]) values" +
                $"('{product.Id},{product.Name},{product.Description},{product.Price}')";
            sqlContext.ExecuteQeury(query, reader => { });
            return Ok(product);
        }

        [HttpGet]
        public IActionResult Search(string keyword)
        {
            List<Products> products = new List<Products>();
            string query = $"select top 1 [Id], [Name], [Description], [Price] From [Products] where [Name] Like '%{keyword}%'";
            sqlContext.ExecuteQeury(query, reader =>
            {
                while (reader.Read())
                {
                    products.Add(new Products
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                    });
                }

            });
            return Ok(products);
        }
        [HttpGet]
        public IActionResult Details(Guid guid)
        {
            Products? products = GetProduct(guid);
            if (products != null)
            {
                return Ok(products);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult Delete(Guid id)
        {
            Products products  = GetProduct(id);
            if(products != null)
            {
                string query = $"Delete from [Products] where [Id] = '{id}'";
                sqlContext.ExecuteQeury(query, reader => { });
                return Ok(products);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
