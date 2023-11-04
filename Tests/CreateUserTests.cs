using GoRest_L9.Helpers;
using GoRest_L9.Models;
using Newtonsoft.Json;

namespace GoRest_L9.Tests
{
    public class CreateUserTests
    {
        readonly HttpMethod postMethod = HttpMethod.Post;
        readonly HttpClient client = new HttpClient();
        readonly string gorestUsersUrl = "https://gorest.co.in/public/v2/users/";
        readonly string token = "Bearer 0ade2be2ee359b676825ff8d59bfaf071b5e64afc5d4ff13efecc60065ac7e10";

        [Fact]
        public async void CreateAndAssertUser()
        {
            HttpMethod getMethod = HttpMethod.Get;
            HttpMethod postMethod = HttpMethod.Post;
            User newUser = UserGenerator.InstantiateUser();

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, token, newUser);
            string content = await response.Content.ReadAsStringAsync();
            User? user = JsonConvert.DeserializeObject<User>(content);

            string getUri = gorestUsersUrl + user?.id;

            var responseGet = await Requests.NoBodyReq(client, getMethod, getUri, token);
            string contentGet = await responseGet.Content.ReadAsStringAsync();
            User? createdUser = JsonConvert.DeserializeObject<User>(contentGet);

            Assert.Multiple(
                () => Assert.Equal(newUser.name, createdUser?.name),
                () => Assert.Equal(newUser.email, createdUser?.email)
                );
        }

        [Fact]
        public async void CreateUserWithInvalidToken()
        {
            string invalidToken = "Bearer invalidToken";
            User newUser = UserGenerator.InstantiateUser();

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, invalidToken, newUser);
            string content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Invalid token", content);
        }

        [Fact]
        public async void CreateUserWithoutAuth()
        {
            User newUser = UserGenerator.InstantiateUser();
            string jsonUser = JsonConvert.SerializeObject(newUser);

            var message = new HttpRequestMessage(HttpMethod.Post, gorestUsersUrl);
            message.Content = new StringContent(jsonUser);

            HttpResponseMessage response = await client.SendAsync(message);
            string content = await response.Content.ReadAsStringAsync();
            var code = response.StatusCode.ToString();

            Assert.Contains("Authentication failed", content);
            Assert.Equal("Unauthorized", code);
        }

        [Fact]
        public async void CreateUserWithIncompleteBody()
        {
            User incompleteUser = new User(null, "Boro", "wwa@ema.il", null, "active");

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, token, incompleteUser);
            string content = await response.Content.ReadAsStringAsync();
            var code = response.StatusCode.ToString();

            Assert.Contains("can't be blank, can be male of female", content);
            Assert.Equal("UnprocessableEntity", code);
        }

        [Fact]
        public async void CreateUserWithUnavailableEmail()
        {
            User invalidUser = new User(null, "Boro", "ez.bluff@af.com", "male", "active");

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, token, invalidUser);
            string content = await response.Content.ReadAsStringAsync();

            Assert.Contains("has already been taken", content);
        }
    }
}
