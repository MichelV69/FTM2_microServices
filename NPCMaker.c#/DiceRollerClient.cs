using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DiceRollerClient
{

    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Name: {product.Name}\tPrice: " +
                $"{product.Price}\tCategory: {product.Category}");
        }


        static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Product>();
            }
            return product;
        }

static async Task RunAsync(string whatToDo)
        {
            // Update port # in the following line.
            var port = switch (whatToDo.ToLower)
            {
              "rolldice":
                9021;
              "scantext":
                9022;
              default:
                9020;
            }


            client.BaseAddress = new Uri($"http://localhost:{port}/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
              // do the things via switch here
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

    }

}
