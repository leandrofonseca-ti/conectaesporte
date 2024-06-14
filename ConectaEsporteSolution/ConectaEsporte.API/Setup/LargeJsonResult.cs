using Microsoft.AspNetCore.Mvc;

namespace ConectaEsporte.API.Setup
{
    public class LargeJsonResult : JsonResult
    {
        private object? _value;
        public LargeJsonResult(object? value = null) : base(value)
        {
            this.ContentType = "application/json";
            _value = value;
        }



    }
}
