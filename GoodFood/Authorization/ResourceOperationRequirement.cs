using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GoodFood.Authorization
{
    public enum Operation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperationRequirement(Operation operation)
        {
            Operation = operation;
        }
        public Operation Operation { get; }
    }
}
