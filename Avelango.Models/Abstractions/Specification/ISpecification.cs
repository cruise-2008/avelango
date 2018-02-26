﻿using System;
using System.Linq.Expressions;

namespace Avelango.Models.Abstractions.Specification
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>> SatisfiedBy();
    }
}


