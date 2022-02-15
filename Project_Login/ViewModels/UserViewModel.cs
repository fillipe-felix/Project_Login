﻿namespace Project_Login.ViewModels;

public class RegisterUserViewModel
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
}

public class LoginUserViewModel
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}

public class UserTokenViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<ClaimViewModel> Claims { get; set; }
}

public class LoginResponseViewModel
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserTokenViewModel UserToken { get; set; }
}

public class ClaimViewModel
{
    public string Value { get; set; }
    public string Type { get; set; }
}