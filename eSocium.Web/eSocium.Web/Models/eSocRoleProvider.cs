using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Web.Models.DAL;
using System.Web.Security;

namespace eSocium.Web.Models
{
    public class eSocRoleProvider : RoleProvider
    {
        UserContext db = new UserContext();

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return db.Roles.Select(r => r.Name).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            if (db.Users.Count() == 0)
            {
                return new string[0];
            }
            var user = db.Users.First(u => u.Login.Equals(username));
            if (user == null)
            {
                throw new Exception("Не найден пользователь " + username);
            }
            List<string> result = new List<string>();
            result.Add(db.Roles.Find(user.Role).Name);
            return result.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var roleId = db.Roles.First(r => r.Name.Equals(roleName));
            if (roleId == null)
            {
                throw new Exception("Не найдена роль " + roleName);
            }
            List<string> result = new List<string>();
            result.AddRange(db.Users.Where(u => u.Role.Equals(roleId)).Select(x => x.Login).ToList());
            return result.ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var user = db.Users.First(u => u.Login.Equals(username));
            if (user == null)
            {
                throw new Exception("Не найден пользователь " + username);
            }
            return user.Role == db.Roles.Find(user.Role).RoleId;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}