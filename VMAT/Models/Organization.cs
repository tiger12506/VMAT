using System.Collections.Generic;

namespace VMAT.Models
{
    public class Organization
    {
        public string Name { get; set; }
        public List<string> Authors { get; set; }
        public string LogoFile { get; set; }
    }
}
