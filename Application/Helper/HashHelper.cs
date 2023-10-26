using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper;

public class HashHelper
{
    public static string EncryptPassword(string password)
    {
        // divide by 8 to convert bits to bytes
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); 

        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8)
            );

        return hashedPassword;
    }
}
