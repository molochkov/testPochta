using AutoMapper;
using CoreTestPochta.Models;
using PublisherConsole.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublisherConsole.Infrastructure
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            this.CreateMap<Message, MessageTransfer>();
        }
    }
}
