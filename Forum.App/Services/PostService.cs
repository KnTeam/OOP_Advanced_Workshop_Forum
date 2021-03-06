﻿namespace Forum.App.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Forum.App.Contracts;
    using Forum.App.Contracts.ViewModels;
    using Forum.Data;
    using Forum.DataModels;

    public class PostService : IPostService
    {
        private ForumData forumData;
        private IUserService userService;

        public PostService(ForumData forumData, IUserService userService)
        {
            this.forumData = forumData;
            this.userService = userService;
        }

        public int AddPost(int userId, string postTitle, string postCategory, string postContent)
        {
            bool emptyCategory = string.IsNullOrWhiteSpace(postCategory);
            bool emptyTitle = string.IsNullOrWhiteSpace(postTitle);
            bool emptyContect = string.IsNullOrWhiteSpace(postContent);

            if (emptyCategory || emptyTitle || emptyContect)
            {
                throw new ArgumentException("All fields must be filled!!");
            }

            Category category = this.EnsureCategory(postCategory);
            int postId = this.forumData.Posts.Any() ? this.forumData.Posts.Last().Id + 1 : 1;
            User author = this.userService.GetUserById(userId);
            Post post = new Post(postId, postTitle, postContent, category.Id, userId, new List<int>());

            this.forumData.Posts.Add(post);
            author.Posts.Add(post.Id);
            category.Posts.Add(post.Id);
            this.forumData.SaveChanges();

            return post.Id;
        }

        public void AddReplyToPost(int postId, string replyContents, int userId)
        {
            Post post = this.forumData.Posts.FirstOrDefault(e => e.Id == postId);

            if (post != null)
            {
                int replyId = this.forumData.Replies.Any() ? this.forumData.Posts.Last().Id + 1 : 1;
                Reply reply = new Reply(replyId, replyContents, userId, postId);

                this.forumData.Replies.Add(reply);
                post.Replies.Add(replyId);
                this.forumData.SaveChanges();
            }
        }

        public IEnumerable<ICategoryInfoViewModel> GetAllCategories()
        {
            var categories = this.forumData.Categories
                .Select(category => new CategoryInfoViewModel(category.Id, category.Name, category.Posts.Count));
            return categories;
        }

        public string GetCategoryName(int categoryId)
        {
            var categoryName = this.forumData.Categories
                .Find(c => c.Id == categoryId)?.Name;

            if (categoryName == null)
            {
                throw new ArgumentException($"Category with category ID: {categoryId} not found!");
            }

            return categoryName;
        }

        public IEnumerable<IPostInfoViewModel> GetCategoryPostsInfo(int categoryId)
        {
            IEnumerable<IPostInfoViewModel> posts = this.forumData.Posts
                .Where(e => e.CategoryId == categoryId)
                .Select(e => new PostInfoViewModel(e.Id, e.Title, e.Replies.Count));

            return posts;
        }

        public IPostViewModel GetPostViewModel(int postId)
        {
            var post = this.forumData.Posts.FirstOrDefault(p => p.Id == postId);
            IPostViewModel postView = new PostViewModel(post.Title, this.userService.GetUserName(post.AuthorId), post.Content, this.GetPostReplies(postId));

            return postView;
        }

        private IEnumerable<IReplyViewModel> GetPostReplies(int postId)
        {
            IEnumerable<IReplyViewModel> replies = this.forumData.Replies
                .Where(r => r.PostId == postId)
                .Select(r => new ReplyViewModel(this.userService.GetUserName(r.AuthorId), r.Content));

            return replies;
        }

        private Category EnsureCategory(string postCategory)
        {
            Category category = this.forumData.Categories.FirstOrDefault(e => e.Name == postCategory);

            if (category == null)
            {
                int categoryId = this.forumData.Categories.LastOrDefault()?.Id + 1 ?? 1;
                category = new Category(categoryId, postCategory, new List<int>());
                this.forumData.Categories.Add(category);
            }

            return category;
        }
    }
}
