using System.Security.Cryptography;
using System.Text;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TodoApi.Test")]
public static class Hasher
{

    public static byte[] SaltAndHash(string message, byte[] salt)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(message);
        byte[] saltAndTextBytes = new byte[salt.Length + textBytes.Length];


        Buffer.BlockCopy(
            salt, // what we are copying
            0, // which index we are copying it from
            saltAndTextBytes, // what we copy it into
            0, // which index in saltAndTextBytes we want to copy into
            salt.Length // how much we copy over
        );

        
        Buffer.BlockCopy(
            textBytes, // what we are copying
            0, // which index we are copying it from
            saltAndTextBytes, // what we copy it into
            salt.Length, // which index in saltAndTextBytes we want to copy into
            textBytes.Length // how much we copy over
        );

        byte[] hash = SHA256.HashData(saltAndTextBytes);

        return hash;
    }

    public static byte[] SaltAndHash(string message, string salt)
    {
        byte[] saltArray = Encoding.UTF8.GetBytes(salt);
        byte[] textBytes = Encoding.UTF8.GetBytes(message);
        byte[] saltAndTextBytes = new byte[salt.Length + textBytes.Length];


        Buffer.BlockCopy(
            saltArray, // what we are copying
            0, // which index we are copying it from
            saltAndTextBytes, // what we copy it into
            0, // which index in saltAndTextBytes we want to copy into
            salt.Length // how much we copy over
        );

        
        Buffer.BlockCopy(
            textBytes, // what we are copying
            0, // which index we are copying it from
            saltAndTextBytes, // what we copy it into
            salt.Length, // which index in saltAndTextBytes we want to copy into
            textBytes.Length // how much we copy over
        );

        byte[] hash = SHA256.HashData(saltAndTextBytes);

        return hash;
    }

    public static string SaltAndHashString(string message, byte[] salt)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(message);
        byte[] saltAndTextBytes = new byte[salt.Length + textBytes.Length];


        Buffer.BlockCopy(
            salt, // what we are copying
            0, // which index we are copying it from
            saltAndTextBytes, // what we copy it into
            0, // which index in saltAndTextBytes we want to copy into
            salt.Length // how much we copy over
        );

        
        Buffer.BlockCopy(
            textBytes, // what we are copying
            0, // which index we are copying it from
            saltAndTextBytes, // what we copy it into
            salt.Length, // which index in saltAndTextBytes we want to copy into
            textBytes.Length // how much we copy over
        );

        byte[] hash = SHA256.HashData(saltAndTextBytes);
        string messageHashString = Convert.ToBase64String(hash);
        return messageHashString;
    }

    
    public static string SaltAndHashString(string message, string salt)
    {
        byte[] saltArray = Encoding.UTF8.GetBytes(salt);
        byte[] textBytes = Encoding.UTF8.GetBytes(message);
        byte[] saltAndTextBytes = new byte[salt.Length + textBytes.Length];


        Buffer.BlockCopy(
            saltArray, // what we are copying
            0, // which index we are copying it from
            saltAndTextBytes, // what we copy it into
            0, // which index in saltAndTextBytes we want to copy into
            salt.Length // how much we copy over
        );

        
        Buffer.BlockCopy(
            textBytes, // what we are copying
            0, // which index we are copying it from
            saltAndTextBytes, // what we copy it into
            salt.Length, // which index in saltAndTextBytes we want to copy into
            textBytes.Length // how much we copy over
        );

        byte[] hash = SHA256.HashData(saltAndTextBytes);
        string messageHashString = Convert.ToBase64String(hash);
        return messageHashString;
    }
}
