using Newtonsoft.Json;
using TransferService.Domain.Common;
using System;

namespace TransferService.Api.ViewModels
{
    public abstract class BaseViewModel : IIdProperty
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
