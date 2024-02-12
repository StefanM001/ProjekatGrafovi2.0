using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatGrafovi.ResourceFolder
{
    public class LocalizedStrings
    {
        private static ParametersResource parametersResource = new ParametersResource();

        public ParametersResource ParametersResource { get => parametersResource; set => parametersResource = value; }
    }
}
