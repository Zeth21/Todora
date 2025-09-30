using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Values
{
    public class IntegerValues
    {
        //2XX STATUS CODES
        public const int Ok = 200;
        public const int Created = 201;
        public const int NoContent = 204;
        public const int Accepted = 202;

        //4XX STATUS CODES
        public const int Unauthorized = 401;
        public const int BadRequest = 400;
        public const int Forbidden = 403;
    }
}
