using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using living_room_api.Data;

namespace living_room_api
{
    public interface IJwtAuthenticationManager
    {
        public Task<string> AuthenticateAsync(AppDbContext context, string username, string password);
    }
}
