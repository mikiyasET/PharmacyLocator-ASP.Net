using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PharmacyLocator.Base
{
    public interface IEntityBase
    {
        long Id { get; set; }

    }
}
