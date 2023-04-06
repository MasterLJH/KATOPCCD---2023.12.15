using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KATOPCCD.Model
{
    [Serializable]
    public class Person
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Level { get; set; }

        public Person(string username,string password,int level)
        {
            UserName = username;
            PassWord = password;
            Level = level;
        }

        public Person(string username, string password)
        {
            UserName = username;
            PassWord = password;      
        }

        public bool IsSame( Person p)
        {
            if (UserName == p.UserName && PassWord == p.PassWord)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsSameUserName(Person p)
        {
            if (UserName == p.UserName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
