using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorPatient.Validations
{
    public class DomainValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string host;
            string address = value.ToString();

            // using Split
            host = address.Split('@')[1];

            // using Split with maximum number of substrings (more explicit)
            host = address.Split(new char[] { '@' }, 2)[1];

            // using Substring/IndexOf
            host = address.Substring(address.IndexOf('@') + 1);
            if (host == "hcl.com") return true;
            return false;
        }
    }
}
