using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedisWeb.Models
{
    /**
      * 
      * blog: id, title, content, createtime, updatetime, createby
      * user: id, name, createtime, updatetime, picture
      * comment: id, blogid, createby,createtime
      * **/
    public class Comment
    {
        public long Id { set; get; }

        public long BlogId { set; get; }

        public long CreateBy { set; get; }

        public DateTime CreateTime { set; get; }

        public string Content { set; get; }

        public User User { set; get; }

    }
}