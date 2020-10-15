namespace SubscriberConsole.Infrastructure
{
    using AutoMapper;
    using CoreTestPochta.Models;
    using SubscriberConsole.Models.Database;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            this.CreateMap<MessageTransfer, Message>();
        }
    }
}

