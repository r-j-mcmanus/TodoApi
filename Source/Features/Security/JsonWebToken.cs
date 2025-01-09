using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;


/*
    https://en.wikipedia.org/wiki/JSON_Web_Token

    A JWT is  a way to securly tansmit info between parties as a json object.
    It is widly used for authentication.

    Structure 
    ---------
    Made of three parts:
    - header (json)
    - Payload (json)
    - Signature (string)

    These three parts are 64byte encoded adn concatenated with a '.' between.

    bae64(header).bae64(Payload).Signature

    Header
    ------
    A json file describing the meta data about the token. Typically the type of token (JWT) and the hashing algo.
    These have standard keys.

    {
        "alg": "HS256",
        "typ": "JWT"
    }

    Payload 
    -------
    A json file containing data relevant to the token's use, such as the user name, date of creation, date of expiration, date of use

    {
        "name": "John Doe",
        "admin": false,
        "iat": 12321654654  # date in unix time
    } 

    Signature
    ---------
    Ensures the token has not been tampered by hashing the header and payload with a secret key stored on the server
    
    hash(header + '.' + payload + secret)

*/

class Header {
    public string alg = "HS256";
    public string typ = "JWT";

};

class Body {
    public string? iss;
    public string? sub;
    public string? aud;
    public long?  exp;
    public long? nbf;
    public long? iat;
    public string? jti;

};


//[assembly: InternalsVisibleTo("TodoApi.Test")]
public static class JsonWebToken {

    static string makeToken()
    {
        
        string? salt = Environment.GetEnvironmentVariable("JWT_SALT");

        if (salt == null) {
            throw new Exception("Cannot load environment variables `JWT_SALT`");
        }

        var header = new Header();
        var body = new Body();

        string headerString = JsonSerializer.Serialize(header);
        string header64 = Base64UrlEncode(headerString);

        string bodyString = JsonSerializer.Serialize(body);
        string body64 = Base64UrlEncode(bodyString);

        string unsignedToken  = $"{header64}.{body64}";

        byte[] signature  = Hasher.SaltAndHash(unsignedToken, salt);
        string signature64 = Base64UrlEncode(signature);

        return $"{unsignedToken}.{signature64}";
    } 

    static bool validateToken(string input)
    {
        string? salt = Environment.GetEnvironmentVariable("JWT_SALT");

        if (salt == null) {
            throw new Exception("Cannot load environment variables `JWT_SALT`");
        }

        int lastDotIndex = input.LastIndexOf('.');

        if (lastDotIndex == -1 || lastDotIndex == 0 || lastDotIndex == input.Length) {
            return false;
        }

        string inputSignature = input.Substring(lastDotIndex + 1);
        
        string unsignedToken = input.Substring(0, lastDotIndex);
        byte[] tokenSignature  = Hasher.SaltAndHash(unsignedToken, salt);
        string tokenSignature64 = Base64UrlEncode(tokenSignature);

        // Use constant-time comparison for security
        // think mastermind game
        return ConstantTimeEquals(inputSignature, tokenSignature64);
    } 

    private static string Base64UrlEncode(string input)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                      .Replace("+", "-")
                      .Replace("/", "_")
                      .TrimEnd('=');
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
                      .Replace("+", "-")
                      .Replace("/", "_")
                      .TrimEnd('=');
    }

    private static bool ConstantTimeEquals(string input, string toMatch)
    {
        /*
        Ensure that the time taken to check if the input and toMatch are equal is independent of the 
        length of input or the overlap between correct characters in input and toMatch
        */
        int maxLength = Math.Max(input.Length, toMatch.Length);
        input = input.PadRight(maxLength, '\0');
        toMatch = toMatch.PadRight(maxLength, '\0');

        bool result = true;
        for (int i =0; i < input.Length; i++) {
            result &= input[i] == toMatch[i];
        }

        return result;
    }
}