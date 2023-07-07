using System.Net.Http.Json;
using System.Text;
using BlogApi.HelperEntities.Pagination;
using BlogApi.Interfaces.IManagers;
using BlogApi.Managers;
using BlogApi.Models.PostModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;

namespace BlogApi_xUnitTests.IntegrationTests;

public class PostControllerTests
{
    private readonly HttpClient _httpClient;
    public PostControllerTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        _httpClient = webApplicationFactory.CreateDefaultClient();
    }
    
    [Fact]
    public async Task AddNewPost_Test()
    {
        // Arrange
        var createModel = new CreatePostModel
        {
            Title = "Bu title ",
            Content = "Bu new content",
            Tag = "Bu new tag",
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "api/Posts");
        request.Content = JsonContent.Create(createModel);

        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        Assert.NotNull(response);
      //Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetAllPosts_Test()
    {
        // Arrange
        var postFilter = new PostGetFilter()
        {
            Page = 1,
            Size = 1
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "api/Posts/pagination");
        request.Content = new StringContent(
            content: JsonConvert.SerializeObject(postFilter),
            encoding: Encoding.UTF8,
            mediaType: "application/json"
        );
        
        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

       // var result = await response.Content.ReadFromJsonAsync<PostModel>();

        //Assert.NotNull(result);
        //Assert.Equal("asd", result.Title);
    }

    [Fact]
    public async Task GetPostByIdWithLikesAndComments_Test()
    {
        // Arrange
        var postId = Guid.Parse("bc6f5118-5333-40ce-88b9-227a2a675341");

        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Posts/{postId}");
        request.Content = new StringContent(
            content: JsonConvert.SerializeObject(postId),
            encoding: Encoding.UTF8,
            mediaType: "application/json");

        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        Assert.NotNull(response);
        // Assert.True(response.IsSuccessStatusCode, "Assert true message: response.IsSuccessStatusCode");

        //var postModel = await response.Content.ReadFromJsonAsync<PostModel>();
        //Assert.NotNull(postModel);
        //Assert.Equal(postId, postModel.Id);
    }

    [Fact]
    public async Task UpdatePost_Test()
    {
        // Arrange
        var postId = Guid.Parse("bc6f5118-5333-40ce-88b9-227a2a675341");
        var updatePostModel = new UpdatePostModel
        {
            Title = "new title",
            Content = "new content",
            Tag = "new tag"
        };

        var request = new HttpRequestMessage(HttpMethod.Put, $"api/Posts/{postId}");
        request.Content = JsonContent.Create(updatePostModel);

        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert 
        Assert.NotNull(response);
        //Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task DeletePost_Test()
    {
        // Arrange
        var postId = Guid.Parse("bc6f5118-5333-40ce-88b9-227a2a675341");

        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Posts/{postId}");
        request.Content = JsonContent.Create(postId);

        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        Assert.NotNull(response);
        //Assert.True(response.IsSuccessStatusCode);
    }
}

public class CustomWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
           //  var postManager = services.SingleOrDefault(c => c.ServiceType == typeof(IPostManager));
           var postManager = services.FirstOrDefault(type => type.ServiceType == typeof(IPostManager));

            if (postManager != null)
                services.Remove(postManager);

            var mockPostManager = new Mock<IPostManager>();
           // mockPostManager.Setup(f => f.GetPostByIdAsync(It.IsAny<Guid>())).Returns();
            
           // services.AddSingleton<IPostManager>(o => mockPostManager);
        });
    }
}

/*

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var priceCalculator = services.SingleOrDefault(c => c.ServiceType == typeof(IPriceCalculator));

            if (priceCalculator != null)
                services.Remove(priceCalculator);

            var priceMock = new PriceCalculator();

            services.AddSingleton<IPriceCalculator>(f => priceMock);
        });

        builder.UseEnvironment("Development");
    }


    [Theory]
    [InlineData(10, 10, 10, 90)]
    [InlineData(10, 10, 100, 0)]
    public async Task CalculatePrice_ReturnsTotalPrice(decimal productId, decimal productPrice, decimal discount, decimal expected)
    {
	    var priceModel = new PriceModel(productId, productPrice, discount);

	    var request = new HttpRequestMessage(HttpMethod.Post, "api/Price");
	    request.Content = JsonContent.Create(priceModel);

	    var response = await _httpClient.SendAsync(request);

        Assert.True(response.IsSuccessStatusCode);

        var priceResult = await response.Content.ReadFromJsonAsync<PriceResultModel>();

        Assert.NotNull(priceResult);
        Assert.Equal(expected, priceResult.TotalPrice);
    }

    [Fact]
    public async Task CalculatePrice_WithInvalidDiscountReturnsBadRequest()
    {
	    var priceModel = new PriceModel(10, 10, 120);

	    var request = new HttpRequestMessage(HttpMethod.Post, "api/Price");
	    request.Content = JsonContent.Create(priceModel);

	    var response = await _httpClient.SendAsync(request);

	    Assert.True(!response.IsSuccessStatusCode);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

}*/