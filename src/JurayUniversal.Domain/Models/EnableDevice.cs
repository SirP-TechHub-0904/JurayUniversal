using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class EnableDevice
    {
        public long Id { get; set; }

        [Required]
        public string UserInformation { get; set; }

        [Required]
        public string DeviceInformation { get; set; }

        [Required]
        public bool DisableDevice { get; set; }
    }
}
