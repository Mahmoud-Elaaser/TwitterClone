﻿namespace TwitterClone.Service.DTOs.LikeDto
{
    public class CreateLikeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TweetId { get; set; }
    }
}
