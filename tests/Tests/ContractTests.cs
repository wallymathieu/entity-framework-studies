using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace SomeBasicEFApp.Tests
{
    public class ContractTests:IClassFixture<ApiFixture>
    {
        private readonly ApiFixture fixture;
        public ContractTests(ApiFixture fixture) => this.fixture = fixture;

        [Fact]
        public async Task Can_save_and_get_customer()
        {
            using (var client = fixture.Server.CreateClient())
            {
                var createdCustomer = await client.PostAsync("/api/v1/customers",
                    new StringContent(@"{
                        ""firstname"": ""Test"",
                        ""lastname"": ""TRest""
                    }", Encoding.UTF8, "application/json"));
                createdCustomer.EnsureSuccessStatusCode();
                var obj = JObject.Parse(await createdCustomer.Content.ReadAsStringAsync());
                var id = obj["id"].Value<string>();
                Assert.NotNull(id);
                var customerResponse = await client.GetAsync("/api/v1/customers/"+id);
                customerResponse.EnsureSuccessStatusCode();
                var customerId = JObject.Parse(await customerResponse.Content.ReadAsStringAsync())["id"].Value<string>();
            }
        }
        [Fact]
        public async Task Can_save_and_get_product()
        {
            using (var client = fixture.Server.CreateClient())
            {
                var createdCustomer = await client.PostAsync("/api/v1/products",
                    new StringContent(@"{
                        ""name"": ""Test"",
                        ""cost"": 10
                    }", Encoding.UTF8, "application/json"));
                createdCustomer.EnsureSuccessStatusCode();
                var obj = JObject.Parse(await createdCustomer.Content.ReadAsStringAsync());
                var id = obj["id"].Value<string>();
                Assert.NotNull(id);
                var customerResponse = await client.GetAsync("/api/v1/products/"+id);
                customerResponse.EnsureSuccessStatusCode();
                var customerId = JObject.Parse(await customerResponse.Content.ReadAsStringAsync())["id"].Value<string>();
            }
        }
    }

}
