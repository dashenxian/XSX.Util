﻿using System;

namespace XSX.Guids
{
    /// <summary>
    /// Implements <see cref="IGuidGenerator"/> by using <see cref="Guid.NewGuid"/>.
    /// </summary>
    public class SimpleGuidGenerator : IGuidGenerator
    {
        public virtual Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
