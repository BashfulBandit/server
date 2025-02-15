﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Bit.Core.Models.Data;

namespace Bit.Core.Test.Repositories.EntityFramework.EqualityComparers
{
    public class UserKdfInformationCompare : IEqualityComparer<UserKdfInformation>
    {
        public bool Equals(UserKdfInformation x, UserKdfInformation y)
        {
            return x.Kdf == y.Kdf &&
                    x.KdfIterations == y.KdfIterations;
        }

        public int GetHashCode([DisallowNull] UserKdfInformation obj)
        {
            return base.GetHashCode();
        }
    }
}
