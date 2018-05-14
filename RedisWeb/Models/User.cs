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
    public class User
    {
        public long Id { set; get; }

        public string Name { set; get; }

        public DateTime CreateTime { set; get; }

        public DateTime UpdateTime { set; get; }

        public string Picture { set; get; }

    }
}