using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace UsefulServices.Services
{
    public class ServiceActionResult
    {
        public ServiceActionResult(string error)
        {
            Succeeded = false;
            Errors = new List<string> { error };
        }

        public ServiceActionResult(IdentityResult result)
        {
            if (result.Errors.Any())
            {
                Succeeded = false;
                Errors = result.Errors.Select(e => e.Description);
            }
        }

        public ServiceActionResult() { }

        public bool Succeeded { get; set; } = true;
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public string GetErrorsMessage()
        {
            if (Errors.Any() && !Succeeded)
            {
                if (Errors.Count() == 1)
                {
                    return Errors.First();
                }
                else
                {
                    return "Multiple Errors: " + string.Join("|", Errors);
                }
            }
            return string.Empty;
        }
    }

}
