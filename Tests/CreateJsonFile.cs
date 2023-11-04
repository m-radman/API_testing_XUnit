using GoRest_L9.Models;
using Newtonsoft.Json;

namespace GoRest_L9.Tests
{
    public class CreateJsonFile
    {
        [Fact]
        public async void TestData()
        {
            var currentDir = System.IO.Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "JSON_File" 
                + Path.DirectorySeparatorChar + "Credentials.json";

            var json = new JsonFile();
            json.username = "username";
            json.password = "password";

            await File.WriteAllTextAsync(currentDir, JsonConvert.SerializeObject(json));

            var readJson = await File.ReadAllTextAsync(currentDir);
            var newJson = JsonConvert.DeserializeObject<JsonFile>(readJson);

            Assert.Multiple(
                () => Assert.Equal(json.username, newJson.username),
                () => Assert.Equal(json.password, newJson.password)
                );
        }
    }
}
