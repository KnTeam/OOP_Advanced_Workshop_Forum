namespace Forum.App.Services
{
    using Forum.App.Contracts;
    using Forum.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            throw new NotImplementedException();
        }

        public void AddReplyToPost(int postId, string replyContents, int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICategoryInfoViewModel> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public string GetCategoryName(int categoryId)
        {
            string categoryName = this.forumData.Categories
                .FirstOrDefault(e => e.Id == categoryId).Name;

            if (categoryName == null)
            {
                throw new ArgumentException($"Category with category ID: {categoryId} not found!");
            }

            return categoryName;
        }

        public IEnumerable<IPostInfoViewModel> GetCategoryPostsInfo(int categoryId)
        {
            throw new NotImplementedException();
        }

        public IPostViewModel GetPostViewModel(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
