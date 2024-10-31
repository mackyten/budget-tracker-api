using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Supabase;

namespace BT.SERVICES.SupabaseService
{
    public class SupabaseService
    {
        private readonly Client _client;

        public SupabaseService(IConfiguration configuration)
        {
            var url = configuration["Supabase:Url"];
            var key = configuration["Supabase:Key"];
            _client = new Supabase.Client(url, key);
        }

        public Supabase.Client GetClient() => _client;
    }
}