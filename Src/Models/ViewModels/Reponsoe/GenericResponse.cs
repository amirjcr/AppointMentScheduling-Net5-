using System;
using System.Collections.Generic;

namespace Src.Models.ViewModels.Response
{
    public class GenericResponse<T>
    {
        public int Status { get; set; }
        public string message { get; set; }
        public T dataenum { get; set; }
    }
}