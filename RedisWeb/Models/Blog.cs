using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedisWeb.Models
{
    public class Blog
    {
        public long Id { set; get; }

        public string Title { set; get; }

        public string Content { set; get; }

        public DateTime CreateTime { set; get; }

        public DateTime UpdateTime { set; get; }

        public long CreateBy { set; get; }

        public List<Comment> CommentList { set; get; }

        public User User { set; get; }

    }
}