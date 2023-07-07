using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LearnAspNetCoreMVC.Models
{
    public class APIResponse
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
