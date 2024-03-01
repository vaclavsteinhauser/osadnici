using Microsoft.AspNetCore.Identity;

public class Hrac : IdentityUser
{
    public override bool Equals(object? obj)
    {
        IdentityUser hrac = obj as IdentityUser;
        if (hrac == null)
            return false;

        return hrac.Id==Id;
    }
    public string? Name { get; set; }
}