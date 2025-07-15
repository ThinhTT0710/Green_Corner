using AutoMapper;
using GreenCorner.ChatAPI.Data;
using GreenCorner.ChatAPI.DTOs;
using GreenCorner.ChatAPI.Models;
using GreenCorner.ChatAPI.Repositories.Interface;
using GreenCorner.ChatAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.ChatAPI.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
        }

        public async Task SaveMessageAsync(ChatMessageDTO messageDto)
        {
            var message = _mapper.Map<ChatMessage>(messageDto);
            await _chatRepository.SaveMessageAsync(message);
        }

        public async Task<List<ChatMessageDTO>> GetMessagesByEventIdAsync(int eventId)
        {
            var messages = await _chatRepository.GetMessagesByEventIdAsync(eventId);
            return _mapper.Map<List<ChatMessageDTO>>(messages);
        }
    }
}
