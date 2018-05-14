using RedisWeb.Cache;
using RedisWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace RedisWeb.Services
{
    public class IndexService
    {
        private static MemoryCacheManager memoryCacheManager;
        private RedisCacheManager redisCacheManager;
        private List<Blog> blogList;
        private List<User> userList;
        private List<RedisWeb.Models.Comment> commentList;
        public IndexService()
        {
            memoryCacheManager = MemoryCacheManager.GetMemoryCacheManager();
            redisCacheManager = RedisCacheManager.GetRedisCacheManager();

            userList = new List<User>();
            userList.Add(new Models.User
            {
                CreateTime = DateTime.Now,
                Id = 1,
                Name = "zhangsan",
                Picture = "",
                UpdateTime = DateTime.Now
            });
            userList.Add(new Models.User
            {
                CreateTime = DateTime.Now,
                Id = 2,
                Name = "lisi",
                Picture = "",
                UpdateTime = DateTime.Now
            });
            userList.Add(new Models.User
            {
                CreateTime = DateTime.Now,
                Id = 3,
                Name = "wangwu",
                Picture = "",
                UpdateTime = DateTime.Now
            });

            blogList = new List<Blog>();
            blogList.Add(new Blog
            {
                Content = "text blog by zhangsan",
                CreateBy = 1,
                CreateTime = DateTime.Now,
                Id = 1,
                Title = "text blog by zhangsan",
                UpdateTime = DateTime.Now
            });
            blogList.Add(new Blog
            {
                Content = "text blog by lisi",
                CreateBy = 2,
                CreateTime = DateTime.Now,
                Id = 2,
                Title = "text blog by lisi",
                UpdateTime = DateTime.Now
            });
            blogList.Add(new Blog
            {
                Content = "text blog by wangwu",
                CreateBy = 3,
                CreateTime = DateTime.Now,
                Id = 3,
                Title = "text blog by wangwu",
                UpdateTime = DateTime.Now
            });

            commentList = new List<Models.Comment>();
            commentList.Add(new Models.Comment
            {
                BlogId = 1,
                Content = "text comment for zhangsan's blog ",
                CreateBy = 2,
                CreateTime = DateTime.Now,
                Id = 1
            });


        }

        public IEnumerable<Blog> GetBlogData()
        {
            var data = memoryCacheManager.Get<List<Blog>>("blogs");
            if (data == null)
            {
                data = redisCacheManager.Get<List<Blog>>("blogs");
                if (data == null)
                {
                    SetRedisAndMemoryCacheData();
                }
                else
                {
                    memoryCacheManager.Set("blogs", blogList,10.00d);
                }
            }
            return data;
        }

        private List<Blog> SetRedisAndMemoryCacheData()
        {
            redisCacheManager.Set("blogs", blogList, TimeSpan.Parse("10"));
            redisCacheManager.Set("comments", commentList, TimeSpan.Parse("10"));
            redisCacheManager.Set("users", userList, TimeSpan.Parse("10"));

            blogList = blogList.Select(m =>
              {
                  var dto = m;
                  dto.CommentList = commentList.Where(c => c.BlogId == m.Id).ToList();
                  dto.CommentList.Select(item =>
                  {
                      item.User = userList.FirstOrDefault(u => u.Id == item.CreateBy);
                      return item;
                  }).ToList();
                  dto.User = userList.FirstOrDefault(u => u.Id == m.CreateBy);
                  return dto;
              }).ToList();

            memoryCacheManager.Set("blogs", blogList, TimeSpan.Parse("10"));
            return blogList;
        }
    }
}