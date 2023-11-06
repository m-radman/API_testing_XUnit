using GoRest_L9.Helpers;
using GoRest_L9.Models;
using Newtonsoft.Json;

namespace GoRest_L9.Tests
{
    public class DeleteUserTests
    {
        readonly HttpMethod postMethod = HttpMethod.Post;
        readonly HttpMethod deleteMethod = HttpMethod.Delete;
        readonly HttpClient client = new HttpClient();
        readonly string gorestUsersUrl = "https://gorest.co.in/public/v2/users/";
        readonly string token = "Bearer 0ade2be2ee359b676825ff8d59bfaf071b5e64afc5d4ff13efecc60065ac7e10";

        [Fact]
        public async void DeleteCreatedUser()
        {
            HttpMethod getMethod = HttpMethod.Get;
            User newUser = UserGenerator.InstantiateUser();

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, token, newUser);
            User? user = await UserGenerator.DeserializeUser(response);

            string userUri = gorestUsersUrl + user?.id;

            await Requests.NoBodyReq(client, deleteMethod, userUri, token);

            var responseGet = await Requests.NoBodyReq(client, getMethod, userUri, token);
            string contentGet = await responseGet.Content.ReadAsStringAsync();

            Assert.Contains("not found", contentGet);
        }

        [Fact]
        public async void DeleteUserWithoutAuth()
        {
            User newUser = UserGenerator.InstantiateUser();
            string invalidToken = "Bearer invalidToken";

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, token, newUser);
            User? user = await UserGenerator.DeserializeUser(response);

            string userUri = gorestUsersUrl + user?.id;

            var responeDel = await Requests.NoBodyReq(client, deleteMethod, userUri, invalidToken);
            var contentDel = await responeDel.Content.ReadAsStringAsync();

            Assert.Contains("Invalid token", contentDel);
        }
    }
}
