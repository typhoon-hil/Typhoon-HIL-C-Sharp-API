﻿using System;

namespace TyphoonHil.Exceptions
{
    public class HilAPIException : Exception
    {
        public HilAPIException()
        {
        }

        public HilAPIException(string message) : base(message)
        {
        }
    }
}