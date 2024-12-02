using System.Text;
using System.Security.Cryptography;

public static class PasswordHasher
{
    private static readonly int saltByteLen = 32;

    public static string HashPassword(string password)
    {
        // Generate a random salt
        // Salting adds a random string (salt) to the password before hashing it. This
        // ensures that even if two users have the same password, the resulting hash will
        // be different due to the unique salt.
        byte[] salt = new byte[saltByteLen];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) // make a new instance of an rng generator
        {
            rng.GetBytes(salt); // files the byte array 
        }

        byte[] passwordHash = SaltAndHash(password, salt);

        // as we store a base 64 string as the password in the db
        return Convert.ToBase64String(passwordHash);
    }


    public static bool VerifyPassword(string providedPassword, string hashedPasswordStr)
    {
        // the user password is stored as a concat of the hashed, salted password as a
        // base 64 string, convert back to bytes
        byte[] hashedPassword = Convert.FromBase64String(hashedPasswordStr);

        // parse into other arrays
        byte[] salt = new byte[saltByteLen];
        Buffer.BlockCopy(hashedPassword, 0, salt, 0, salt.Length);
        byte[] hashedProvidedPassword = SaltAndHash(providedPassword, salt);
                    
        return CompareHashes(hashedProvidedPassword, hashedPassword);
    }

    // Combine the salt and password, then hash them together

    private static byte[] SaltAndHash(string text, byte[] salt)
    {
        // A byte array containing the results of encoding the specified set of characters https://learn.microsoft.com/en-us/dotnet/api/system.text.encoding.getbytes?view=net-9.0#system-text-encoding-getbytes(system-char())
        // System.Text
        byte[] textBytes = Encoding.UTF8.GetBytes(text); 
        byte[] saltedBytes = new byte[salt.Length + textBytes.Length]; // new byt array to hold the union of the salt and the password

        // copy over the data into the byte array
        // https://learn.microsoft.com/en-us/dotnet/api/system.buffer.blockcopy?view=net-8.0
        Buffer.BlockCopy(salt, 0, saltedBytes, 0, salt.Length);
        Buffer.BlockCopy(textBytes, 0, saltedBytes, salt.Length, textBytes.Length);
        // Compute the hash
        // https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.sha256?view=net-9.0
        // https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1850
        // The hash is used as a unique value of fixed size representing a large amount
        // of data. Hashes of two sets of data should match if and only if the 
        // corresponding data also matches. Small changes to the data result in
        // large unpredictable changes in the hash.
        //
        // Hashing is a one-way cryptographic operation that converts a password into a
        // fixed-length string, called a hash. This hash is not reversible, meaning you
        // cannot derive the original password from it.
        byte[] hash = SHA256.HashData(textBytes);
        // SHA256 shouldn't really be used
        // https://stackoverflow.com/questions/11624372/best-practice-for-hashing-passwords-sha256-or-sha512
        return hash;
    }

    private static bool CompareHashes(byte[] hash1, byte[] hash2)
    {
        if (hash1.Length != hash2.Length)
        {
            return false;
        }

        for (int i = 0; i< hash1.Length; i++)
        {
            if(hash1[i] != hash2[i])
            {
                return false;
            }
        }

        return true;
    }
}