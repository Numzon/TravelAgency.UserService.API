namespace TravelAgency.UserService.Application.Authentication.Models;

public record SignInResponseDto
{
    public AuthResponseDto? AuthResponse { get; set; }
    public ChallengeResponseDto? ChallengeResponse { get; set; }

    public static SignInResponseDto CreateWithAuthResponse(string accessToken, string refreshToken)
    {
        return new SignInResponseDto
        {
            AuthResponse = new AuthResponseDto(accessToken, refreshToken)
        };
    }

    public static SignInResponseDto CreateWithChallengeReponse(string challangeCode, string session)
    {
        return new SignInResponseDto
        {
            ChallengeResponse = new ChallengeResponseDto(challangeCode, session)
        };
    }
}
