using Xunit;
using Microsoft.AspNetCore.Http;
using TravelAgency.UserService.Application.Common.Result;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TravelAgency.UserService.Application.UnitTests.Common.Result;
public sealed class CustomResultTests
{
    private readonly Fixture _fixture; 

    public CustomResultTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void CustomResult_IResultAsParameter_ValueNotEmpryAndSucceessAsTrue()
    {
        //Arrange
        var randomValue = _fixture.Create<string>();
        var result = Results.Ok(randomValue);

        //Act
        var customResult = new CustomResult(result);

        //Assess
        customResult.Error.Should().BeNull();
        customResult.Value.Should().NotBeNull();
        customResult.IsSuccess.Should().BeTrue();

        var retrivedResult = customResult.Value as Ok<string>;

        retrivedResult.Should().NotBeNull();
        retrivedResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var retrivedValue = retrivedResult.Value as string;

        retrivedValue.Should().NotBeNull();
        retrivedValue.Should().Be(randomValue);
    }
}
