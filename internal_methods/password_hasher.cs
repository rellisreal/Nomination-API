using System.Security.Cryptography;
using System.Text;
namespace nomination_api.internal_methods; 
public class PasswordHasher
{
    private SHA256 mySHA256;
    StringBuilder output; 
    public PasswordHasher()
    {
        mySHA256 = SHA256.Create();
        output = new StringBuilder(); 
    }
    public string HashPassword(string password)
    {
        byte[] bytes; 
        bytes = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < bytes.Length; i++)
        {
            output.Append(bytes[i].ToString("x2"));
        }
        return output.ToString(); 
    }

}