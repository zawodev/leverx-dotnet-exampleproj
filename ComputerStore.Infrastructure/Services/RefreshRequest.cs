﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Infrastructure.Services {
    public class RefreshRequest {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
