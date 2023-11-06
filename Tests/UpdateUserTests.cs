using Bogus;
using GoRest_L9.Helpers;
using GoRest_L9.Models;
using Newtonsoft.Json;

namespace GoRest_L9.Tests
{
    public class UpdateUserTests
    {
        readonly HttpMethod patchMethod = HttpMethod.Patch;
        readonly HttpMethod postMethod = HttpMethod.Post;
        readonly HttpClient client = new HttpClient();
        readonly string gorestUsersUrl = "https://gorest.co.in/public/v2/users/";
        readonly string token = "Bearer 0ade2be2ee359b676825ff8d59bfaf071b5e64afc5d4ff13efecc60065ac7e10";
        readonly Faker faker = new Faker();

        [Fact]
        public async void CreateAndUpdateUser()
        {
            string newEmail = faker.Internet.ExampleEmail();
            string patchField = "{\"email\": \"" + newEmail + "\"}";

            User newUser = UserGenerator.InstantiateUser();

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, token, newUser);
            User? user = await UserGenerator.DeserializeUser(response);

            string userUri = gorestUsersUrl + user?.id;

            var responsePatch = await Requests.BodyReq(client, patchMethod, userUri, token, patchField);
            User? updatedUser = await UserGenerator.DeserializeUser(responsePatch);

            Assert.Equal(newEmail, updatedUser?.email);
        }

        [Fact]
        public async void UpdateUserWithInvalidStatus()
        {
            string patchField = "{\"status\": \"proactive\"}";

            User newUser = UserGenerator.InstantiateUser();

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, token, newUser);
            User? user = await UserGenerator.DeserializeUser(response);

            string userUri = gorestUsersUrl + user?.id;

            var responsePatch = await Requests.BodyReq(client, patchMethod, userUri, token, patchField);
            string contentPatch = await responsePatch.Content.ReadAsStringAsync();

            Assert.Contains("can't be blank", contentPatch);
        }

        [Fact]
        public async void UpdateUserWithUnavailableEmail()
        {
            string patchField = "{\"email\": \"ez.bluff@af.com\"}";

            User newUser = UserGenerator.InstantiateUser();

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, token, newUser);
            User? user = await UserGenerator.DeserializeUser(response);

            string userUri = gorestUsersUrl + user?.id;

            var responsePatch = await Requests.BodyReq(client, patchMethod, userUri, token, patchField);
            string contentPatch = await responsePatch.Content.ReadAsStringAsync();

            Assert.Contains("has already been taken", contentPatch);
        }

        [Fact]
        public async void UpdateUserWithEmptyBody()
        {
            string patchField = "";

            User newUser = UserGenerator.InstantiateUser();

            var response = await Requests.BodyReq(client, postMethod, gorestUsersUrl, token, newUser);
            User? user = await UserGenerator.DeserializeUser(response);

            string userUri = gorestUsersUrl + user?.id;

            var responsePatch = await Requests.BodyReq(client, patchMethod, userUri, token, patchField);
            User? updatedUser = await UserGenerator.DeserializeUser(responsePatch);

            Assert.Equivalent(user, updatedUser);
        }
    }
}
