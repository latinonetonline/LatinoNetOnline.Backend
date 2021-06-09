using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Emails
{
    public record Email

    {
        private readonly string _email;

        public Email(string email)
        {

            if (!ValidateEmail(email))
            {
                throw new ArgumentException(null, nameof(email));
            }

            _email = email;
        }

        static public bool ValidateEmail(string email)
        {
            string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"

               + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"

               + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"

               + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"

               + @"[a-zA-Z]{2,}))$";

            Regex regexStrict = new(patternStrict);

            return regexStrict.IsMatch(email);
        }

        public override string ToString()
            => _email;




        public override int GetHashCode()
            => _email.ToLower().GetHashCode();

    }
}
