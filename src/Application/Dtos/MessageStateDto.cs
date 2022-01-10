using System.Text.Json.Serialization;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Kibrit.Smpp.Common;

namespace Application.Dtos
{
    public record MessageStateDto : IMapFrom<Message>
    {
        [JsonPropertyName("id")]
        public string Id { get; init; }
        [JsonPropertyName("src")]
        public string Source { get; init; }
        [JsonPropertyName("dst")]
        public string Destination { get; init; }
        [JsonPropertyName("text")]
        public string Text { get; init; }
        [JsonPropertyName("tag")]
        public string Tag { get; init; }
        [JsonPropertyName("state")]
        public MessageState State { get; set; }
        [JsonPropertyName("ts")] 
        public string Timestamp { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Message, MessageStateDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));
        }
    }
}