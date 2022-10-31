using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using WebApplication1.Models;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace WebApplication1.Controllers

{
    public abstract class BaseController : ControllerBase
{
    protected readonly IConfiguration _configuration;
    protected readonly IWebHostEnvironment _env;
    protected readonly IMemoryCache _cache;
    protected MySqlConnection con;

    public BaseController(IConfiguration configuration, IWebHostEnvironment env, IMemoryCache cache)
    {
        _configuration = configuration;
        _env = env;
        _cache = cache;
        string connStr = _configuration.GetConnectionString("ConnectionStringForDatabase");
        con = new MySqlConnection(connStr);

        }
    protected bool IsAuthenticated()
    {
        var requestToken = HttpContext.Request.Headers["AuthToken"].ToArray().FirstOrDefault().ToString();
            requestToken = requestToken.Substring(1, requestToken.Length - 2);
        var sessionToken = this.GetToken().ToString();
        return requestToken == sessionToken;
    }
    protected string GetUser(string username, string password)
    {
            string user = null;
            try
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT user_name, user_password from erp_system_db.user where user_name ='" + username + "' and user_password ='" + password + "'", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) 
                {
                    user = (reader["user_name"].ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MySql " + ex.ToString());
            }
            con.Close();
            return user;
    }
    protected bool IsValid(string username, string password)
    {
        var user = this.GetUser(username, password);
        return user != null;
    }
    protected string CreateToken()
    {
        return Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
    }
    protected string GetToken()
    {
        object cacheEntry = "";
        _cache.TryGetValue("AuthToken" + GetClientIP(), out cacheEntry);
        return (cacheEntry ?? "").ToString();
    }
    protected string SetNewToken()
    {
        var token = this.CreateToken();
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));
        _cache.Set("AuthToken" + GetClientIP(), token, cacheEntryOptions);
        return token;
    }
    protected string GetClientIP()
    {
        var remoteIpAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
        return remoteIpAddress.ToString();
    }
    protected string Authenticate(string username, string password)
    {
        if (this.IsValid(username, password))
        {
            Debug.WriteLine("Authentification Succeed");
            return SetNewToken();
        }
        else
        {
            Debug.WriteLine("Authentification failed");
            return null;
        }
    }
    protected void CheckAuthentication()
    {
        string currentSessionToken = this.GetToken();
        if (currentSessionToken == null || currentSessionToken.Trim() == "")
            this.SetNewToken();
        if (!this.IsAuthenticated())
            throw new Exception("User not authenticated. Please log in.");
    }
}
}