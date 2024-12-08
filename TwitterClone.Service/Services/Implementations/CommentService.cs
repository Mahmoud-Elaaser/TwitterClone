using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.CommentDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ResponseDto> GetAllCommentsAsync()
        {
            var comment = await _unitOfWork.Repository<Comment>().GetAllAsync();
            var mappedcomment = _mapper.Map<IEnumerable<GetCommentDto>>(comment);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Models = mappedcomment
            };
        }

        public async Task<ResponseDto> GetCommentByIdAsync(int id)
        {
            var comment = await _unitOfWork.Repository<Comment>().FindAsync(c => c.Id == id);
            if (comment == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Comment not found."
                };
            }
            var mappedComment = _mapper.Map<GetCommentDto>(comment);

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Model = mappedComment
            };
        }

        public async Task<ResponseDto> AddCommentAsync(CreateOrUpdateCommentDto createCommentDto)
        {
            var comment = _mapper.Map<Comment>(createCommentDto);
            comment.Createdat = DateTime.UtcNow;

            await _unitOfWork.Repository<Comment>().AddAsync(comment);
            await _unitOfWork.Complete();


            /// Notification: User with id: commented on your tweet
            await _notificationService.SendCommentNotificationAsync(comment.UserId, comment.TweetId);

            var mappedComment = _mapper.Map<GetCommentDto>(comment);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Model = mappedComment,
                Message = "Comment created successfully."
            };
        }

        public async Task<ResponseDto> UpdateCommentAsync(int id, CreateOrUpdateCommentDto updateCommentDto)
        {
            var existingComment = await _unitOfWork.Repository<Comment>().GetByIdAsync(id);
            if (existingComment == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Comment not found"
                };
            }

            _mapper.Map(updateCommentDto, existingComment);
            _unitOfWork.Repository<Comment>().Update(existingComment);
            await _unitOfWork.Complete();

            var mappedComment = _mapper.Map<GetCommentDto>(existingComment);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Comment updated successfully"
            };
        }



        public async Task<ResponseDto> DeleteCommentAsync(int id)
        {
            var comment = await _unitOfWork.Repository<Comment>().FindAsync(c => c.Id == id);
            if (comment == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "comment not found."
                };
            }

            _unitOfWork.Repository<Comment>().Delete(comment);
            await _unitOfWork.Complete();

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "comment deleted successfully."
            };
        }

        public async Task<ResponseDto> GetAllCommentsOnTweetAsync(int tweetId)
        {
            var comments = await _unitOfWork.Repository<Comment>().GetAllPredicated(t => t.TweetId == tweetId);

            if (comments == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Comments not found"
                };
            }

            var mappedComments = _mapper.Map<IEnumerable<GetCommentDto>>(comments);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Comments retrieved successfully",
                Models = mappedComments
            };
        }
    }
}
