using eCommerce.ProductsService.API.Middlewares;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace eCommerce.ProductsService.Tests.API.Middlewares;

public class ExceptionHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
    private const string ExceptionMessage = "Test exception";
    private const string Path = "/test-path";
    private const string SuccessMessage = "Executed successfully";

    public ExceptionHandlingMiddlewareTests()
    {
        _loggerMock = new();
    }

    [Fact(DisplayName = "Invoke should set StatusCode to 500, request path and exception message as instance and detail of ProblemDetails when exception is raised")]
    public async Task Invoke_ShouldSetResponse_WhenExceptionRaised()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        httpContext.Request.Path = Path;

        RequestDelegate next = async (context) =>
        {
            await Task.Delay(100);
            throw new NotImplementedException(ExceptionMessage);
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);


        // Act
        await middleware.Invoke(httpContext);


        // Assert
        httpContext.Response.Body.Position = 0;
        var jsonDoc = await JsonDocument.ParseAsync(httpContext.Response.Body);
        var message = jsonDoc.RootElement.GetProperty("detail").GetString();
        var instance = jsonDoc.RootElement.GetProperty("instance").GetString();

        httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        instance.Should().Be(Path);
        message.Should().Be(ExceptionMessage);

        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact(DisplayName = "Invoke should not modify response when no exception is thrown")]
    public async Task Invoke_ShouldNotModifyResponse_WhenNoException()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        RequestDelegate next = async (context) =>
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync(SuccessMessage);
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);


        // Act
        await middleware.Invoke(httpContext);


        // Assert
        httpContext.Response.Body.Position = 0;
        var streamReader = new StreamReader(httpContext.Response.Body);
        var body = await streamReader.ReadToEndAsync();

        httpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        body.Should().Be(SuccessMessage);
    }
}
